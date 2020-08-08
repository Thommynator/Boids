using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleConfigerer : MonoBehaviour
{
    public GameObject swarm;

    private bool isAvoidingWalls = false;
    private bool isAvoidingOthers = false;

    public void ToggleAvoidWallsRule()
    {
        isAvoidingWalls = !isAvoidingWalls;
        foreach (Transform child in swarm.transform)
        {
            child.GetComponent<Movement>().isAvoidingWalls = isAvoidingWalls;
        }
        Debug.Log(isAvoidingWalls ? "Boids are avoiding the walls." : "Boids don't care about walls.");
    }

    public void ToggleAvoidingOthersRule()
    {
        isAvoidingOthers = !isAvoidingOthers;
        foreach (Transform child in swarm.transform)
        {
            child.GetComponent<Movement>().isAvoidingOthers = isAvoidingOthers;
        }
        Debug.Log(isAvoidingOthers ? "Boids are avoiding the others." : "Boids are not avoiding others.");
    }

}
