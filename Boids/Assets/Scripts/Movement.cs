using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Settings;

public class Movement : MonoBehaviour
{
    public float maxSpeed;

    // defines how fast a Boid can steer (change direction)
    public float maxSteeringForce;

    public bool isAvoidingWalls;
    public bool isAvoidingOthers;
    public bool isAligningWithOthers;
    public bool isUsingCohesion;

    private Vector3 steeringForce;
    private Rigidbody body;

    Vector3 targetPosition = Vector3.zero;

    private Vector3 velocityDifference;

    // distance around target within the boid starts to slow down (approach target)
    private float slowDownDistance = 10f;

    private GameObject swarm;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        swarm = GameObject.Find("Swarm");
    }

    void FixedUpdate()
    {
        RuleConfigurator ruleConfigurator = swarm.GetComponent<RuleConfigurator>();

        if (ruleConfigurator.doFollowMouse)
        {
            targetPosition = GetMousePosition();
        }
        else
        {
            targetPosition = transform.position + StraightWalkOffset();
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position, Vector3.up), 0.08f);

        body.AddForce(Seek(targetPosition, true));


        if (ruleConfigurator.isAvoidingWalls)
        {
            body.AddForce(ruleConfigurator.avoidWallsScale * AvoidWalls());
        }
        if (ruleConfigurator.isAvoidingNeighbors)
        {
            body.AddForce(ruleConfigurator.avoidNeighborsScale * AvoidNeighbors());
        }
        if (ruleConfigurator.isAligningWithNeighbors)
        {
            body.AddForce(ruleConfigurator.alignWithNeighborsScale * AlignWithNeighbors());
        }
        if (ruleConfigurator.isUsingCohesion)
        {
            body.AddForce(ruleConfigurator.useCohesionScale * Cohesion());
        }
    }

    private Vector3 StraightWalkOffset()
    {
        return body.velocity.normalized * maxSpeed;
    }

    private Vector3 AvoidWalls()
    {
        bool isOutside = false;
        float offset = 1f;
        Vector3 currentPosition = transform.position;
        Vector3 desiredPosition = transform.position;

        // X
        if (currentPosition.x < Settings.current.GetMinPosition(Dimension.X))
        {
            desiredPosition.x = Settings.current.GetMinPosition(Dimension.X) + offset;
            isOutside = true;
        }
        else if (currentPosition.x > Settings.current.GetMaxPosition(Dimension.X))
        {
            desiredPosition.x = Settings.current.GetMaxPosition(Dimension.X) - offset;
            isOutside = true;
        }

        // Y
        if (currentPosition.y < Settings.current.GetMinPosition(Dimension.Y))
        {
            desiredPosition.y = Settings.current.GetMinPosition(Dimension.Y) + offset;
            isOutside = true;
        }
        else if (currentPosition.y > Settings.current.GetMaxPosition(Dimension.Y))
        {
            desiredPosition.y = Settings.current.GetMaxPosition(Dimension.Y) - offset;
            isOutside = true;
        }

        // Z
        if (currentPosition.z < Settings.current.GetMinPosition(Dimension.Z))
        {
            desiredPosition.z = Settings.current.GetMinPosition(Dimension.Z) + offset;
            isOutside = true;
        }
        else if (currentPosition.z > Settings.current.GetMaxPosition(Dimension.Z))
        {
            desiredPosition.z = Settings.current.GetMaxPosition(Dimension.Z) - offset;
            isOutside = true;
        }

        if (isOutside)
        {
            Debug.DrawLine(transform.position, desiredPosition);
            return Seek(desiredPosition, false);
        }
        return Vector3.zero;
    }

    private Vector3 AvoidNeighbors()
    {
        List<Transform> neighbors = GetComponent<NeighborFinder>().GetNeighbors();
        Vector3 separationVector = Vector3.zero;

        foreach (var neighbor in neighbors)
        {
            float scale = (transform.position - neighbor.position).magnitude;
            separationVector += ((transform.position - neighbor.position).normalized) / scale;
        }

        if (neighbors.Count > 0)
        {
            separationVector /= neighbors.Count;
        }

        Vector3 target = transform.position + separationVector * maxSpeed;
        Debug.DrawLine(transform.position, target);
        return Seek(target, false);
    }

    private Vector3 AlignWithNeighbors()
    {
        List<Transform> neighbors = GetComponent<NeighborFinder>().GetNeighbors();
        Vector3 averageVelocity = Vector3.zero;
        foreach (var boid in neighbors)
        {
            averageVelocity += boid.GetComponent<Rigidbody>().velocity;
        }

        int amountOfNeighbors = neighbors.Count;
        if (amountOfNeighbors > 0)
        {
            averageVelocity /= neighbors.Count;
        }

        Vector3 target = transform.position + averageVelocity;
        Debug.DrawLine(transform.position, target, Color.green);
        return Seek(target, false);
    }

    private Vector3 Cohesion()
    {
        List<Transform> neighbors = GetComponent<NeighborFinder>().GetNeighbors();
        Vector3 center = Vector3.zero;
        foreach (var boid in neighbors)
        {
            center += boid.position;
        }

        int amountOfNeighbors = neighbors.Count;
        if (amountOfNeighbors > 0)
        {
            center /= neighbors.Count;
        }

        Vector3 target = center;
        Debug.DrawLine(transform.position, target, Color.blue);
        return Seek(target, false);
    }

    private Vector3 Seek(Vector3 target, bool approachSlowly)
    {
        Vector3 desiredVelocity = target - transform.position;
        Vector3 limitedVelocity;

        // slow down, when close enough to the target
        float distanceToTarget = desiredVelocity.magnitude;
        if (approachSlowly && distanceToTarget < slowDownDistance)
        {
            float variableSpeed = map(distanceToTarget, 0, slowDownDistance, 0, maxSpeed);
            limitedVelocity = desiredVelocity.normalized * variableSpeed;
        }
        // otherwise go with maximal speed
        else
        {
            limitedVelocity = desiredVelocity.normalized * maxSpeed;
        }

        velocityDifference = limitedVelocity - body.velocity;
        steeringForce = Vector3.ClampMagnitude(velocityDifference, maxSteeringForce);
        return steeringForce;
    }

    private float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void OnDrawGizmosSelected()
    {
        // actual velocity
        drawLineHelper(transform.position, transform.position + body.velocity, new Color(0, 0, 220), 15);

        // velocity difference
        drawLineHelper(transform.position, transform.position + velocityDifference, new Color(220, 220, 220, 0.5f), 15);

        // steering
        drawLineHelper(transform.position, transform.position + steeringForce, new Color(0, 220, 0, 0.5f), 5);

        Gizmos.DrawWireSphere(targetPosition, slowDownDistance);
    }

    private void drawLineHelper(Vector3 startPos, Vector3 endPos, Color color, float thickness)
    {
        UnityEditor.Handles.DrawBezier(startPos, endPos, startPos, endPos, color, null, thickness);
    }

    private Vector3 GetMousePosition()
    {
        var plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            // some point of the plane was hit - get its coordinates
            var hitPoint = ray.GetPoint(distance);
            return new Vector3(hitPoint.x, 0, hitPoint.z);
        }
        return Vector3.zero;
    }


}
