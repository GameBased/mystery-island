using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics.Geometry;
using MonoGame.Extended.Input;
using System.Diagnostics;

namespace MysteryIsland
{
    public class DebugOverlay // TODO: this should make the collisions layer invisible when disabled
    {
        private bool isVisible = Debugger.IsAttached;
        private RectangleF? playerBounds;

        private void ToggleVisibility()
        {
            isVisible = !isVisible;
        }

        public void Update(PlayableCharacter character, GameTime gameTime)
        {
            var keyboard = KeyboardExtended.GetState();
            if (keyboard.IsKeyDown(Keys.O)) ToggleVisibility(); // also check against the previous state

            if (isVisible is false) return;

            if (character.Bounds is RectangleF rect) playerBounds = rect;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (isVisible is false) return;
            spriteBatch.DrawRectangle(camera.ScreenToWorld(camera.ViewportBounds), Color.White);
            if(playerBounds.HasValue)
            {
                spriteBatch.DrawRectangle(playerBounds.Value, Color.White);
            }
        }

        

    }
}
