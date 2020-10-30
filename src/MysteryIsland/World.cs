using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.Collisions;
using System.Linq;

namespace MysteryIsland
{
    public class World
    {

        public Camera Camera { get; private set; }
        public TiledMap Map { get; private set; }
        public PlayableCharacter Character { get; private set; } = new PlayableCharacter();

        private CollisionComponent collisionComponent;
        private TiledMapRenderer mapRenderer;

        private SpriteBatch SpriteBatch { get; set; }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            SpriteBatch = spriteBatch;

            // Load the map
            Map = content.Load<TiledMap>("maps/exp");
            // Create the map renderer
            mapRenderer = new TiledMapRenderer(graphicsDevice, Map);

            Camera = new Camera(viewportAdapter);

            Character.LoadContent(content);
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, Map.WidthInPixels, Map.HeightInPixels));
            var layer = Map.GetLayer<TiledMapTileLayer>("collision");

            foreach (var collidableTile in layer.Tiles.Where(t => !t.IsBlank).Select(t => new TileActor(t, tileWIdth: Map.TileWidth, Map.TileHeight))) collisionComponent.Insert(collidableTile);
            collisionComponent.Insert(Character);
            collisionComponent.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            Character.Update(gameTime);

            // Update the map renderer
            mapRenderer.Update(gameTime);
            Camera.Update(Map, Character);

            collisionComponent.Update(gameTime);
        }

        public void Draw()
        {
            mapRenderer.Draw(Camera.GetViewMatrix());
            Character.Draw(SpriteBatch);
        }
    }
}
