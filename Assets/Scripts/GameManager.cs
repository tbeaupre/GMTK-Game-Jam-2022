using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataManager dataManager;
    public TileFactory tileFactory;
    public Player player;
    private List<GameObject> tiles = new List<GameObject>();
    public bool isDebugMode;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        foreach(GameObject tile in tiles)
        {
            Object.Destroy(tile);
        }
        tiles = tileFactory.DrawTiles(dataManager.Grid).ToList();
        player.Init(dataManager.PlayerData);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDebugMode && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider && hit.transform != null)
            {
                GameObject target = hit.transform.gameObject;
                if (target.TryGetComponent<Tile>(out var tile))
                {
                    HandleTileClick(tile);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            dataManager.SaveGame(player);
        }

        if (IsWin)
        {
            Debug.Log("WIN!");
        }

        if (IsPlayerDead()) {
            Init();
        }
    }

    private bool IsWin => player.GetOppositeSide() == dataManager.Grid.GetTileData(player.tile)?.Goal;

    private void HandleTileClick(Tile tile)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            tile.Data.IsDeleted = true;
            return;
        }

        tile.Data.IsDeleted = false;
        if (Input.GetKey(KeyCode.Alpha0)) tile.Data.Goal = 0;
        if (Input.GetKey(KeyCode.Alpha1)) tile.Data.Goal = 1;
        if (Input.GetKey(KeyCode.Alpha2)) tile.Data.Goal = 2;
        if (Input.GetKey(KeyCode.Alpha3)) tile.Data.Goal = 3;
        if (Input.GetKey(KeyCode.Alpha4)) tile.Data.Goal = 4;
        if (Input.GetKey(KeyCode.Alpha5)) tile.Data.Goal = 5;
        if (Input.GetKey(KeyCode.Alpha6)) tile.Data.Goal = 6;
        if (Input.GetKey(KeyCode.Alpha7)) tile.Data.Goal = 7;
        if (Input.GetKey(KeyCode.Alpha8)) tile.Data.Goal = 8;

    }

    bool IsPlayerDead()
    {
        var boardTile = dataManager.Grid.GetTileData(player.tile);
        return boardTile == null || boardTile.IsDeleted;
    }
}
