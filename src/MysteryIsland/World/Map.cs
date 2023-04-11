using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MysteryIsland.Exceptions;
using MysteryIsland.World.Collision;

namespace MysteryIsland.World
{
    public class Map : IDisposable
    {
        private TiledMap? map;
        private TiledMapRenderer? mapRenderer;

        public int WidthInPixels => map?.WidthInPixels ?? 0;
        public int HeightInPixels => map?.HeightInPixels ?? 0;

        private IEnumerable<TiledMapTileLayer> CollisionLayers => map!.TileLayers
            .Where(l => 
                l.Properties.ContainsKey("Type") 
             && l.Properties["Type"] is "Collision");
        public IEnumerable<ICollisionActor> GetCollisionActors()
        {
            if (map is null) throw new MapNotLoadedException();

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

        public void LoadContent(string mapfile, ContentManager content, GraphicsDevice graphicsDevice)
        {
            map = content.Load<TiledMap>(mapfile);
            mapRenderer = new TiledMapRenderer(graphicsDevice, map);
            SetCollisionLayerVisibility(false);
        }

        public void Update(GameTime gameTime)
        {
            if (map is null) throw new MapNotLoadedException();
            if (mapRenderer is null) return;

            mapRenderer.Update(gameTime);
        }

        public void DrawLayersBelowCharacter(Camera camera)
        {
            Draw(camera, beforePlayer: true);
        }

        public void DrawLayersAboveCharacter(Camera camera)
        {
            Draw(camera, beforePlayer: false);
        }

        private void Draw(Camera camera, bool beforePlayer = true)
        {
            const string RENDER_ORDER_KEY = "RenderOrder";
            const string RENDER_ORDER_BEFORE_CHARACTERS = "BeforeCharacters";
            const string RENDER_ORDER_AFTER_CHARACTERS = "AfterCharacters";

            if(map is null) throw new MapNotLoadedException();
            if (mapRenderer is null) return;

            var layersToRender = map.TileLayers.Where(l => shouldRender(l));
            foreach (var layer in layersToRender) mapRenderer.Draw(layer, camera.GetViewMatrix());

            bool shouldRender(TiledMapLayer layer)
            {
                if (layer.IsVisible is false) return false;
                if (!layer.Properties.ContainsKey(RENDER_ORDER_KEY)) return beforePlayer;
                var order = layer.Properties[RENDER_ORDER_KEY];

                if (beforePlayer) return order is RENDER_ORDER_BEFORE_CHARACTERS;
                else return order is RENDER_ORDER_AFTER_CHARACTERS;
            }
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    mapRenderer?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
