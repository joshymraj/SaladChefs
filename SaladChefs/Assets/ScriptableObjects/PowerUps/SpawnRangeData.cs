using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpawnRangeData : ScriptableObject
{
    [SerializeField]
    float startX;

    [SerializeField]
    public float startZ;

    [SerializeField]
    float endX;

    [SerializeField]
    public float endZ;


    public float StartX
    {
        get
        {
            return startX;
        }
    }

    public float StartZ
    {
        get
        {
            return startZ;
        }
    }

    public float EndX
    {
        get
        {
            return endX;
        }
    }

    public float EndZ
    {
        get
        {
            return endZ;
        }
    }
}
