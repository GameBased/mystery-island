using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

// To get the camera
using MonoGame.Extended;
// Then you need a viewport adapter
using MonoGame.Extended.ViewportAdapters;
using System.Collections.Generic;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;

namespace MysteryIsland
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _characterPosition;
        private AnimatedSprite characterSprite;

        // The camera object
        private OrthographicCamera camera;
        private TiledMap map;
        private TiledMapRenderer mapRenderer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            // _graphics.IsFullScreen = true;


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {   
            // Initialize logic goes before the base.Initialize
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 832;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the map
            map = Content.Load<TiledMap>("maps/exp");
            // Create the map renderer
            mapRenderer = new TiledMapRenderer(GraphicsDevice, map);
            // If you decided to use the camere, then you could also initialize it here like this
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 832);
            camera = new OrthographicCamera(viewportadapter);

            var spriteSheet = Content.Load<SpriteSheet>("characters/char1-sprite.sf", new JsonContentLoader());
            characterSprite = new AnimatedSprite(spriteSheet);
            characterSprite.Play("walk-backward");

            _characterPosition = new Vector2(100, 100);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var walkSpeed = deltaSeconds * 128;
            var keyboardState = Keyboard.GetState();
            var animation = "look-backward";

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                animation = "walk-forward";
                _characterPosition.Y -= walkSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                animation = "walk-backward";
                _characterPosition.Y += walkSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                animation = "walk-left";
                _characterPosition.X -= walkSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                animation = "walk-right";
                _characterPosition.X += walkSpeed;
            }

            characterSprite.Play(animation);
            characterSprite.Update(gameTime);
            // Update the map renderer
            mapRenderer.Update(gameTime);
            // If you used the camera then we update it aswell
            camera.LookAt(_characterPosition);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);



            // Transform matrix is only needed if you have a camera
            // Setting the sampler state to `new SamplerState { Filter = TextureFilter.Point }` will reduce gaps and odd artifacts when using animated tiles
            _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });

            // Then we will render the map
            mapRenderer.Draw(camera.GetViewMatrix());

            _spriteBatch.Draw(characterSprite, _characterPosition);

            // End the sprite batch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
