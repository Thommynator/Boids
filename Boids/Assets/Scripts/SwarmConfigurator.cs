using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwarmConfigurator : MonoBehaviour
{

    public GameObject textfieldObject;

    private TextMeshProUGUI textfield;

    public void Start()
    {
        textfield = textfieldObject.GetComponent<TextMeshProUGUI>();
    }

    [Range(1, 300)]
    public int swarmSize;

    [Range(1, 10)]
    public int predatorSize;

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

    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
        textfield.SetText("Max Speed :" + maxSpeed);
    }

    public void SetMaxSteeringForce(float maxSteeringForce)
    {
        this.maxSteeringForce = maxSteeringForce;
        textfield.SetText("Max Steering :" + maxSteeringForce);
    }

    public void setNeighborSearchRadius(float neighborSearchRadius)
    {
        this.neighborSearchRadius = neighborSearchRadius;
        textfield.SetText("Search Radius :" + neighborSearchRadius);
    }

    public void setAvoidWallsScale(float avoidWallsScale)
    {
        this.avoidWallsScale = avoidWallsScale;
        textfield.SetText("Avoid Walls Scale:" + avoidWallsScale);
    }

    public void setAvoidNeighborsScale(float avoidNeighborsScale)
    {
        this.avoidNeighborsScale = avoidNeighborsScale;
        textfield.SetText("Avoid Neighbors Scale:" + avoidNeighborsScale);
    }
    public void setAlignWithNeighborsScale(float alignWithNeighborsScale)
    {
        this.alignWithNeighborsScale = alignWithNeighborsScale;
        textfield.SetText("Align Neighbors Scale:" + alignWithNeighborsScale);
    }

    public void setCohesionScale(float cohesionScale)
    {
        this.useCohesionScale = cohesionScale;
        textfield.SetText("Cohesion Scale:" + string.Format("{0:0.##}", cohesionScale));
    }

    public void setAvoidPredatorsScale(float avoidPredatorsScale)
    {
        this.avoidPredatorsScale = avoidPredatorsScale;
        textfield.SetText("Avoid Predators Scale:" + avoidPredatorsScale);
    }
}
