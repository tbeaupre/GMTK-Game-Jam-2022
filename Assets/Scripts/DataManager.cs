using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int DefaultMapRadius;
    public SerializedPlayerData PlayerData;
    public string levelSetName;
    public int levelValue;

    private List<TileData> initialTiles = null;
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

    public void LoadNext()
    {
        levelValue++;
        try
        {
            LoadGame();
        }
        catch
        {
            LoadFirst();
        }
    }

    public void LoadFirst()
    {
        levelValue = 1;
        LoadGame();
    }

    private void LoadGame()
    {
        Debug.Log($"Loading from {MapPath}...");
        string fileContents = File.ReadAllText(MapPath);
        SerializedGameData mapData = JsonUtility.FromJson<SerializedGameData>(fileContents);
        PlayerData = mapData.playerData;
        initialTiles = mapData.tiles.Where(t => !t.IsDeleted).ToList();
    }

    private void LoadDefault() => PlayerData = new SerializedPlayerData();

    public void SaveGame(TriangleGrid grid, Player player)
    {
        Debug.Log($"Saving to {MapPath}...");
        string jsonData = JsonUtility.ToJson(new SerializedGameData(grid, player), true);
        File.WriteAllText(MapPath, jsonData);
        LoadGame();
    }

    public void SaveGame(TriangleGrid grid)
    {
        Debug.Log($"Saving map data to {MapPath}...");
        string jsonData = JsonUtility.ToJson(new SerializedGameData(grid, PlayerData), true);
        File.WriteAllText(MapPath, jsonData);
        LoadGame();
    }

    public TriangleGrid GetInitialGrid()
    {
        if (initialTiles != null) return new TriangleGrid(initialTiles.Select(t => new TileData(t)).ToList());
        return new TriangleGrid(DefaultMapRadius);
    }

    private string MapPath => $"{Application.dataPath}/Maps/{levelSetName}_{levelValue}.json";

}
