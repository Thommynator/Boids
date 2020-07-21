using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInitializer : MonoBehaviour
{

    void Start()
    {
        transform.position = Settings.current.centerPosition;
        transform.localScale = Settings.current.moveableSpace;
    }


}
