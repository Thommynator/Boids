using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawner : MonoBehaviour
{
    public GameObject boid;

    public MovementSpace movementSpace;

    public int swarmSize = 50;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < swarmSize; i++)
        {
            float x = movementSpace.center.x + Random.Range(-movementSpace.size.x / 2, movementSpace.size.x / 2);
            float y = movementSpace.center.y + Random.Range(-movementSpace.size.y / 2, movementSpace.size.y / 2);
            float z = movementSpace.center.z + Random.Range(-movementSpace.size.z / 2, movementSpace.size.z / 2);

            GameObject newBoid = Instantiate(boid, new Vector3(x, y, z), Quaternion.identity);
            newBoid.transform.SetParent(this.transform);
        }
    }


}
