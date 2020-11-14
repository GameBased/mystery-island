using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;

namespace MysteryIsland.Screens
{
    public class SelectMapScreen : IScreen
    {
        

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter adapter)
        {
            
        }

        public void Update(GameTime gameTime, IScreenManager screenManager)
        {
            if (KeyboardHelper.WasKeyJustPressed(Keys.Enter))
            {
                screenManager.ChangeScreen(ScreenName.GameScreen);
            }
        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
