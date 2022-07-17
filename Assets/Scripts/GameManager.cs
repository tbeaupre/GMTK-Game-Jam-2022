using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public DataManager dataManager;
    public TileFactory tileFactory;
    public AudioManager AudioMgmt;
    public Player player;
    public bool isDebugMode;

    private GameObject playerObject;
    private List<GameObject> tileRefs = new List<GameObject>();
    private List<GameObject> oldRefs = new List<GameObject>();
    private TriangleGrid activeMap;

    private TileData goalTileToDelete = null;
    private bool isStarted = false;

    public void Play()
    {
        Init();
        isStarted = true;
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
        if (!isStarted)
            return;

        if (!player)
            player = playerObject.GetComponent<Player>();
        CleanUpTiles();
        if (isDebugMode) HandleDebugFunctions();
        if (player != null && player.enabled)
        {
            HandlePlayerFunctions();
            HandlePlayerInputs();
        }
    }

    private void HandlePlayerInputs()
    {
        var direction = getDirectionFromInput();
        if (direction >= 0)
        {
            var moveSuccess = player.TryMove(direction);
            if (moveSuccess)
            {
                AudioMgmt.PlaySFX(GetAppropriateSoundEffectForSuchASpecialOccassion());
            }
        }
    }

    private SFX_TYPE GetAppropriateSoundEffectForSuchASpecialOccassion()
    {
        var activeTile = activeMap.GetTileData(player.tile);
        var isOverGoal = activeTile != null && player.GetOppositeSide() == activeTile.Goal;
        if (isOverGoal) return SFX_TYPE.CLINK;
        if (IsPlayerDead()) return SFX_TYPE.FLOORBOARD;
        return SFX_TYPE.CLUNK;
    }

    private void HandlePlayerFunctions()
    {
        if (IsPlayerDead())
        {
            playerObject.AddComponent<DyingPlayer>();
            player.enabled = false;
            Init();
        }

        if (isDebugMode || activeMap == null) return; // not sure why this would happen but just in case

        if (goalTileToDelete != null && !player.tile.PositionalMatch(goalTileToDelete)) //player has recently left a goal tile
        {
            goalTileToDelete.IsDeleted = true;
            goalTileToDelete = null;
        }

        var activeTile = activeMap.GetTileData(player.tile);
        var isOverGoal = activeTile != null && player.GetOppositeSide() == activeTile.Goal;
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

    private int getDirectionFromInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return 0;
        if (Input.GetKeyDown(KeyCode.RightArrow) && player.PointsDown) return 1;
        if (Input.GetKeyDown(KeyCode.RightArrow) && !player.PointsDown) return 2;
        if (Input.GetKeyDown(KeyCode.DownArrow)) return 3;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !player.PointsDown) return 4;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && player.PointsDown) return 5;
        return -1;
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
