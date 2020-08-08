using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleConfigerer : MonoBehaviour
{
    public GameObject swarm;

    public void ToggleAvoidWallsRule()
    {
        foreach (Transform child in swarm.transform)
        {
            child.GetComponent<Movement>().isAvoidingWalls = !child.GetComponent<Movement>().isAvoidingWalls;
            Debug.Log("Avoid Walls: " + child.GetComponent<Movement>().isAvoidingWalls);
        }
    }

    public void ToggleSeparateFromOthersRule()
    {
        foreach (Transform child in swarm.transform)
        {
            child.GetComponent<Movement>().isSeparatingFromOthers = !child.GetComponent<Movement>().isSeparatingFromOthers;
            Debug.Log("Avoid Walls: " + child.GetComponent<Movement>().isSeparatingFromOthers);
        }
    }



}
