using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;

namespace MysteryIsland
{
    public class Camera
    {
        /// <summary>
        /// The camera will only pan when the character's viewport position 
        /// is PAN_BOUNDARY pixels away from a viewport edge
        /// </summary>
        const int PAN_BOUNDARY = 100;

        public Vector2 Position => camera.Position;
        public Rectangle VirtualBounds { get; private set; }
        public Rectangle PanBounds { get; private set; } // the viewport bounds within which the camera shouldn't pan

        private readonly OrthographicCamera camera;
        private readonly ViewportAdapter adapter;
        public Camera(ViewportAdapter adapter)
        {
            this.camera = new OrthographicCamera(adapter);
            this.adapter = adapter;

            VirtualBounds = new Rectangle(0, 0, adapter.VirtualWidth, adapter.VirtualHeight);
            PanBounds = new RectangleF(
                     x: VirtualBounds.X + PAN_BOUNDARY,
                     y: VirtualBounds.Y + PAN_BOUNDARY,
                 width: VirtualBounds.Width - PAN_BOUNDARY * 2,
                height: VirtualBounds.Height - PAN_BOUNDARY * 2).ToRectangle();
        }

        public void Update(TiledMap map, PlayableCharacter character)
        {
            // translate the panbounds and virtual bounds to the camera's current position
            var panbounds = new Rectangle(camera.Position.ToPoint() + PanBounds.Location, PanBounds.Size);
            var virtualbounds = new Rectangle(camera.Position.ToPoint() + VirtualBounds.Location, VirtualBounds.Size);

            var characterViewportPosition = character.Position;
            if (panbounds.Contains(characterViewportPosition)) return;

            var panOffset = new Vector2(
                    x: getX(),
                    y: getY()
                );

            var newPos = new Vector2(
                    x: MathHelper.Clamp(camera.Position.X + panOffset.X, 0, map.WidthInPixels - virtualbounds.Width),
                    y: MathHelper.Clamp(camera.Position.Y + panOffset.Y, 0, map.HeightInPixels - virtualbounds.Height)
                );

            camera.Position = newPos;



            float getX()
            {
                // move to the right
                if (characterViewportPosition.X > panbounds.Right) return characterViewportPosition.X - panbounds.Right;

                // move to the left
                else if (characterViewportPosition.X < panbounds.Left) return characterViewportPosition.X - panbounds.Left;

                // don't move
                else return 0;
            }

            float getY()
            {
                // move towards the bottom
                if (characterViewportPosition.Y > panbounds.Bottom) return characterViewportPosition.Y - panbounds.Bottom;

                // move towards the top
                else if (characterViewportPosition.Y < panbounds.Top) return characterViewportPosition.Y - panbounds.Top;

                // don't move
                else return 0;
            }
        }

        public Matrix GetViewMatrix() => camera.GetViewMatrix();
        public Vector2 ScreenToWorld(Vector2 screenPosition) => camera.ScreenToWorld(screenPosition);
        public RectangleF ScreenToWorld(Rectangle screenPosition) => new RectangleF(camera.Position + screenPosition.GetCorners()[0].ToVector2(), screenPosition.Size);
        public Vector2 WorldToScreen(Vector2 screenPosition) => camera.WorldToScreen(screenPosition);
    }
}
