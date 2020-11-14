using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;

namespace MysteryIsland.Screens
{
    public class SelectMapScreen : IScreen
    {
        // names of the map *.tmx files in the /Content/maps dir without extensions
        string[] maps = new string[]
        {
            "exp",
            "ortho"
        };
        int currentSelection = 0;

        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private ViewportAdapter adapter;

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter adapter)
        {
            font = content.Load<SpriteFont>("fonts/Cascadia"); // TODO: Change this to something else
            this.spriteBatch = spriteBatch;
            this.adapter = adapter;
        }

        public void Update(GameTime gameTime, IScreenManager screenManager)
        {
            if (KeyboardHelper.WasKeyJustPressed(Keys.Enter))
            {
                screenManager.ChangeScreen(ScreenName.GameScreen);
            }
            if(KeyboardHelper.WasKeyJustPressed(Keys.Up))
            {
                currentSelection = maps.Length - currentSelection - 1;
            }
            if (KeyboardHelper.WasKeyJustPressed(Keys.Down))
            {
                currentSelection = (currentSelection + 1) % maps.Length;
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, "PICK A MAP", new Vector2(x: 20, y: 20 * 8), Color.WhiteSmoke);
            for(int i = 0; i < maps.Length; i++)
            {
                var color = i == currentSelection ? Color.HotPink : Color.WhiteSmoke;
                spriteBatch.DrawString(
                    font, 
                    maps[i],
                    new Vector2(20, 20 * (i+10)), 
                    color);
            }

            spriteBatch.End();
        }
    }
}
