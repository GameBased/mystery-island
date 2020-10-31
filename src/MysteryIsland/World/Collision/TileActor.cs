using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System;

namespace MysteryIsland.World.Collision
{
    public class TileActor : StaticActor
    {
        public TileActor(TiledMapTile tile, int tileWidth, int tileHeight) : base(bounds: 
            new RectangleF(
                     x: tile.X * tileWidth,
                     y: tile.Y * tileHeight,
                 width: tileWidth,
                height: tileHeight
            ))
        {
            if (tile.IsBlank) throw new InvalidOperationException("Blank tiles cannot be checked for collision");
        }
    }
}
