using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public TriangleGrid Grid;
    public GameObject TilePrefab;
    // Start is called before the first frame update
    public void Start()
    {
        Grid = new TriangleGrid(9);
        foreach (var tileData in Grid.Tiles)
        {
            var obj = Instantiate(TilePrefab);
            obj.GetComponent<Tile>().Data = tileData;
        }
    }

    // Update is called once per frame
    public void Update()
    { 
        
    }
}
