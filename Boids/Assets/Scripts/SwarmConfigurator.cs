using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmConfigurator : MonoBehaviour
{


    [Range(1, 1000)]
    public int swarmSize;

    [Range(0.0f, 30.0f)]
    public float maxSpeed;

    [Range(0.0f, 30.0f)]
    public float maxSteeringForce;

    [Range(0, 30)]
    public float neighborSearchRadius;


    [Range(0.0f, 5.0f)]
    public float avoidWallsScale;
    public bool isAvoidingWalls = true;

    [Range(0.0f, 5.0f)]
    public float avoidNeighborsScale;
    public bool isAvoidingNeighbors = true;

    [Range(0.0f, 5.0f)]
    public float alignWithNeighborsScale;
    public bool isAligningWithNeighbors = true;

    [Range(0.0f, 5.0f)]
    public float useCohesionScale;
    public bool isUsingCohesion = true;

    public float avoidPredatorsScale;
    public bool isAvoidingPredators;

    public GameObject swarm;
}
