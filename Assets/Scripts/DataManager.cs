using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int DefaultMapRadius;
    public SerializedPlayerData PlayerData;
    public string levelName;

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

    private void LoadGame()
    {
        Debug.Log($"Loading from {MapPath}...");
        string fileContents = File.ReadAllText(MapPath);
        SerializedGameData mapData = JsonUtility.FromJson<SerializedGameData>(fileContents);
        PlayerData = mapData.playerData;
        initialTiles = mapData.tiles.ToList();
    }

    private void LoadDefault() => PlayerData = new SerializedPlayerData();

    public void SaveGame(TriangleGrid grid, Player player)
    {
        Debug.Log($"Saving to {MapPath}...");
        string jsonData = JsonUtility.ToJson(new SerializedGameData(grid, player), true);
        File.WriteAllText(MapPath, jsonData);
        initialTiles = grid.Tiles;
    }

    public TriangleGrid GetInitialGrid()
    {
        if (initialTiles != null) return new TriangleGrid(initialTiles.Select(t => new TileData(t)).ToList());
        return new TriangleGrid(DefaultMapRadius);
    }

    private string MapPath => Application.dataPath + "/Maps/" + levelName + ".json";

}
