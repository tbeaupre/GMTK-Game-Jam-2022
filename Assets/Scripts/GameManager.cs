using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataManager dataManager;
    public TileFactory tileFactory;
    public bool isDebugMode;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        tileFactory.DrawTiles(dataManager.Grid);
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
                    dataManager.Grid.ToggleTile(tileData.A, tileData.B, tileData.C);
                    tile.UpdateVisuals(!tileData.IsDeleted);
                }
            }
        }
    }
}
