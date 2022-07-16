using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public TileData Data;

    private const float sqrtOfThree = 1.73205080757f;
    private const float squishFactor = 1.2777f;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        UpdateVisuals();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void UpdateVisuals(bool shouldDelete = false)
    {
        Debug.Log($"Updating Visuals: {Data.IsDeleted} {shouldDelete}");
        Data.IsDeleted = shouldDelete;
        spriteRenderer.enabled = !Data.IsDeleted;

        transform.position = Center();
        transform.localEulerAngles = new Vector3(0, 0, Data.PointsUp ? 0 : 180);
    }

    private Vector2 Center()
    {
        return new Vector2
        {
            x = ((0.5f * Data.A) + (-0.5f * Data.C)) * EdgeLength * squishFactor,
            y = (-sqrtOfThree / 6 * Data.A + sqrtOfThree / 3 * Data.B - sqrtOfThree / 6 * Data.C) * EdgeLength 
        };
    }
    private float EdgeLength => transform.localScale.x * 0.24f; // arbitrary spacing factor. feel free to mess w this
}
