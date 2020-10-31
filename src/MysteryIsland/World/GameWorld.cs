using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ViewportAdapters;

namespace MysteryIsland.World
{
    public class GameWorld
    {

        public Camera Camera { get; private set; }
        public Map Map { get; private set; } = new Map();
        public PlayableCharacter Character { get; private set; } = new PlayableCharacter();

        private CollisionComponent collisionComponent;
        

        private SpriteBatch SpriteBatch { get; set; }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            SpriteBatch = spriteBatch;

            Map.LoadContent(content, graphicsDevice);

            Camera = new Camera(viewportAdapter);

            Character.LoadContent(content);
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, Map.WidthInPixels, Map.HeightInPixels));

            foreach (var collidableTile in Map.CollidableTiles) collisionComponent.Insert(collidableTile);
            collisionComponent.Insert(Character);
            collisionComponent.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            Character.Update(gameTime);

            Map.Update(gameTime);
            Camera.Update(Map, Character);

            collisionComponent.Update(gameTime);
        }

        public void Draw()
        {
            Map.Draw(Camera);
            Character.Draw(SpriteBatch);
        }
    }
}
