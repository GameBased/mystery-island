using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Text;

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
        public Rectangle Viewport { get; }
        public Rectangle ViewportBounds { get; } // the viewport bounds within which the camera shouldn't pan

        private readonly OrthographicCamera camera;
        public Camera(ViewportAdapter adapter, Rectangle viewport)
        {
            this.camera = new OrthographicCamera(adapter);
            Viewport = viewport;
            ViewportBounds = new Rectangle(
                     x: viewport.X + PAN_BOUNDARY,
                     y: viewport.Y + PAN_BOUNDARY,
                 width: viewport.Width - PAN_BOUNDARY * 2,
                height: viewport.Height - PAN_BOUNDARY * 2)
            ;
        }

        public void Update(TiledMap map, PlayableCharacter character)
        {
            var characterViewportPosition = camera.WorldToScreen(character.Position);
            if (ViewportBounds.Contains(characterViewportPosition)) return;

            var panOffset = new Vector2(
                    x: getX(),
                    y: getY()
                );

            var newPos = new Vector2(
                    x: MathHelper.Clamp(camera.Position.X + panOffset.X, 0, map.WidthInPixels - Viewport.Width),
                    y: MathHelper.Clamp(camera.Position.Y + panOffset.Y, 0, map.HeightInPixels - Viewport.Height)
                );

            camera.Position = newPos;



            float getX()
            {
                // move to the right
                if (characterViewportPosition.X > ViewportBounds.Right) return characterViewportPosition.X - ViewportBounds.Right;

                // move to the left
                else if (characterViewportPosition.X < ViewportBounds.Left) return characterViewportPosition.X - ViewportBounds.Left;

                // don't move
                else return 0;
            }

            float getY()
            {
                // move towards the bottom
                if (characterViewportPosition.Y > ViewportBounds.Bottom) return characterViewportPosition.Y - ViewportBounds.Bottom;

                // move towards the top
                else if (characterViewportPosition.Y < ViewportBounds.Top) return characterViewportPosition.Y - ViewportBounds.Top;

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
