using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnInfo
{
    public SpawnInfo(Vector3 position, int index)
    {
        this.position = position;
        this.index = index;
    }

    public Vector3 position;
    public int index;
}
