using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public TileData Data;

    private const float sqrtOfThree = 1.73205080757f;
    private const float squishFactor = 1.2777f;
    // Start is called before the first frame update
    public void Start()
    {
        transform.position = Center();
        if (!Data.PointsUp)
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }
    }

    // Update is called once per frame
    public void Update()
    {
    }

    private Vector2 Center()
    {
        return new Vector2
        {
            x = ((0.5f * Data.A) + (-0.5f * Data.C)) * EdgeLength * squishFactor,
            y = (-sqrtOfThree / 6 * Data.A + sqrtOfThree / 3 * Data.B - sqrtOfThree / 6 * Data.C) * EdgeLength 
        };
    }

    private float EdgeLength => transform.localScale.x * 0.24f;
}
