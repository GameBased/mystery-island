using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace MysteryIsland;

public static class KeyboardHelper
{
    public static KeyboardStateExtended State { get; private set; }

    public static void Update()
    {
        KeyboardExtended.Refresh();
        State = KeyboardExtended.GetState();
    }

    public static bool WasKeyJustPressed(Keys key) => State.WasKeyJustDown(key);
}
