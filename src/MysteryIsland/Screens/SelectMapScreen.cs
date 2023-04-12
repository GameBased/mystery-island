using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace MysteryIsland.Screens;

public class SelectMapScreen : IScreen
{

    MenuItem[] menuItems = new[]
    {
        new MenuItem (Type: MenuItemType.Resume, Name: "Resume"),

        // names of the map *.tmx files in the /Content/maps dir without extensions
        new MenuItem (Type : MenuItemType.NewGame, Name : "exp"),
#if DEBUG
        new MenuItem (Type: MenuItemType.NewGame, Name: "ortho"),
        new MenuItem (Type: MenuItemType.NewGame, Name: "beach"),
#endif

        new MenuItem (Type : MenuItemType.Exit, Name : "Exit")
    };
    int currentSelection = 1;
    bool canResume;
    readonly string[] controls = new[]
    {
        "MENU CONTROLS",
        string.Empty,
        "<UP/DOWN>   - Browse Menu",
        " <ENTER>    - Make a selection",
        string.Empty,
        string.Empty,
        "IN-GAME CONTROLS",
        string.Empty,
        " <UP>       - Move up",
        " <DOWN>     - Move down",
        " <LEFT>     - Move left",
        " <RIGHT>    - Move right",
        "   O        - Toggle debug overlay",
        " <ESC>      - Go back to menu"
    };

    private SpriteFont? font;
    private SpriteBatch? spriteBatch;
    private ViewportAdapter? adapter;

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
            var item = menuItems[currentSelection];

            switch (item.Type)
            {
                case MenuItemType.Resume:
                    screenManager.ChangeScreen(ScreenName.GameScreen);
                    break;
                case MenuItemType.NewGame:
                    canResume = true;
                    screenManager.ChangeToGameScreenAndLoadMap($"maps/{item.Map}");
                    break;
                case MenuItemType.Exit:
                    MysteryIslandGame.Instance?.Exit();
                    break;
                default:
                    break;
            }
        }
        if (KeyboardHelper.WasKeyJustPressed(Keys.Up))
        {
            currentSelection = currentSelection - 1;
            if (currentSelection < 0) currentSelection = menuItems.Length - 1;
            if (currentSelection is 0 && !canResume) currentSelection = menuItems.Length - 1;
        }
        if (KeyboardHelper.WasKeyJustPressed(Keys.Down))
        {
            currentSelection = (currentSelection + 1) % menuItems.Length;
            if (currentSelection is 0 && !canResume) currentSelection++;
        }
    }

    public void Draw(GameTime gameTime)
    {
        if (spriteBatch is null) return;
        spriteBatch.Begin();

        spriteBatch.DrawRectangle(new RectangleF(80, 20 * 6, 300, 20 * (menuItems.Length + 6)), Color.DimGray, thickness: 2.2f);
        spriteBatch.DrawString(font, "MENU", new Vector2(x: 120, y: 20 * 8), Color.WhiteSmoke);
        for (int i = 0; i < menuItems.Length; i++)
        {
            var color = menuItems[i].Type is MenuItemType.Resume && !canResume
                        ? Color.DimGray
                        : i == currentSelection ? Color.HotPink : Color.WhiteSmoke;
            spriteBatch.DrawString(
                font,
                menuItems[i].Map,
                new Vector2(120, 20 * (i + 10)),
                color);
        }

        for (int i = 0; i < controls.Length; i++)
        {
            var color = Color.DimGray;
            spriteBatch.DrawString(font, controls[i], new Vector2(520, 20 * (i + 5)), color);
        }

        spriteBatch.End();
    }

    enum MenuItemType
    {
        Resume,
        NewGame,
        Exit
    }

    sealed record MenuItem(MenuItemType Type, string Name)
    {
        public string Map => Name;
    }
}
