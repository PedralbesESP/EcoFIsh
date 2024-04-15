using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BuildingCell : CellData
{
    public bool Polluted
    {
        get => polluted; set
        {
            pollutionChanged();
            polluted = value;
        }
    }

    [SerializeField] protected bool polluted;
    protected virtual void pollutionChanged() { }

    protected void OnDrawGizmos()
    {
        if (polluted)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(transform.position, 1);
    }
}
