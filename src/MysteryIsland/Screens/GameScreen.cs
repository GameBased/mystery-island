using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.World;

namespace MysteryIsland.Screens
{
    public class GameScreen : IScreen
    {
        private readonly DebugOverlay debugOverlay = new DebugOverlay();
        private readonly GameWorld world = new GameWorld();

        private SpriteBatch SpriteBatch { get; set; }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter adapter)
        {
            world.LoadContent(content, graphicsDevice, spriteBatch, adapter);
            debugOverlay.LoadContent(content, world);

            SpriteBatch = spriteBatch;
        }

        public void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            debugOverlay.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (world.IsActive) world.Draw();

            // TODO: draw this from within the world... or something of that sort
            // to get rid of this extra spritebatch.Begin
            SpriteBatch.Begin(transformMatrix: world.Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });
            debugOverlay.DrawOnMap(SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin();
            debugOverlay.DrawOnScreen(SpriteBatch, gameTime);
            SpriteBatch.End();
        }
    }
}
