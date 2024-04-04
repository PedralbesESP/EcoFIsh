using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
}


public class PlacementData
{
    public List<Vector3> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
}