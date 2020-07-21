using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Settings;

public class Movement : MonoBehaviour
{
    private float maxSpeed = 150;

    // defines how fast a Boid can steer (change direction)
    private float maxSteeringForce = 500;

    private Vector3 steeringForce;
    private Rigidbody body;

    Vector3 targetPosition = Vector3.zero;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.anyKeyDown)
        {
            float x = Random.Range(Settings.current.GetMinPosition(Dimension.X), Settings.current.GetMaxPosition(Dimension.X));
            float y = Random.Range(Settings.current.GetMinPosition(Dimension.Y), Settings.current.GetMaxPosition(Dimension.Y));
            float z = Random.Range(Settings.current.GetMinPosition(Dimension.Z), Settings.current.GetMaxPosition(Dimension.Z));
            targetPosition = new Vector3(x, y, z);
        }
        MoveTo(targetPosition);
    }

    private void MoveTo(Vector3 target)
    {
        Vector3 desiredVelocity = target - transform.position;
        Vector3 limitedVelocity;

        float slowDownDistance = 10f;
        // Slow down, when close enough to the target
        if (desiredVelocity.magnitude < slowDownDistance)
        {
            float variableSpeed = map(desiredVelocity.magnitude, 0, slowDownDistance, 0, maxSpeed);
            limitedVelocity = desiredVelocity.normalized * variableSpeed;
        }
        // otherwise go with maximal speed
        else
        {
            limitedVelocity = desiredVelocity.normalized * maxSpeed;
        }

        Debug.Log("Desired: " + desiredVelocity.magnitude);
        Debug.Log("Limited: " + limitedVelocity.magnitude);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(limitedVelocity, Vector3.up), 0.08f);
        steeringForce = Vector3.ClampMagnitude(limitedVelocity - body.velocity, maxSteeringForce);
        body.AddForce(steeringForce);
    }

    private float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 220, 0);
        Gizmos.DrawLine(transform.position, transform.position + steeringForce);
        Gizmos.DrawWireSphere(targetPosition, 1);
    }

}
