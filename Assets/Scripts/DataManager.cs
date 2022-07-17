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

    private int[] highscores;
    private SerializedGameData gameData;
    private List<TileData> initialTiles = null;
    // Start is called before the first frame update

    private void Awake()
    {
        try
        {
            LoadScores();
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
        TextAsset jsonObj = Resources.Load<TextAsset>($"Maps/{MapName}");
        gameData = JsonUtility.FromJson<SerializedGameData>(jsonObj.text);
        PlayerData = gameData.playerData;
        initialTiles = gameData.tiles.Where(t => !t.IsDeleted).ToList();
    }

    private void LoadDefault() => PlayerData = new SerializedPlayerData();

    public int BestScore()
    {
        if (highscores[levelValue] > 0) return highscores[levelValue];
        return -1;
    }


    public void SaveScore(int score)
    {
        if (highscores[levelValue] > 0 && score >= BestScore()) return;
        highscores[levelValue] = score;
        string jsonData = JsonUtility.ToJson(new HighScoreData(highscores), true);
        Debug.Log(jsonData);
        File.WriteAllText(ScorePath, jsonData);
    }

    public void LoadScores()
    {
        try
        {
            if (File.Exists(ScorePath))
            {
                Debug.Log(ScorePath);
                string jsonString = File.ReadAllText(ScorePath);
                highscores = JsonUtility.FromJson<HighScoreData>(jsonString).Highscores;
                if (highscores.Length == 0) highscores = new int[20];
                return;
            }
        }
        catch { }
        highscores = new int[20];
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

    private string MapPath => $"{Application.dataPath}/Resources/Maps/{MapName}.json";
    private string ScorePath => $"{Application.persistentDataPath}/highscores.json";
    private string MapName => $"{levelSetName}_{levelValue}";

}
