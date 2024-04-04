using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSpawnerGrid : EditorWindow
{
    int selGridInt;
    ObjectsDatabaseSO database;
    public string[] selStrings1 = new string[] { "radio1", "radio2", "radio3" };


    [MenuItem("Tools/Object Spawner Grid")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ObjectSpawnerGrid));
    }


    private void OnGUI()
    {
        GUILayout.Label("Object Spawner Grid", EditorStyles.boldLabel);

        database = EditorGUILayout.ObjectField("Database", database, typeof(ObjectsDatabaseSO), true) as ObjectsDatabaseSO;

        if (GUILayout.Button("Use Database"))
        {
            UseDatabase();
        }
        //string[] selStrings = new string[database.objectsData.Count];
        //for(int i = 0; i < database.objectsData.Count; i++)
        {
            //    selStrings[i] = database.objectsData[i].Name;
        }


        //objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn, typeof(GameObject), false) as GameObject;

        
        GUILayout.BeginVertical("Box");
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings1, 1);


        GUILayout.EndVertical();
    }

    void UseDatabase()
    {
        Debug.Log("Llega " + database.objectsData.Count);
        GUILayout.BeginVertical("Box");
        string[] selStrings = new string[database.objectsData.Count];
        for (int i = 0; i < database.objectsData.Count; i++)
        {
            selStrings[i] = database.objectsData[i].Name;
            Debug.Log(selStrings[i]);
        }

        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);

        if (GUILayout.Button("Start"))
        {
            Debug.Log("You choose " + selStrings[selGridInt]);
        }
        GUILayout.EndVertical();
    }
}
