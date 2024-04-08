using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PosidoniaBuilding : BuildingCell
{
    [SerializeField] private int cleanArea = 3;

    // Start is called before the first frame update
    void Start()
    {
        polluted = true;
        OnBuild();
    }
    protected  void OnBuild()
    {
        gridData.cleanArea(this, cleanArea);
    }

    private void OnDrawGizmos()
    {
        if (polluted)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
