using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class KeyValuePair<K, V>
{
    public KeyValuePair(K key, V value)
    {
        Key = key;
        Value = value;
    }

    public K Key { get; set; }
    public V Value { get; set; }
}



public class GridData : MonoBehaviour
{
    [SerializeField]
    public PersistentPlacedObjectsData persistentPlacedObjectsData;
    public Dictionary<Vector3, CellData> runTimePlacedObjects = null;

    public Dictionary<Vector3, CellData> GetRuntimeDictionary()
    {
        if (runTimePlacedObjects == null)
        {
            runTimePlacedObjects = new Dictionary<Vector3, CellData>();
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

        CellData data = gameObject.GetComponent<CellData>();
        data.InitCellData(positionToOccupy, ID, placedObjectIndex);
        //CellData data = new CellData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (GetRuntimeDictionary().ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains cell position {pos}");
                //GetRuntimeDictionary()[pos] = data;
            }
            else
            {
                data.SetGrid(this);
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

        CellData data = gameObject.GetComponent<CellData>();
        data.InitCellData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (KeyValuePairArrayContainsKey(persistentPlacedObjectsData.placedObjects, pos))
            {
                //int index = GetIndexOf(placedObjects, pos);
                //placedObjects[index].Value = data;
                GameObject go = persistentPlacedObjectsData.placedObjects[GetIndexOf(persistentPlacedObjectsData.placedObjects, pos)].Value.gameObject;



                //persistentPlacedObjectsData.placedObjects[GetIndexOf(persistentPlacedObjectsData.placedObjects, pos)].Value.gameObject = gameObject;

                persistentPlacedObjectsData.placedObjects.RemoveAt(GetIndexOf(persistentPlacedObjectsData.placedObjects, pos));
                persistentPlacedObjectsData.placedObjects.Add(new KeyValuePair<Vector3, CellData>(pos, data));
                data.SetGrid(this);
                //throw new Exception($"Dictionary already contains cell position {pos}");
                UnityEngine.Object.DestroyImmediate(go);
            }
            else
            {
                
                data.SetGrid(this);
                persistentPlacedObjectsData.placedObjects.Add(new KeyValuePair<Vector3, CellData>(pos, data));
            }
        }

        //SaveLoadGrid.SaveGrid(persistentPlacedObjectsData);
    }

    public static int GetIndexOf(List<KeyValuePair<Vector3, CellData>> keyValuePairs, Vector3Int key)
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

    public static KeyValuePair<Vector3Int, CellData> GetKeyValuePairArrayWithKey(KeyValuePair<Vector3Int, CellData>[] keyValuePairs, Vector3Int key)
    {
        KeyValuePair<Vector3Int, CellData> retVal = null; // Aqui habia un ?

        for (int i = 0; i < keyValuePairs.Length && retVal == null; i++)
        {
            if (keyValuePairs[i].Key == key)
            {
                retVal = keyValuePairs[i];
            }
        }

        return (KeyValuePair<Vector3Int, CellData>)retVal;
    }

    public static bool KeyValuePairArrayContainsKey(List<KeyValuePair<Vector3, CellData>> keyValuePairs, Vector3Int key)
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

    public void cleanArea(CellData cell, int cleanArea)
    {
        List<Vector2Int> neighborList = GetNeighborsWithRange(new Vector2Int(cell.occupiedPositions[0].x, cell.occupiedPositions[0].y), cleanArea);

        for (int i = 0; i < neighborList.Count; i++)
        {
            var foundCell = persistentPlacedObjectsData.placedObjects.FirstOrDefault(pObject => neighborList.Contains(new Vector2Int((int)pObject.Key.x, (int)pObject.Key.y)));
            if (foundCell != null)
            {
                if (foundCell.Value is BuildingCell)
                {
                    foundCell.Value.GetComponent<BuildingCell>().Polluted = false;
                }
            }
        }
    }

    public List<Vector2Int> GetNeighborsWithRange(Vector2Int cellPos, int range)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                // Skip the center cell (current cell position)
                if (x == 0 && y == 0)
                    continue;

                // Calculate the neighbor's position
                Vector2Int neighborPos = new Vector2Int(cellPos.x + x, cellPos.y + y);

                // Add the neighbor if it's within the range
                if (Vector2Int.Distance(cellPos, neighborPos) <= range)
                {
                    neighbors.Add(neighborPos);
                }
            }
        }

        return neighbors;
    }

    public void printData()
    {
        foreach (var itemsRunTime in runTimePlacedObjects)
        {
            Debug.Log("Key: " + itemsRunTime.Key.ToString() + " Value: " + itemsRunTime.Value.ToString());
        }
        for (int i = 0; i < persistentPlacedObjectsData.placedObjects.Count; i++)
        {

            Debug.Log("Item pos: " + persistentPlacedObjectsData.placedObjects[i].Key.ToString() + "  Name: " + persistentPlacedObjectsData.placedObjects[i].Value.name + " Data type: " + persistentPlacedObjectsData.placedObjects[i].Value.GetType().ToString() + "" +
                " " + persistentPlacedObjectsData.placedObjects[i].Value.occupiedPositions);

        }
    }
}


