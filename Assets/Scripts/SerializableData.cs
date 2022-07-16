using System;

[Serializable]
public class SerializedGameData
{
    public TileData[] tiles;
    public SerializedPlayerData playerData;

    public SerializedGameData(TriangleGrid grid, Player player)
    {
        tiles = grid.Tiles.ToArray();
        playerData = new SerializedPlayerData(player);
    }
}

[Serializable]
public class SerializedPlayerData
{
    public TileData tile;
    public int side;
    public int rotation;

    public SerializedPlayerData(Player player)
    {
        tile = player.GetTileData;
        side = player.side;
        rotation = player.sideRotation;
    }
}
