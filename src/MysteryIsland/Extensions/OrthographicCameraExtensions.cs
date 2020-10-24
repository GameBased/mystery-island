using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace MysteryIsland.Extensions
{
    public static class OrthographicCameraExtensions
    {
        public static void LookAt(this OrthographicCamera camera, TiledMap map, PlayableCharacter character, Rectangle viewport)
        {
            var position = new Vector2(
                    x: MathHelper.Clamp(character.Position.X, viewport.Width / 2, map.WidthInPixels - viewport.Width / 2),
                    y: MathHelper.Clamp(character.Position.Y, viewport.Height / 2, map.HeightInPixels - viewport.Height / 2)
                );
            camera.LookAt(position);
        }
    }
}
