using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborFinder : MonoBehaviour
{

    public float searchRadius = 5;

    public List<Transform> neighbors;

    void Start()
    {
        neighbors = new List<Transform>();
    }

    void Update()
    {
        neighbors = new List<Transform>();
        Collider[] otherBoids = Physics.OverlapSphere(transform.position, searchRadius);
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
        Gizmos.DrawSphere(transform.position, searchRadius);
        foreach (var neighbor in neighbors)
        {
            Gizmos.DrawLine(transform.position, neighbor.position);
        }
    }
}
