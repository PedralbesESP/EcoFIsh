using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData : MonoBehaviour
{
    [SerializeField] public List<Vector3Int> occupiedPositions;
    [SerializeField] public int ID { get; private set; }
    [SerializeField] public int PlacedObjectIndex { get; private set; }

    [SerializeField] public bool polluted;


    public void InitCellData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = new List<Vector3Int>();
        foreach (Vector3Int pos in occupiedPositions)
        {
            this.occupiedPositions.Add(new Vector3Int(pos.x, pos.y, pos.z));
        }
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }

}



