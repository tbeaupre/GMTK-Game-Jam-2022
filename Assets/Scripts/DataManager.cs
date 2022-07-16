using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TriangleGrid Grid;
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        LoadGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGrid();
        }
    }

    public TriangleGrid LoadGrid()
    {
        string path = Application.dataPath + "/Maps/" + levelName + ".json";
        if (!File.Exists(path))
        {
            Debug.LogWarning("Data file not found at: " + path);
            Grid = new TriangleGrid(3);
            return Grid;
        }

        string fileContents = File.ReadAllText(path);
        SerializableTileGrid tileGrid = JsonUtility.FromJson<SerializableTileGrid>(fileContents);
        Grid = new TriangleGrid(tileGrid);
        return Grid;
    }

    void SaveGrid()
    {
        string jsonData = JsonUtility.ToJson(new SerializableTileGrid(Grid), true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + levelName + ".json", jsonData);
    }
}
