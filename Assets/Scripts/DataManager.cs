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
        string fileContents = File.ReadAllText(path);
        SerializedGameData mapData = JsonUtility.FromJson<SerializedGameData>(fileContents);
        PlayerData = mapData.playerData;
        Grid = new TriangleGrid(mapData.tiles.ToList());
    }

    private void LoadDefault()
    {
        PlayerData = new SerializedPlayerData(new Player());
        Grid = new TriangleGrid(3);
    }

    public void SaveGame(Player player)
    {
        string jsonData = JsonUtility.ToJson(new SerializedGameData(Grid, player), true);
        File.WriteAllText(Application.persistentDataPath + "/" + levelName + ".json", jsonData);
    }
}
