using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using System.Diagnostics;

namespace MysteryIsland
{
    public class DebugOverlay // TODO: this should make the collisions layer invisible when disabled
    {
        private bool isVisible = Debugger.IsAttached;

        private PlayableCharacter player;

        private void ToggleVisibility()
        {
            isVisible = !isVisible;
        }

        public void Update(PlayableCharacter character, GameTime gameTime)
        {
            var keyboard = KeyboardExtended.GetState();
            if (keyboard.IsKeyDown(Keys.O)) ToggleVisibility(); // also check against the previous state

            if (isVisible is false) return;

            player = character;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (isVisible is false) return;

            // Draw the viewport PAN_BOUNDARY
            spriteBatch.DrawRectangle(camera.ScreenToWorld(camera.ViewportPanBounds), Color.White);

            // Draw the player bounds
            if(player.Bounds is RectangleF playerBounds)
            {
                spriteBatch.DrawRectangle(playerBounds, Color.White);
            }

            // Draw the player position
            spriteBatch.DrawPoint(player.Position, Color.DarkRed, size: 6);
        }
    }
}
