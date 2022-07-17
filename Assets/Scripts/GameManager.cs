using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public DataManager dataManager;
    public TileFactory tileFactory;
    public Player player;
    public bool isDebugMode;

    private GameObject playerObject;
    private List<GameObject> tileRefs = new List<GameObject>();
    private List<GameObject> oldRefs = new List<GameObject>();
    private TriangleGrid activeMap;

    private TileData goalTileToDelete = null;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        activeMap = dataManager.GetInitialGrid();
        oldRefs = tileRefs;
        tileRefs = tileFactory.DrawTiles(activeMap).ToList();
        playerObject = Instantiate(playerPrefab);
        SpawnPlayer spawnPlayer = playerObject.GetComponent<SpawnPlayer>();
        player = spawnPlayer.Init(dataManager.PlayerData);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = playerObject.GetComponent<Player>();
        CleanUpTiles();
        if (isDebugMode) HandleDebugFunctions();
        HandlePlayerFunctions();
    }

    private void HandlePlayerFunctions()
    {
        if (IsPlayerDead())
        {
            playerObject.AddComponent<DyingPlayer>();
            player.enabled = false;
            Init();
        }

        var activeTile = activeMap.GetTileData(player.tile);
        if (activeTile == null || isDebugMode) return; // not sure why this would happen but just in case

        if (goalTileToDelete != null && !activeTile.PositionalMatch(goalTileToDelete)) //player has recently left a goal tile
        {
            goalTileToDelete.IsDeleted = true;
            goalTileToDelete = null;
        }

        var isOverGoal = player.GetOppositeSide() == activeTile.Goal;
        if (isOverGoal) goalTileToDelete = activeTile;
        if (activeMap.GetRemainingColoredTiles().Count() == 1 && isOverGoal)
        {
            dataManager.LoadNext();
            Destroy(playerObject);
            Init();
        }
    }

    private void CleanUpTiles()
    {
        foreach (GameObject tile in oldRefs) Destroy(tile);
        oldRefs.Clear();
    }


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
        if (Input.GetKey(KeyCode.LeftShift)) tile.Data.Goal = player.GetOppositeSide();
    }

    private bool IsPlayerDead()
    {
        if (player == null || !player.enabled) return false;
        var boardTile = activeMap.GetTileData(player.tile);
        return boardTile == null || boardTile.IsDeleted;
    }

    private void HandleDebugFunctions()
    {
        if (Input.GetMouseButton(0))
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
            dataManager.SaveGame(activeMap, player);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            dataManager.SaveGame(activeMap);
        }
    }
}
