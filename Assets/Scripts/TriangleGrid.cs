using System;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class SerializableTileGrid
{
    public TileData[] tiles;

    public SerializableTileGrid(TriangleGrid grid)
    {
        tiles = grid.Tiles.ToArray();
    }
}

public class TriangleGrid
{
    public List<TileData> Tiles { get; set; } = new List<TileData>();
    public TriangleGrid(int radius)
    {
        var bounds = Enumerable.Range(-radius, radius * 2).ToList();

        foreach (int a in bounds)
        {
            foreach (int b in bounds)
            {
                foreach (int c in bounds)
                {
                    if (a + b + c == 1 || a + b + c == 2)
                    {
                        Tiles.Add(new TileData(a, b, c));
                    }
                }
            }
        }
    }
    public TriangleGrid(List<TileData> tiles)
    {
        Tiles = tiles;
    }

    public TriangleGrid(SerializableTileGrid grid)
    {
        Tiles = new List<TileData>(grid.tiles);
    }

    public TileData? GetTileData(int a, int b, int c)
    {
        return Tiles.FirstOrDefault(t => t.A == a && t.B == b && t.C == c);
    }

    public IEnumerable<TileData?> GetNeighbors(int a, int b, int c)
    {
        return GetNeighbors(new TileData(a, b, c));
    }

    // Probably want to ToList() this as it does not materialize
    public IEnumerable<TileData?> GetNeighbors(TileData tile)
    {
        if (tile.PointsUp)
        {
            yield return GetTileData(tile.A - 1, tile.B, tile.C);
            yield return GetTileData(tile.A, tile.B - 1, tile.C);
            yield return GetTileData(tile.A, tile.B, tile.C - 1);
        }
        else
        {
            yield return GetTileData(tile.A + 1, tile.B, tile.C);
            yield return GetTileData(tile.A, tile.B + 1, tile.C);
            yield return GetTileData(tile.A, tile.B, tile.C + 1);
        }
    }

    public void ToggleTile(int a, int b, int c)
    {
        var index = Tiles.FindIndex(t => t.A == a && t.B == b && t.C == c);
        if (index >= 0)
        {
            var t = Tiles.ElementAt(index);
            Tiles.RemoveAt(index);
            Tiles.Add(new TileData(a, b, c, !t.IsDeleted));
        }
    }
}

[System.Serializable]
public struct TileData
{
    public int A;
    public int B;
    public int C;
    public bool IsDeleted;

    public TileData(int a, int b, int c, bool isDeleted = false)
    {
        A = a;
        B = b;
        C = c;
        IsDeleted = isDeleted;
    }

    public bool PointsUp => A + B + C == 2;
}