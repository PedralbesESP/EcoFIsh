using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(),System.Serializable]
public class PersistentPlacedObjectsData : ScriptableObject
{
    [SerializeField] public List< Vector3> placedObjectCellPositions = new List<Vector3>();
    [SerializeField] public List<GameObject> placedObjectCellDatas = new List<GameObject>();
    public int hola = 1;


    public void DebugValues()
    {
        for (int i = 0; i < placedObjectCellDatas.Count; i++)
        {
            Debug.Log(placedObjectCellDatas[i]);
        }
    }

    public void modifyHola(int num)
    {
        hola = num;
    }
}