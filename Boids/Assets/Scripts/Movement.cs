﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Settings;

public class Movement : MonoBehaviour
{
    protected float maxSpeed;
    protected float maxSteeringForce;

    protected Rigidbody body;

    private Vector3 targetPosition = Vector3.zero;


    // distance around target within the boid starts to slow down (approach target)
    protected float slowDownDistance = 10f;

    private GameObject swarm;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        swarm = GameObject.Find("Swarm");
    }

    void FixedUpdate()
    {
        SwarmConfigurator configurator = Settings.current.GetComponent<SwarmConfigurator>();
        maxSpeed = configurator.maxSpeed;
        maxSteeringForce = configurator.maxSteeringForce;

        if (Input.GetMouseButton(0))
        {
            targetPosition = GetMousePosition();
        }
        else
        {
            targetPosition = transform.position + StraightWalkOffset();
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position, Vector3.up), 0.08f);

        body.AddForce(Seek(targetPosition, true));


        if (configurator.isAvoidingWalls)
        {
            body.AddForce(configurator.avoidWallsScale * AvoidWalls());
        }
        if (configurator.isAvoidingNeighbors)
        {
            body.AddForce(configurator.avoidNeighborsScale * AvoidNeighbors());
        }
        if (configurator.isAligningWithNeighbors)
        {
            body.AddForce(configurator.alignWithNeighborsScale * AlignWithNeighbors());
        }
        if (configurator.isUsingCohesion)
        {
            body.AddForce(configurator.useCohesionScale * Cohesion());
        }
        if (configurator.isAvoidingPredators)
        {
            body.AddForce(configurator.avoidPredatorsScale * AvoidPredators());
        }
    }

    protected Vector3 StraightWalkOffset()
    {
        return body.velocity.normalized * maxSpeed;
    }

    protected Vector3 AvoidWalls()
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
            Debug.DrawLine(transform.position, desiredPosition, Color.red);
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
            float scale = (transform.position - neighbor.position).sqrMagnitude;
            separationVector += ((transform.position - neighbor.position).normalized) / scale;
        }

        if (neighbors.Count > 0)
        {
            separationVector /= neighbors.Count;
        }

        Vector3 target = transform.position + separationVector * maxSpeed;
        Debug.DrawLine(transform.position, target, new Color(128, 0, 0));
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
        Debug.DrawLine(transform.position, target, new Color(0, 200, 0, 0.5f));
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
        Debug.DrawLine(transform.position, target, new Color(25, 25, 25, 0.5f));
        return Seek(target, false);
    }


    private Vector3 AvoidPredators()
    {
        List<Transform> predators = GetComponent<NeighborFinder>().GetPredators();
        if (predators.Count == 0)
        {
            return Vector3.zero;
        }
        return -Seek(predators[0].position, false);
    }

    protected Vector3 Seek(Vector3 target, bool approachSlowly)
    {
        Vector3 desiredVelocity = target - transform.position;
        Vector3 limitedVelocity;

        // slow down, when close enough to the target
        float sqrDistanceToTarget = desiredVelocity.sqrMagnitude;
        if (approachSlowly && sqrDistanceToTarget < slowDownDistance * slowDownDistance)
        {
            float variableSpeed = map(sqrDistanceToTarget, 0, slowDownDistance * slowDownDistance, 0, maxSpeed);
            limitedVelocity = desiredVelocity.normalized * variableSpeed;
        }
        // otherwise go with maximal speed
        else
        {
            limitedVelocity = desiredVelocity.normalized * maxSpeed;
        }
        return Vector3.ClampMagnitude(limitedVelocity - body.velocity, maxSteeringForce);
    }

    protected float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
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
            Vector3 position = new Vector3(hitPoint.x, 0, hitPoint.z);

            // GameObject sphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            // sphere.transform.position = position;
            // Destroy(sphere, 0.1f);
            return position;
        }
        return Vector3.zero;
    }


}
