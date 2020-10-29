using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Collisions;

using MysteryIsland.Collisions;

using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace MysteryIsland
{
    public class MysteryIslandGame : Game
    {
        private GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch { get; private set; }

        // The camera object
        private Camera camera;
        private TiledMap map;
        private TiledMapRenderer mapRenderer;

        private PlayableCharacter character = new PlayableCharacter();
        private CollisionComponent collisionComponent;
        private DebugOverlay debugOverlay = new DebugOverlay();

        const int WIDTH = 960;
        const int HEIGHT = 540;

        public MysteryIslandGame()
        {
            graphics = new GraphicsDeviceManager(this);
            // _graphics.IsFullScreen = true;


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

            // Load the map
            map = Content.Load<TiledMap>("maps/exp");
            // Create the map renderer
            mapRenderer = new TiledMapRenderer(GraphicsDevice, map);
            // If you decided to use the camere, then you could also initialize it here like this
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, WIDTH, HEIGHT);
            camera = new Camera(viewportadapter);

            character.LoadContent(Content);
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, map.WidthInPixels, map.HeightInPixels));
            var layer = map.GetLayer<TiledMapTileLayer>("collision");
            
            foreach (var collidableTile in layer.Tiles.Where(t => !t.IsBlank).Select(t => new TileActor(t, tileWIdth: map.TileWidth, map.TileHeight))) collisionComponent.Insert(collidableTile);
            collisionComponent.Insert(character);
            collisionComponent.Initialize();

            debugOverlay.LoadContent(Content, map);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardHelper.Update();
            if (KeyboardHelper.WasKeyJustPressed(Keys.Escape)) Exit();
            if (KeyboardHelper.State.IsAltDown() && KeyboardHelper.WasKeyJustPressed(Keys.Enter)) ToggleFullScreen();

            character.Update(gameTime);
            
            // Update the map renderer
            mapRenderer.Update(gameTime);
            camera.Update(map, character);

            collisionComponent.Update(gameTime);
            base.Update(gameTime);
            debugOverlay.Update(character, gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Transform matrix is only needed if you have a camera
            // Setting the sampler state to `new SamplerState { Filter = TextureFilter.Point }` will reduce gaps and odd artifacts when using animated tiles
            SpriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });

            // Then we will render the map
            mapRenderer.Draw(camera.GetViewMatrix());

            character.Draw(SpriteBatch);

            // base.Draw(gameTime);

            debugOverlay.Draw(SpriteBatch, camera, gameTime);

            // End the sprite batch
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
