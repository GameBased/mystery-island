using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System.Diagnostics;

namespace MysteryIsland
{
    public class DebugOverlay
    {
        private bool isVisible = Debugger.IsAttached;

        private World world;
        private FramesPerSecondCounter counter = new FramesPerSecondCounter();

        private void ToggleVisibility()
        {
            isVisible = !isVisible;
            UpdateCollisionLayerVisibility();
        }
        private void UpdateCollisionLayerVisibility()
        {
            world.Map.GetLayer("collision").IsVisible = isVisible;
        }

        private SpriteFont font;
        public void LoadContent(ContentManager content, World world)
        {
            this.world = world;
            font = content.Load<SpriteFont>("fonts/Arial");
            UpdateCollisionLayerVisibility();
        }

        public void Update(GameTime gameTime)
        {
            if (KeyboardHelper.WasKeyJustPressed(Keys.O)) ToggleVisibility();

            if (isVisible is false) return;

            counter.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isVisible is false) return;
            var player = world.Character;
            var camera = world.Camera;

            // Draw the viewport PAN_BOUNDARY
            spriteBatch.DrawRectangle(camera.ScreenToWorld(camera.PanBounds), Color.White);

            // Draw the player bounds
            if(player.Bounds is RectangleF playerBounds)
            {
                spriteBatch.DrawRectangle(playerBounds, Color.White);
            }

            // Draw the player position
            spriteBatch.DrawPoint(player.Position, Color.DarkRed, size: 6);
            spriteBatch.DrawString(font, $"Player world pos: ({player.Position.X:F1}, {player.Position.Y:F1})", camera.ScreenToWorld(new Vector2(10, 30)), Color.White);
            var playerScreenPosition = camera.WorldToScreen(player.Position);
            spriteBatch.DrawString(font, $"Player screen pos: ({playerScreenPosition.X:F1}, {playerScreenPosition.Y:F1})", camera.ScreenToWorld(new Vector2(10, 50)), Color.White);

            counter.Draw(gameTime);
            spriteBatch.DrawString(font, $"FPS: {counter.FramesPerSecond}", camera.ScreenToWorld(new Vector2(10, 10)), Color.White);
        }
    }
}
