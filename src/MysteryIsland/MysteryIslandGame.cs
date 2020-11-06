using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.World;

namespace MysteryIsland
{
    public class MysteryIslandGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch { get; private set; }

        private readonly DebugOverlay debugOverlay = new DebugOverlay();
        private readonly GameWorld world = new GameWorld();

        const int WIDTH = 960;
        const int HEIGHT = 540;

        public MysteryIslandGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialize logic goes before the base.Initialize
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.ApplyChanges();

            Window.Title = "Mystery Island";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, WIDTH, HEIGHT);
            world.LoadContent(Content, GraphicsDevice, SpriteBatch, viewportadapter);

            debugOverlay.LoadContent(Content, world);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardHelper.Update();
            if (KeyboardHelper.WasKeyJustPressed(Keys.Escape)) Exit();
            if (KeyboardHelper.State.IsAltDown() && KeyboardHelper.WasKeyJustPressed(Keys.Enter)) ToggleFullScreen();

            world.Update(gameTime);
            base.Update(gameTime);
            debugOverlay.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if(world.IsActive) world.Draw();

            // TODO: draw this from within the world... or something of that sort
            // to get rid of this extra spritebatch.Begin
            SpriteBatch.Begin(transformMatrix: world.Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });
            debugOverlay.DrawOnMap(SpriteBatch);
            SpriteBatch.End();

            SpriteBatch.Begin();
            debugOverlay.DrawOnScreen(SpriteBatch, gameTime);
            SpriteBatch.End();
        }

        private void ToggleFullScreen()
        {
            var width = graphics.IsFullScreen ? WIDTH : GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var height = graphics.IsFullScreen ? HEIGHT : GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            graphics.ToggleFullScreen();
        }
    }
}
