namespace MysteryIsland.Screens;

public interface IScreenManager
{
    void ChangeScreen(ScreenName screenName);
    void ChangeToGameScreenAndLoadMap(string mapfile);
}
