using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using MysteryIsland.World;

namespace MysteryIsland.Screens;

public class GameScreen : IScreen, IDisposable
{
    private readonly DebugOverlay debugOverlay = new();
    private readonly GameWorld world = new();

    private SpriteBatch? SpriteBatch { get; set; }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ViewportAdapter adapter)
    {
        world.LoadContent(content, graphicsDevice, spriteBatch, adapter);
        debugOverlay.LoadContent(content, world);

        SpriteBatch = spriteBatch;
    }

    public void LoadMap(string mapfile)
    {
        world.LoadMap(mapfile);
    }

    public void Update(GameTime gameTime, IScreenManager screenManager)
    {
        if (KeyboardHelper.WasKeyJustPressed(Keys.Escape))
        {
            screenManager.ChangeScreen(ScreenName.SelectMapScreen);
            return;
        }

        world.Update(gameTime);
        debugOverlay.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        if (SpriteBatch is null) return;

        if (world.IsReady)
        {
            world.Draw((sb) =>
            {
                debugOverlay.DrawOnMap(SpriteBatch);
            });
        }

        SpriteBatch.Begin();
        debugOverlay.DrawOnScreen(SpriteBatch, gameTime);
        SpriteBatch.End();
    }

    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                world.Dispose();
            }

            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
