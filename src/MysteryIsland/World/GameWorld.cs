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
        public Map Map { get; } = new Map();
        public PlayableCharacter Character { get; private set; }

        private CollisionComponent collisionComponent;
        private GraphicsDevice graphicsDevice;
        private ContentManager content;

        public bool IsReady { get; private set; }

        private SpriteBatch SpriteBatch { get; set; }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            IsReady = false;
            SpriteBatch = spriteBatch;
            Camera = new Camera(viewportAdapter);
            this.graphicsDevice = graphicsDevice;
            this.content = content;
        }

        public void LoadMap(string mapfile)
        {
            // This method should not be blocking
            // so that the gamescreen can show some kind of loading animation

            IsReady = false;
            Character = new PlayableCharacter();
            Map.LoadContent(mapfile, content, graphicsDevice);
            Character.LoadContent(content);
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, Map.WidthInPixels, Map.HeightInPixels));

            foreach (var collidableTile in Map.GetCollisionActors()) collisionComponent.Insert(collidableTile);
            collisionComponent.Insert(Character);
            collisionComponent.Initialize();
            Camera.LookAt(Character.Position);
            IsReady = true;
        }


        public void Update(GameTime gameTime)
        {
            if (!IsReady) return;

            Character.Update(gameTime);

            Map.Update(gameTime);
            Camera.Update(Map, Character);

            collisionComponent.Update(gameTime);
        }

        public void Draw()
        {
            if(!IsReady) return;

            Map.DrawLayersBelowCharacter(Camera);

            SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });
            Character.Draw(SpriteBatch);
            SpriteBatch.End();
            
            Map.DrawLayersAboveCharacter(Camera);
        }
    }
}
