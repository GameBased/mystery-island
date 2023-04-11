using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ViewportAdapters;
using System.Diagnostics.CodeAnalysis;

namespace MysteryIsland.World
{
    public class GameWorld : IDisposable
    {
        
        public Camera? Camera { get; private set; }
        public Map Map { get; } = new Map();
        public PlayableCharacter? Character { get; private set; }

        private CollisionComponent? collisionComponent;
        private GraphicsDevice? graphicsDevice;
        private ContentManager? content;

        [MemberNotNullWhen(true, 
            nameof(Camera), 
            nameof(Character), 
            nameof(collisionComponent), 
            nameof(graphicsDevice), 
            nameof(content))]
        public bool IsReady { get; private set; }

        private SpriteBatch? SpriteBatch { get; set; }

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

            if (Camera is null) return;
            if (content is null) return;
            if (graphicsDevice is null) return;

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
            if (Camera is null) return;
            if (Character is null) return;
            if (collisionComponent is null) return;
            if (!IsReady) return;

            Character.Update(gameTime);

            Map.Update(gameTime);
            Camera.Update(Map, Character);

            collisionComponent.Update(gameTime);
        }

        public void Draw()
        {
            if (Camera is null) return;
            if (SpriteBatch is null) return;
            if (Character is null) return;

            if(!IsReady) return;

            Map.DrawLayersBelowCharacter(Camera);

            using var samplerState = new SamplerState { Filter = TextureFilter.Point };
            SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix(), samplerState: samplerState);
            Character.Draw(SpriteBatch);
            SpriteBatch.End();
            
            Map.DrawLayersAboveCharacter(Camera);
        }


        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    collisionComponent?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
