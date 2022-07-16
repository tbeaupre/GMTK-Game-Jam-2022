using System;
using System.Collections.Generic;
using System.Linq;


public class TriangleGrid
{
    public List<TileData> Tiles { get; set; } = new List<TileData>();
    public TriangleGrid(int radius)
    {
        var bounds = Enumerable.Range(-radius * 2, radius * 4).ToList();

        foreach (int a in bounds)
        foreach (int b in bounds)
        foreach (int c in bounds)
        {
            var abc = new List<int> { a, b, c };
            var isDeleted = abc.Select(v => Math.Abs(v)).Max() > radius;
            var tile = new TileData(a, b, c, isDeleted);
            if (tile.IsValid)
            {
                Tiles.Add(tile);
            }
        }
    }
    public TriangleGrid(List<TileData> tiles)
    {
        Tiles = tiles;
    }

    public TileData GetTileData(TileData tileData) => Tiles.FirstOrDefault(t => t.PositionalMatch(tileData));
    public TileData GetTileData(int a, int b, int c) => Tiles.FirstOrDefault(t => t.PositionalMatch(a, b, c));
    public IEnumerable<TileData> GetNeighbors(int a, int b, int c) => GetNeighbors(new TileData(a, b, c));
    public IEnumerable<TileData> GetRemainingColoredTiles() => Tiles.Where(t => t.IsColored && !t.IsDeleted);

    // Probably want to ToList() this as it does not materialize
    public IEnumerable<TileData> GetNeighbors(TileData tile)
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

}

[Serializable]
public class TileData
{
    public int A;
    public int B;
    public int C;
    public bool IsDeleted;
    public int Goal;

    public TileData(int a, int b, int c, bool isDeleted = false, int goal = 0)
    {
        A = a;
        B = b;
        C = c;
        IsDeleted = isDeleted;
        Goal = goal % 8;
    }

    public TileData(TileData data)
    {
        A = data.A;
        B = data.B;
        C = data.C;
        IsDeleted = data.IsDeleted;
        Goal = data.Goal;
    }

    public bool PositionalMatch(int a, int b, int c) => a == A && b == B && c == C;
    public bool PositionalMatch(TileData tile) => PositionalMatch(tile.A, tile.B, tile.C);

    public bool PointsUp => A + B + C == 2;
    public bool IsValid => A + B + C == 2 || A + B + C == 1;
    public bool IsColored => Goal != 0;
}