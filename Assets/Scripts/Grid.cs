using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid
{
    public List<TileData> Tiles { get; set; }
    public int Radius { get; set; }
    public Grid(int radius)
    {
        Tiles = new List<TileData>();
        Radius = radius;
        foreach (int a in Enumerable.Range(-radius, radius))
        {
            foreach (int b in Enumerable.Range(-radius, radius))
            {
                foreach (int c in Enumerable.Range(-radius, radius))
                {
                    Tiles.Add(new TileData(a, b, c));
                }
            }
        }
    }

    public bool WithinRadius(int a, int b, int c) => Math.Abs(a) > Radius || Math.Abs(b) > Radius || Math.Abs(c) > Radius;
    public void AssertWithinRadius(int a, int b, int c) { 
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

public class TileData
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }

    public TileData(int a, int b, int c)
    {
        A = a;
        B = b;
        C = c;
    }

    public bool PointsUp => A + B + C == 2;
}