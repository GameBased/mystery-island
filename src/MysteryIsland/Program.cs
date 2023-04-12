using System;

namespace MysteryIsland;

public static class Program
{
    [STAThread]
    static void Main()
    {
        using var game = new MysteryIslandGame();
        game.Run();
    }
}
