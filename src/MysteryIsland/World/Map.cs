using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MysteryIsland.World.Collision;
using System.Collections.Generic;
using System.Linq;

namespace MysteryIsland.World
{
    public class Map
    {
        public TiledMap map;
        private TiledMapRenderer mapRenderer;

        public int WidthInPixels => map?.WidthInPixels ?? 0;
        public int HeightInPixels => map?.HeightInPixels ?? 0;

        private IEnumerable<TiledMapTileLayer> CollisionLayers => map.Layers.Where(l => l.Properties.ContainsKey("Type") && l.Properties["Type"] is "Collision").OfType<TiledMapTileLayer>();
        public IEnumerable<ICollisionActor> GetCollisionActors()
        {
            var tiles = CollisionLayers
                .SelectMany(l => l.Tiles)
                .Where(t => !t.IsBlank)
                .Select(t => new TileActor(t, tileWidth: map.TileWidth, map.TileHeight));
            var mapBounds = getMapBounds();

            return tiles.Concat(mapBounds);

            IEnumerable<StaticActor> getMapBounds()
            {
                yield return new StaticActor(new RectangleF(0,                                         0, map.WidthInPixels,                .1f));
                yield return new StaticActor(new RectangleF(0,                                         0,               .1f,  map.HeightInPixels));
                yield return new StaticActor(new RectangleF(map.WidthInPixels - 1,                     0,               .1f,  map.HeightInPixels));
                yield return new StaticActor(new RectangleF(0,                     map.HeightInPixels - 1, map.WidthInPixels,                1f));

            }
        }

        public void SetCollisionLayerVisibility(bool visibility)
        {
            foreach(var layer in CollisionLayers)
            {
                layer.IsVisible = visibility;
            }
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            map = content.Load<TiledMap>("maps/exp");
            mapRenderer = new TiledMapRenderer(graphicsDevice, map);
        }

        public void Update(GameTime gameTime)
        {
            mapRenderer.Update(gameTime);
        }

        public void Draw(Camera camera)
        {
            mapRenderer.Draw(camera.GetViewMatrix());
        }
    }
}
