using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;
    [SerializeField] public GridData gridData;

    private Renderer previewRenderer;

    private void Start()
    {
        StopPlacement();
        AssertGridData();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition;

        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void ClearPlacedObjects()
    {
#if UNITY_EDITOR
        Undo.RegisterCompleteObjectUndo(this, "Clear");
        gridData.persistentPlacedObjectsData.placedObjectCellPositions.Clear();
        gridData.persistentPlacedObjectsData.placedObjectCellDatas.Clear();
#endif
    }

    public void AssertGridData()
    {
#if UNITY_EDITOR

#endif
        if (gridData == null)
        {
            gridData = new GridData();
        }
        Dictionary<Vector3, CellData> runTimePlacedObjects = gridData.GetRuntimeDictionary();
        Debug.Log(runTimePlacedObjects.Count);
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.Onclicked += PlaceBuilding;
        inputManager.OnExit += StopPlacement;

    }

    private void PlaceBuilding()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity)
        {
            return;
        }
        InstantiateCellAtGridPosition(true, gridPosition, database, selectedObjectIndex, grid, gridData, ref gridData.persistentPlacedObjectsData.placedObjectCellPositions, ref gridData.persistentPlacedObjectsData.placedObjectCellDatas);
    }


    public static void InstantiateCellAtGridPosition(Vector3 gridPosition, PlacementSystem placementSystem, int selectedIndex)
    {
#if UNITY_EDITOR
        Undo.RegisterCompleteObjectUndo(placementSystem, "InstantiateNewObject");
#endif
        Vector3Int aGridPosition = placementSystem.grid.WorldToCell(gridPosition);
        InstantiateCellAtGridPosition(false, aGridPosition, placementSystem.database, selectedIndex, placementSystem.grid, placementSystem.gridData, ref placementSystem.gridData.persistentPlacedObjectsData.placedObjectCellPositions, ref placementSystem.gridData.persistentPlacedObjectsData.placedObjectCellDatas);
    }
    public static void InstantiateCellAtGridPosition(bool runtime, Vector3Int gridPosition, ObjectsDatabaseSO database, int selectedObjectIndex, Grid grid, GridData gridData, ref List<Vector3> placedGameObjectPositions, ref List<GameObject> placedGameObjectCellDatas)
    {
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

//        newObject.gameObject.GetComponentInChildren<CheckOverlap>().DestroyOtherIfOverlap();
        if (runtime)
        {
            //placedGameObjects.Add(newObject);

            gridData.AddObjectAtRuntime(gridPosition,
                database.objectsData[selectedObjectIndex].Size,
                database.objectsData[selectedObjectIndex].ID,
                placedGameObjectPositions.Count - 1,
                newObject);
        }
        else
        {
            gridData.AddObjectAt(gridPosition,
                database.objectsData[selectedObjectIndex].Size,
                database.objectsData[selectedObjectIndex].ID,
                placedGameObjectPositions.Count - 1,
                newObject);
        }
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(newObject, "InstantiateNewObject");
#endif

    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.Onclicked -= PlaceBuilding;
        inputManager.OnExit -= StopPlacement;
    }

    public GridData GetGridData()
    {
        return gridData;
    }

    public void SetGridData(GridData d)
    {
        gridData = d;
    }

}
