using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.Exceptions;
using System.Collections.Generic;

namespace MysteryIsland.Screens
{
    public enum ScreenName
    {
        GameScreen,
        SelectMapScreen
    }

    public class ScreenManager : IScreenManager
    {
        private Dictionary<ScreenName, IScreen> screens = new Dictionary<ScreenName, IScreen>
        {
            { ScreenName.SelectMapScreen, new SelectMapScreen() },
            { ScreenName.GameScreen, new GameScreen() }
        };

        private IScreen currentScreen;

        public ScreenManager()
        {
            currentScreen = screens[ScreenName.SelectMapScreen];
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter adapter)
        {
            foreach(var screen in screens.Values)
            {
                screen.LoadContent(content, graphicsDevice, spriteBatch, adapter);
            }
        }

        public void ChangeScreen(ScreenName screenName)
        {
            currentScreen = screens[screenName] ?? throw new ScreenNotFoundException(screenName);
        }

        public void ChangeToGameScreenAndLoadMap(string mapfile)
        {
            currentScreen = screens[ScreenName.GameScreen] ?? throw new ScreenNotFoundException(ScreenName.GameScreen);
            (currentScreen as GameScreen)?.LoadMap(mapfile);
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime, this);
        }

        public void Draw(GameTime gameTime)
        {
            currentScreen.Draw(gameTime);
        }
    }
}
