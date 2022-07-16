using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataManager dataManager;
    public TileFactory tileFactory;
    public Player player;
    public bool isDebugMode;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        tileFactory.DrawTiles(dataManager.Grid);
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
                    var tileData = tile.Data;
                    TileData newTileData;
                    if (tileData.Goal == 8 || tileData.IsDeleted)
                        newTileData = dataManager.Grid.ToggleTile(tileData.A, tileData.B, tileData.C);
                    else
                        newTileData = dataManager.Grid.IncrementGoal(tileData.A, tileData.B, tileData.C);

                    tile.UpdateVisuals(newTileData.IsDeleted, newTileData.Goal);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            dataManager.SaveGame(player);
        }

        if (player.GetOppositeSide() == dataManager.Grid.GetTileData(player.a, player.b, player.c)?.Goal)
        {
            Debug.Log("WIN!");
        }
    }
}
