using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace MysteryIsland
{
    public class PlayableCharacter
    {
        private AnimatedSprite sprite;
        private Vector2 _position = new Vector2(100, 100);
        public Vector2 Position => _position;



        public void LoadContent(ContentManager content)
        {
            var spriteSheet = content.Load<SpriteSheet>("characters/char1-sprite.sf", new JsonContentLoader());
            sprite = new AnimatedSprite(spriteSheet);
            sprite.Play("walk-backward");
        }

        public void Update(GameTime gameTime)
        {
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var walkSpeed = deltaSeconds * 128;
            var keyboardState = Keyboard.GetState();
            var animation = "look-backward";

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                animation = "walk-forward";
                _position.Y -= walkSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                animation = "walk-backward";
                _position.Y += walkSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                animation = "walk-left";
                _position.X -= walkSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                animation = "walk-right";
                _position.X += walkSpeed;
            }

            sprite.Play(animation);
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position);
        }
    }
}
