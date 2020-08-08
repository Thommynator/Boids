using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleConfigurator : MonoBehaviour
{
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

    public bool doFollowMouse = true;
    public GameObject swarm;
}
