using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborFinder : MonoBehaviour
{
    public List<Transform> neighbors;

    public List<Transform> predators;

    private SwarmConfigurator configurator;

    // check for neighbors only every n frames
    private int skipInterval;

    void Start()
    {
        skipInterval = 10;
        neighbors = new List<Transform>();
        configurator = Settings.current.GetComponent<SwarmConfigurator>();
    }

    void Update()
    {
        if (Time.frameCount % skipInterval == 0)
        {
            neighbors = new List<Transform>();
            predators = new List<Transform>();

            // find Boids
            int boidsLayerMask = 1 << LayerMask.NameToLayer("Boids");
            Collider[] surroundingBoids = Physics.OverlapSphere(transform.position, configurator.neighborSearchRadius, boidsLayerMask);
            foreach (var boid in surroundingBoids)
            {
                if (boid.gameObject != this.gameObject)
                {
                    neighbors.Add(boid.transform);
                }
            }

            // find Predators
            int lowerRangelimit = 10;
            int upperRangelimit = 40;
            // radius depends on neighbor radius, but is limited between [lowerRangelimit, upperRangelimit]
            float predatorAwarenessRadius = Mathf.Min(upperRangelimit, Mathf.Max(lowerRangelimit, 1.5f * configurator.neighborSearchRadius));
            int predatorsLayerMask = 1 << LayerMask.NameToLayer("Predators");
            Collider[] surroundingPredators = Physics.OverlapSphere(transform.position, predatorAwarenessRadius, predatorsLayerMask);
            foreach (var predator in surroundingPredators)
            {
                if (predator.gameObject != this.gameObject)
                {
                    predators.Add(predator.transform);
                }
            }
        }
    }

    public List<Transform> GetNeighbors()
    {
        return neighbors;

    }
    public List<Transform> GetPredators()
    {
        return predators;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(220, 220, 220, 0.5f);
        Gizmos.DrawSphere(transform.position, configurator.neighborSearchRadius);
        foreach (var neighbor in neighbors)
        {
            Gizmos.DrawLine(transform.position, neighbor.position);
        }
    }
}
