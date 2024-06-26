using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSpawnerGrid : EditorWindow
{
    int selGridInt = -1;
    LayerMask placementLayermask;
    ObjectsDatabaseSO database;
    PlacementSystem placementSystem;
    bool useDataBase = false;

    [MenuItem("Tools/Object Spawner Grid")]
    public static void ShowWindow()
    {
        ObjectSpawnerGrid objectSpawnerGrid = (ObjectSpawnerGrid)GetWindow(typeof(ObjectSpawnerGrid));
        objectSpawnerGrid.placementLayermask = 3;
    }


    private void OnGUI()
    {
        GUILayout.Label("Object Spawner Grid", EditorStyles.boldLabel);
        placementLayermask = EditorGUILayout.LayerField("Layer", placementLayermask);
        database = EditorGUILayout.ObjectField("Database", database, typeof(ObjectsDatabaseSO), true) as ObjectsDatabaseSO;

        if (GUILayout.Button("Empty PlacedObject"))
        {
            placementSystem = FindObjectOfType<PlacementSystem>();
            placementSystem.ClearPlacedObjects();
        }
        if (GUILayout.Button("Use Database"))
        {
            placementSystem = FindObjectOfType<PlacementSystem>();
            useDataBase = true;
            List<CellData> previousObjectsList = new List<CellData>(FindObjectsByType<CellData>(FindObjectsSortMode.None));
            GridData gridData = FindObjectOfType<GridData>();
            if (gridData.persistentPlacedObjectsData.placedObjects.Count<=0)
            {
                foreach (CellData cell in previousObjectsList)
                {
                    KeyValuePair<Vector3, CellData> keyvalue = new KeyValuePair<Vector3, CellData>(cell.occupiedPositions[0], cell);
                    gridData.persistentPlacedObjectsData.placedObjects.Add(keyvalue);
                }
            }
            else
            {
                // gridData.persistentPlacedObjectsData = ScriptableObject.CreateInstance<PersistentPlacedObjectsData>();
            }

        }

        if (useDataBase)
        {
            UseDatabase();
        }

    }

    void OnEnable()
    {
        SceneView.duringSceneGui += SceneGUI;
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }

    void SceneGUI(SceneView sceneView)
    {
        // This will have scene events including mouse down on scenes objects
        Event cur = Event.current;
        if (useDataBase)
            InstantiationManagement();

    }


    private void InstantiationManagement()
    {
        if (placementSystem == null)
        {
            useDataBase = false;
            return;
        }
        if (Event.current.type == EventType.MouseDown)
        {
            if (Event.current.button == 0)
            {
                if (selGridInt != -1)
                {
                    if (placementSystem.GetGridData() == null)
                    {
                        Undo.RegisterCompleteObjectUndo(placementSystem, "GridNew");
                        placementSystem.AssertGridData();
                    }
                    PlacementSystem.InstantiateCellAtGridPosition(GetGridPosFromMouse(placementLayermask), placementSystem, selGridInt);
                }
            }
        }
    }

    private Vector3 GetGridPosFromMouse(LayerMask layerMask)
    {
        Vector3 lastPosition = Vector3.zero;
        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    void UseDatabase()
    {
        GUILayout.BeginVertical("Box");
        string[] selStrings = new string[database.objectsData.Count];
        for (int i = 0; i < database.objectsData.Count; i++)
        {
            selStrings[i] = database.objectsData[i].Name;
        }

        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);

        if (GUILayout.Button("Start"))
        {
            Debug.Log("You choose " + selStrings[selGridInt]);
        }
        GUILayout.EndVertical();
    }


    public void SaveGrid(List<KeyValuePair<Vector3, CellData>> placedObjects)
    {
        string jsonText = JsonUtility.ToJson(placedObjects);
        File.WriteAllText(Path.Combine(Application.dataPath, "gridLayout"), jsonText);
    }
}
