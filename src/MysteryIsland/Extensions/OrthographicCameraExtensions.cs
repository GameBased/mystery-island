using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace MysteryIsland.Extensions
{
    public static class OrthographicCameraExtensions
    {
        // The camera will only pan when the character's viewport position 
        // is PAN_BOUNDARY pixels away from a viewport edge
        private const int PAN_BOUNDARY = 100;

        public static void LookAt(this OrthographicCamera camera, TiledMap map, PlayableCharacter character, Rectangle viewport)
        {
            // the viewport bounds within which the camera shouldn't pan
            Rectangle viewportBounds = new Rectangle(
                     x: viewport.X      + PAN_BOUNDARY,
                     y: viewport.Y      + PAN_BOUNDARY,
                 width: viewport.Width  - PAN_BOUNDARY * 2,
                height: viewport.Height - PAN_BOUNDARY * 2);

            var characterViewportPosition = camera.WorldToScreen(character.Position);
            if (viewportBounds.Contains(characterViewportPosition)) return;

            var panOffset = new Vector2(
                    x: getX(),
                    y: getY()
                );

            var newPos = new Vector2(
                    x: MathHelper.Clamp(camera.Position.X + panOffset.X, 0, map.WidthInPixels - viewport.Width),
                    y: MathHelper.Clamp(camera.Position.Y + panOffset.Y, 0, map.HeightInPixels - viewport.Height)
                );

            camera.Position = newPos;

            

            float getX()
            {
                // move to the right
                if (characterViewportPosition.X > viewportBounds.Right) return characterViewportPosition.X - viewportBounds.Right;

                // move to the left
                else if (characterViewportPosition.X < viewportBounds.Left) return characterViewportPosition.X - viewportBounds.Left;

                // don't move
                else return 0;
            }

            float getY()
            {
                // move towards the bottom
                if (characterViewportPosition.Y > viewportBounds.Bottom) return characterViewportPosition.Y - viewportBounds.Bottom;

                // move towards the top
                else if (characterViewportPosition.Y < viewportBounds.Top) return characterViewportPosition.Y - viewportBounds.Top;

                // don't move
                else return 0;
            }
        }
    }
}
