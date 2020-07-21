using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Settings;

public class SwarmSpawner : MonoBehaviour
{
    public GameObject boid;

    public int swarmSize = 50;

    void Start()
    {
        for (int i = 0; i < swarmSize; i++)
        {
            Vector3 center = Settings.current.centerPosition;

            float x = Random.Range(Settings.current.GetMinPosition(Dimension.X), Settings.current.GetMaxPosition(Dimension.X));
            float y = Random.Range(Settings.current.GetMinPosition(Dimension.Y), Settings.current.GetMaxPosition(Dimension.Y));
            float z = Random.Range(Settings.current.GetMinPosition(Dimension.Z), Settings.current.GetMaxPosition(Dimension.Z));

            GameObject newBoid = Instantiate(boid, new Vector3(x, y, z), boid.transform.rotation);
            newBoid.transform.SetParent(this.transform);
        }
    }


}
