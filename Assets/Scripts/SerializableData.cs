using System;

[Serializable]
public class SerializedGameData
{
    public TileData[] tiles;
    public SerializedPlayerData playerData;
    public int BestScore;

    public SerializedGameData(TriangleGrid grid, Player player, int score = 0)
    {
        tiles = grid.Tiles.ToArray();
        playerData = new SerializedPlayerData(player);
        BestScore = score;
    }

    public SerializedGameData(TriangleGrid grid, SerializedPlayerData playerData, int score = 0)
    {
        tiles = grid.Tiles.ToArray();
        this.playerData = playerData;
        BestScore = score;
    }
}

[Serializable]
public class SerializedPlayerData
{
    public TileData tile;
    public int side;
    public int rotation;

    public SerializedPlayerData()
    {
        tile = new TileData(1, 1, 0);
        side = 1;
        rotation = 1;
    }

    public SerializedPlayerData(Player player)
    {
        tile = player.tile;
        side = player.side;
        rotation = player.sideRotation;
    }
}
