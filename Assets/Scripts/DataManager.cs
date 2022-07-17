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
    public int BestScore;

    private SerializedGameData gameData;
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
        TextAsset jsonObj = Resources.Load<TextAsset>($"Maps/{levelSetName}_{levelValue}");
        gameData = JsonUtility.FromJson<SerializedGameData>(jsonObj.text);
        PlayerData = gameData.playerData;
        initialTiles = gameData.tiles.Where(t => !t.IsDeleted).ToList();
        BestScore = gameData.BestScore;
    }

    private void LoadDefault() => PlayerData = new SerializedPlayerData();

    public void SaveScore(int score)
    {
        if (score < gameData.BestScore)
        {
            Debug.Log($"Saving to {MapPath}...");
            gameData.BestScore = score;
            string jsonData = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(MapPath, jsonData);
        }
    }

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

    private string MapPath => $"{Application.dataPath}/Resources/Maps/{levelSetName}_{levelValue}.json";

}
