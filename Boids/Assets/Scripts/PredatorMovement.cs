using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Settings;

public class PredatorMovement : Movement
{
    public void Start()
    {
        body = GetComponent<Rigidbody>();
        maxSpeed = 10;
        maxSteeringForce = 10;
    }

    public void FixedUpdate()
    {
        Vector3 targetPosition = targetPosition = transform.position + StraightWalkOffset();
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position, Vector3.up), 0.08f);

        body.AddForce(Seek(targetPosition, true));
        body.AddForce(AvoidWalls());
    }

}