using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        FramesPerSecondCounter counter = new FramesPerSecondCounter();

        private void ToggleVisibility()
        {
            isVisible = !isVisible;
        }

        private SpriteFont font;
        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("fonts/Arial");

        }

        public void Update(PlayableCharacter character, GameTime gameTime)
        {
            if (KeyboardHelper.WasKeyJustPressed(Keys.O)) ToggleVisibility();

            if (isVisible is false) return;

            player = character;
            counter.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, GameTime gameTime)
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
            spriteBatch.DrawString(font, $"Pos: ({player.Position.X:F1}, {player.Position.Y:F1})", camera.ScreenToWorld(new Vector2(10, 28)), Color.White);

            counter.Draw(gameTime);
            spriteBatch.DrawString(font, $"FPS: {counter.FramesPerSecond}", camera.ScreenToWorld(new Vector2(10, 10)), Color.White);
        }
    }
}
