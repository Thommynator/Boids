using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    public static Settings current;
    private void Awake()
    {
        current = this;
    }

    public Vector3 moveableSpace;
    public Vector3 centerPosition;

    public float GetMaxPosition(Dimension dimension)
    {
        switch (dimension)
        {
            case Dimension.X:
                return centerPosition.x + moveableSpace.x / 2.0f;
            case Dimension.Y:
                return centerPosition.y + moveableSpace.y / 2.0f;
            case Dimension.Z:
                return centerPosition.z + moveableSpace.z / 2.0f;
            default:
                throw new IllegalStateException("Dimension not defined");
        }
    }

    public float GetMinPosition(Dimension dimension)
    {
        switch (dimension)
        {
            case Dimension.X:
                return centerPosition.x - moveableSpace.x / 2.0f;
            case Dimension.Y:
                return centerPosition.y - moveableSpace.y / 2.0f;
            case Dimension.Z:
                return centerPosition.z - moveableSpace.z / 2.0f;
            default:
                throw new IllegalStateException("Dimension not defined");
        }
    }

    public enum Dimension
    {
        X, Y, Z
    }


}
