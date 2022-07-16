using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public TileData Data;
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisuals();
    }

    // Update is called once per frame
    public void Update()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        transform.position = TileUtils.GetPosition(Data);
        transform.localEulerAngles = new Vector3(0, 0, Data.PointsUp ? 0 : 180);

        spriteRenderer.enabled = !Data.IsDeleted;
        spriteRenderer.sprite = sprites[Data.Goal];
    }
}

public static class TileUtils
{
    private static float width = 0.47f;
    private static float height = 0.25f;

    public static Vector2 GetPosition(TileData tileData)
    {
        return new Vector2 {
            x = ((0.5f * tileData.A) + (-0.5f * tileData.C)) * width,
            y = ((-0.5f * tileData.A) + tileData.B + (-0.5f * tileData.C)) * height
        };
    }

    public static void PrintTile(TileData tile, string message = "Debug") => Debug.Log($"{message}: {tile.A}, {tile.B}, {tile.C}");
}