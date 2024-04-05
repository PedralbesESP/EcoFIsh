using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveLoadGrid
{
    const string FILENAME = "gridLayout";
    public static void SaveGrid(PersistentPlacedObjectsData objectList)
    {

        //string jsonText = JsonUtility.ToJson(objectList);
        string jsonText = JsonConvert.SerializeObject(objectList);
        File.WriteAllText(Path.Combine(Application.dataPath, FILENAME), jsonText);
        Debug.Log("Saved");
    }

    public static PersistentPlacedObjectsData LoadGrid()
    {
        if (File.Exists(Path.Combine(Application.dataPath)))
        {
            string jsonText = File.ReadAllText(Path.Combine(Application.dataPath, FILENAME));
            return JsonUtility.FromJson<PersistentPlacedObjectsData>(jsonText);
        }
        //Resources.
        //File.Create(Path.Combine(Application.dataPath));

        return null;
    }
}
