using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KeyValuePair<K, V>
{
    public KeyValuePair(K key, V value)
    {
        Key = key;
        Value = value;
    }

    public K Key { get; set; }
    public V Value { get; set; }
}

[CreateAssetMenu()]
public class PersistentPlacedObjectsData : ScriptableObject
{
    [SerializeField] public List<KeyValuePair<Vector3, PlacementData>> placedObjects;

}

[System.Serializable]
public class GridData
{
    [SerializeField]
    public PersistentPlacedObjectsData persistentPlacedObjectsData;
    Dictionary<Vector3, PlacementData> runTimePlacedObjects = null;
    
    public Dictionary<Vector3, PlacementData> GetRuntimeDictionary()
    {
        if (runTimePlacedObjects == null)
        {
            runTimePlacedObjects = new Dictionary<Vector3, PlacementData>();
            for (int i = 0; i < persistentPlacedObjectsData.placedObjects.Count; i++)
            {
                runTimePlacedObjects.Add(persistentPlacedObjectsData.placedObjects[i].Key, persistentPlacedObjectsData.placedObjects[i].Value);
            }
        }
        return runTimePlacedObjects;
    }
    public void AddObjectAtRuntime(Vector3Int gridPosition,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex,
                            GameObject gameObject)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, gameObject);
        foreach (var pos in positionToOccupy)
        {
            if (GetRuntimeDictionary().ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains cell position {pos}");
                //GetRuntimeDictionary()[pos] = data;
            }
            else
            {
                GetRuntimeDictionary().Add(pos, data);
            }
        }
    }

    public void AddObjectAt(Vector3Int gridPosition,
                        Vector2Int objectSize,
                        int ID,
                        int placedObjectIndex,
                        GameObject gameObject)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, gameObject);
        foreach (var pos in positionToOccupy)
        {
            if (KeyValuePairArrayContainsKey(persistentPlacedObjectsData.placedObjects, pos))
            {
                //int index = GetIndexOf(placedObjects, pos);
                //placedObjects[index].Value = data;
                GameObject go = persistentPlacedObjectsData.placedObjects[GetIndexOf(persistentPlacedObjectsData.placedObjects, pos)].Value.SavedGameObject;
                
                UnityEngine.Object.DestroyImmediate(go);
                
                persistentPlacedObjectsData.placedObjects[GetIndexOf(persistentPlacedObjectsData.placedObjects, pos)].Value.SavedGameObject = gameObject;
                
                //persistentPlacedObjectsData.placedObjects.RemoveAt(GetIndexOf(persistentPlacedObjectsData.placedObjects, pos));

                //persistentPlacedObjectsData.placedObjects.Add(new KeyValuePair<Vector3, PlacementData>(pos, data));

                //throw new Exception($"Dictionary already contains cell position {pos}");
            }
            else
            {
                persistentPlacedObjectsData.placedObjects.Add(new KeyValuePair<Vector3, PlacementData>(pos, data));
            }
        }
    }

    public static int GetIndexOf(List<KeyValuePair<Vector3, PlacementData>> keyValuePairs, Vector3Int key)
    {
        int index = -1;

        for (int i = 0; i < keyValuePairs.Count && index == -1; i++)
        {
            if (keyValuePairs[i].Key == key)
            {
                index = i;
            }
        }

        return index;

    }

    public static KeyValuePair<Vector3Int, PlacementData> GetKeyValuePairArrayWithKey(KeyValuePair<Vector3Int, PlacementData>[] keyValuePairs, Vector3Int key)
    {
        KeyValuePair<Vector3Int, PlacementData>? retVal = null;

        for (int i = 0; i < keyValuePairs.Length && retVal == null; i++)
        {
            if (keyValuePairs[i].Key == key)
            {
                retVal = keyValuePairs[i];
            }
        }

        return (KeyValuePair<Vector3Int, PlacementData>)retVal;
    }

    public static bool KeyValuePairArrayContainsKey(List<KeyValuePair<Vector3, PlacementData>> keyValuePairs, Vector3Int key)
    {
        for (int i = 0; i < keyValuePairs.Count; i++)
        {
            if (keyValuePairs[i].Key == key)
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y)); //cuidao
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (GetRuntimeDictionary().ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class PlacementData
{
    [SerializeField] public List<Vector3Int> occupiedPositions;

    [SerializeField] public int ID { get; private set; }
    [SerializeField] public int PlacedObjectIndex { get; private set; }

    [SerializeField] public GameObject SavedGameObject;

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, GameObject GameObjectToSave)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        SavedGameObject = GameObjectToSave;
    }
}