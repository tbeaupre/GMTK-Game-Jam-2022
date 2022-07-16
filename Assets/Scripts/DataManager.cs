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

    public void LoadGame()
    {
        string path = Application.dataPath + "/Maps/" + levelName + ".json";
        if (!File.Exists(path))
        {
            Debug.LogWarning("Data file not found at: " + path);
            LoadDefault();
            return;
        }

        string fileContents = File.ReadAllText(path);
        SerializedGameData mapData = JsonUtility.FromJson<SerializedGameData>(fileContents);
        PlayerData = mapData.playerData;
        Grid = new TriangleGrid(mapData.tiles.ToList());
    }

    private void LoadDefault()
    {
        Grid = new TriangleGrid(3);
        PlayerData = new SerializedPlayerData(new Player());
    }

    public void SaveGame(Player player)
    {
        string jsonData = JsonUtility.ToJson(new SerializedGameData(Grid, player), true);
        File.WriteAllText(Application.persistentDataPath + "/" + levelName + ".json", jsonData);
    }
}
