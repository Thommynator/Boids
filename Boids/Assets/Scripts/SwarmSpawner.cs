using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Settings;

public class SwarmSpawner : MonoBehaviour
{
    public GameObject boid;

    void Start()
    {
        for (int i = 0; i < Settings.current.GetComponent<SwarmConfigurator>().swarmSize; i++)
        {
            Vector3 center = Settings.current.centerPosition;

            float x = Random.Range(Settings.current.GetMinPosition(Dimension.X), Settings.current.GetMaxPosition(Dimension.X));
            float y = Random.Range(Settings.current.GetMinPosition(Dimension.Y), Settings.current.GetMaxPosition(Dimension.Y));
            float z = Random.Range(Settings.current.GetMinPosition(Dimension.Z), Settings.current.GetMaxPosition(Dimension.Z));

            GameObject newBoid = Instantiate(boid, new Vector3(x, y, z), boid.transform.rotation);
            newBoid.transform.SetParent(this.transform);

            newBoid.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0.1f, 1), 0, Random.Range(0.1f, 1));
        }
    }


}
