using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuData : ScriptableObject
{
    [SerializeField]
    List<SaladData> salads;

    public List<SaladData> Salads
    {
        get
        {
            return salads;
        }
        set
        {
            salads = value;
        }
    }
}
