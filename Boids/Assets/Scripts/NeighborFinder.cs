using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborFinder : MonoBehaviour
{
    public List<Transform> neighbors;

    private SwarmConfigurator configurator;

    void Start()
    {
        neighbors = new List<Transform>();
        configurator = Settings.current.GetComponent<SwarmConfigurator>();
    }

    void Update()
    {
        neighbors = new List<Transform>();
        Collider[] otherBoids = Physics.OverlapSphere(transform.position, configurator.neighborSearchRadius);
        foreach (var boid in otherBoids)
        {
            if (boid.gameObject.layer == LayerMask.NameToLayer("Boids") && boid.gameObject != this.gameObject)
            {
                neighbors.Add(boid.transform);
            }
        }
    }

    public List<Transform> GetNeighbors()
    {
        return neighbors;
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
