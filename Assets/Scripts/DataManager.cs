using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TriangleGrid Grid;
    public SerializedPlayerData PlayerData;
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            LoadGame();
        }
        catch
        {
            LoadDefault();
        }
    }

    private void LoadGame()
    {
        Debug.Log($"Loading from {MapPath}...");
        string fileContents = File.ReadAllText(MapPath);
        SerializedGameData mapData = JsonUtility.FromJson<SerializedGameData>(fileContents);
        PlayerData = mapData.playerData;
        Grid = new TriangleGrid(mapData.tiles.ToList());
    }

    private void LoadDefault()
    {
        PlayerData = new SerializedPlayerData();
        Grid = new TriangleGrid(3);
    }

    public void SaveGame(Player player)
    {
        Debug.Log($"Saving to {MapPath}...");
        string jsonData = JsonUtility.ToJson(new SerializedGameData(Grid, player), true);
        File.WriteAllText(MapPath, jsonData);
    }

    private string MapPath => Application.dataPath + "/Maps/" + levelName + ".json";



}
