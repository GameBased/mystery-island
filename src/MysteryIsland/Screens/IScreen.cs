using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace MysteryIsland.Screens
{
    public interface IScreen
    {
        void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter adapter);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
