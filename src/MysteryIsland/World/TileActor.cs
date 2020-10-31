using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using System;

namespace MysteryIsland.World
{
    public class TileActor : ICollisionActor
    {
        public TileActor(TiledMapTile tile, int tileWIdth, int tileHeight)
        {
            if (tile.IsBlank) throw new InvalidOperationException("Blank tiles cannot be checked for collision");
            Bounds = new RectangleF(
                     x: tile.X * tileWIdth,
                     y: tile.Y * tileHeight,
                 width: tileWIdth,
                height: tileHeight
            ); 
        }

        public IShapeF Bounds { get; set; }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {

        }
    }
}
