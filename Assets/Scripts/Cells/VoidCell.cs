using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCell : BuildingCell
{
    [SerializeField] private Material _cleanMaterial;
    [SerializeField] private Material _dirtyMaterial;

    protected override void pollutionChanged()
    {
        if (polluted)
        {
            transform.GetChild(0).GetComponent<Renderer>().material = _dirtyMaterial;
        }
        else
        {
            transform.GetChild(0).GetComponent<Renderer>().material = _cleanMaterial;
        }
    }

    private void OnValidate()
    {
        pollutionChanged();
    }
}
