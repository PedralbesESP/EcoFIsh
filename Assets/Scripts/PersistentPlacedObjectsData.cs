using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PersistentPlacedObjectsData : ScriptableObject
{
    [SerializeField] public List<KeyValuePair<Vector3, CellData>> placedObjects = new List<KeyValuePair<Vector3, CellData>>();
    public int hola = 1;


    public void modifyHola(int num)
    {
        hola = num;
    }
}