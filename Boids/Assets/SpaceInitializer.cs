using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInitializer : MonoBehaviour
{

    public MovementSpace movementSpace;

    void Start()
    {
        transform.position = movementSpace.center;
        transform.localScale = movementSpace.size;
    }


}
