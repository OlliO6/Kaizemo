using System;
using Godot;
using Game.PlayerBehaviour;

public partial class LevelMap : TileMap
{
    public void ProcessCollision(CollisionObject2D body, KinematicCollision2D collision)
    {
        ProcessCollision(body, GetCoordsForBodyRid(collision.GetColliderRid()));
    }

    public void ProcessCollision(CollisionObject2D body, Vector2i coords)
    {
        for (int i = 0; i < GetLayersCount(); i++)
        {
            int sourceId = GetCellSourceId(i, coords);

            if (sourceId == -1)
                continue;

            var source = TileSet.GetSource(sourceId) as TileSetAtlasSource;

            ProcessCollision(
                body,
                source.GetTileData(GetCellAtlasCoords(i, coords), GetCellAlternativeTile(i, coords)));
        }
    }

    public void ProcessCollision(CollisionObject2D body, TileData tile)
    {
        if (body is Player player)
        {
            // if (tile.GetCustomData("KillPlayer").AsBool() == true)
            //     player.Die();'

            return;
        }

        // if ()
    }
}
