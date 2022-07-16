using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public TileData Data;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        UpdateVisuals(Data.IsDeleted);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void UpdateVisuals(bool shouldDelete = false)
    {
        Data.IsDeleted = shouldDelete;
        spriteRenderer.enabled = !Data.IsDeleted;

        transform.position = TileUtils.GetPosition(Data);
        transform.localEulerAngles = new Vector3(0, 0, Data.PointsUp ? 0 : 180);
    }
}

public static class TileUtils
{
    private static float width = 0.65f;
    private static float height = 0.35f;

    public static Vector2 GetPosition(TileData tileData)
    {
        return new Vector2 {
            x = ((0.5f * tileData.A) + (-0.5f * tileData.C)) * width,
            y = ((-0.5f * tileData.A) + tileData.B + (-0.5f * tileData.C)) * height
        };
    }
}