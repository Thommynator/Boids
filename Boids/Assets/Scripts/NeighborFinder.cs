using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborFinder : MonoBehaviour
{
    public List<Transform> neighbors;

    public List<Transform> predators;

    private SwarmConfigurator configurator;

    void Start()
    {
        neighbors = new List<Transform>();
        configurator = Settings.current.GetComponent<SwarmConfigurator>();
    }

    void Update()
    {
        neighbors = new List<Transform>();
        predators = new List<Transform>();

        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, configurator.neighborSearchRadius);
        foreach (var other in surroundingObjects)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Boids") && other.gameObject != this.gameObject)
            {
                neighbors.Add(other.transform);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Predators") && other.gameObject != this.gameObject)
            {
                predators.Add(other.transform);
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
