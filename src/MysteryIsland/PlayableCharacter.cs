﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Input;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace MysteryIsland
{
    public class PlayableCharacter
    {
        const string ANIMATION_WALK_BACKWARD = "walk-backward";
        const string ANIMATION_WALK_FORWARD  = "walk-forward";
        const string ANIMATION_WALK_RIGHT    = "walk-right";
        const string ANIMATION_WALK_LEFT     = "walk-left";
        const string ANIMATION_DEFAULT       = "look-backward";

        private AnimatedSprite sprite;
        private Vector2 _position = new Vector2(100, 100);
        public Vector2 Position => _position;
        private string animation = ANIMATION_DEFAULT;

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
            var keyboard = KeyboardExtended.GetState();
            animation = animation.Replace("walk", "look");

            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                animation = ANIMATION_WALK_FORWARD;
                _position.Y -= walkSpeed;
            }

            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
            {
                animation = ANIMATION_WALK_BACKWARD;
                _position.Y += walkSpeed;
            }

            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                animation = ANIMATION_WALK_LEFT;
                _position.X -= walkSpeed;
            }

            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                animation = ANIMATION_WALK_RIGHT;
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
