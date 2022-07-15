using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleGrid
{
    public List<TileData> Tiles { get; set; } = new List<TileData>();
    public int Radius { get; set; }
    public TriangleGrid(int radius)
    {
        Radius = radius;
        var bounds = Enumerable.Range(-radius, radius * 2).ToList();

        foreach (int a in bounds)
        {
            foreach (int b in bounds)
            {
                foreach (int c in bounds)
                {
                    if (a + b + c == 1 || a + b + c == 2)
                    {
                        var d = new TileData(a, b, c);
                        Debug.Log($"ADD: {a}, {b}, {c}, {d.PointsUp}");
                        Tiles.Add(new TileData(a, b, c));
                    }
                }
            }
        }

    }

    public bool WithinRadius(int a, int b, int c) => Math.Abs(a) > Radius || Math.Abs(b) > Radius || Math.Abs(c) > Radius;
    public void AssertWithinRadius(int a, int b, int c) 
    { 
        if (!WithinRadius(a, b, c))
        {
            throw new Exception($"a, b, or c cannot exceed the radius of the grid: {Radius}");
        }
    }

    public TileData GetTileData(int a, int b, int c)
    {
        AssertWithinRadius(a, b, c);
        return Tiles.First(t => t.A == a && t.B == b && t.C == c);
    }

    public IEnumerable<TileData> GetNeighbors(int a, int b, int c)
    {
        AssertWithinRadius(a, b, c);
        return GetNeighbors(new TileData(a, b, c));
    }

    // Probably want to ToList() this as it does not materialize
    public IEnumerable<TileData> GetNeighbors(TileData tile)
    {
        if (tile.PointsUp)
        {
            if (WithinRadius(tile.A - 1, tile.B, tile.C)) yield return GetTileData(tile.A - 1, tile.B, tile.C);
            if (WithinRadius(tile.A, tile.B - 1, tile.C)) yield return GetTileData(tile.A, tile.B - 1, tile.C);
            if (WithinRadius(tile.A, tile.B, tile.C - 1)) yield return GetTileData(tile.A, tile.B, tile.C - 1);
        }
        else
        {
            if (WithinRadius(tile.A + 1, tile.B, tile.C)) yield return GetTileData(tile.A + 1, tile.B, tile.C);
            if (WithinRadius(tile.A, tile.B + 1, tile.C)) yield return GetTileData(tile.A, tile.B + 1, tile.C);
            if (WithinRadius(tile.A, tile.B, tile.C + 1)) yield return GetTileData(tile.A, tile.B, tile.C + 1);
        }
    }
}

[Serializable]
public struct TileData
{
    public int A;
    public int B;
    public int C;

    public TileData(int a, int b, int c)
    {
        A = a;
        B = b;
        C = c;
    }

    public bool PointsUp => A + B + C == 2;
}