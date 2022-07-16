using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject TilePrefab;

    public IEnumerable<GameObject> DrawTiles(TriangleGrid grid)
    {
        foreach (var tileData in grid.Tiles)
        {
            var obj = Instantiate(TilePrefab);
            obj.GetComponent<Tile>().Data = tileData;
            yield return obj;
        }
    }
}
