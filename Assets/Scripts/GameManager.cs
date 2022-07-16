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
        player = playerObject.GetComponent<SpawnPlayer>().Init(dataManager.PlayerData);
    }

    // Update is called once per frame
    void Update()
    {
        CleanUpTiles();
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

        if (Input.GetKeyDown(KeyCode.S) && isDebugMode)
        {
            dataManager.SaveGame(activeMap, player);
        }

        if (IsWin)
        {
            Debug.Log("WIN!");
        }

        if (IsPlayerDead()) {
            playerObject.AddComponent<DyingPlayer>();
            player.enabled = false;
            Init();
        }
    }

    private void CleanUpTiles()
    {
        foreach (GameObject tile in oldRefs) Destroy(tile);
        oldRefs.Clear();
    }

    private bool IsWin => player.GetOppositeSide() == activeMap.GetTileData(player.tile)?.Goal;

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

    bool IsPlayerDead()
    {
        if (!player.enabled)
            return false;

        var boardTile = activeMap.GetTileData(player.tile);
        return boardTile == null || boardTile.IsDeleted;
    }
}
