using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.Screens;

namespace MysteryIsland
{
    public class MysteryIslandGame : Game
    {
        public static MysteryIslandGame? Instance { get; private set; }

        private readonly GraphicsDeviceManager graphics;

        public SpriteBatch? SpriteBatch { get; private set; }

        ScreenManager screenManager = new ();

        const int WIDTH = 960;
        const int HEIGHT = 540;

        public MysteryIslandGame()
        {
            Instance = this;
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

            using var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, WIDTH, HEIGHT);
            screenManager.LoadContent(Content, GraphicsDevice, SpriteBatch, viewportadapter);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardHelper.Update();
            if (KeyboardHelper.State.IsAltDown() && KeyboardHelper.WasKeyJustPressed(Keys.Enter)) ToggleFullScreen();

            screenManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            screenManager.Draw(gameTime);
        }

        private void ToggleFullScreen()
        {
            var width = graphics.IsFullScreen ? WIDTH : GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var height = graphics.IsFullScreen ? HEIGHT : GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            graphics.ToggleFullScreen();
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    graphics.Dispose();
                }
                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
