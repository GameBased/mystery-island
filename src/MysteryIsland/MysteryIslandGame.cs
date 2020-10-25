using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

// To get the camera
using MonoGame.Extended;
// Then you need a viewport adapter
using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.Extensions;
using MonoGame.Extended.Input;

namespace MysteryIsland
{
    public class MysteryIslandGame : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch { get; private set; }

        // The camera object
        private OrthographicCamera camera;
        private TiledMap map;
        private TiledMapRenderer mapRenderer;

        private PlayableCharacter character = new PlayableCharacter();

        public MysteryIslandGame()
        {
            _graphics = new GraphicsDeviceManager(this);

            // _graphics.IsFullScreen = true;


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {   
            // Initialize logic goes before the base.Initialize
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 540;
            _graphics.ApplyChanges();

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
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 832);
            camera = new OrthographicCamera(viewportadapter);

            character.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = KeyboardExtended.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            if (keyboard.IsAltDown() && keyboard.IsKeyDown(Keys.Enter)) _graphics.ToggleFullScreen();

            // TODO: Add your update logic here
            character.Update(gameTime);
            
            // Update the map renderer
            mapRenderer.Update(gameTime);
            camera.LookAt(map, character, GraphicsDevice.Viewport.Bounds);
            
            base.Update(gameTime);
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

            // End the sprite batch
            SpriteBatch.End();            
        }
    }
}
