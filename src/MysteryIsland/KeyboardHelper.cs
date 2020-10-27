using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace MysteryIsland
{
    public static class KeyboardHelper
    {
        public static KeyboardStateExtended PreviousState { get; private set; }
        public static KeyboardStateExtended State { get; private set; }

        public static void Update()
        {
            PreviousState = State;
            State = KeyboardExtended.GetState();
        }

        public static bool WasKeyJustPressed(Keys key) =>
            PreviousState.IsKeyDown(key) is false && State.IsKeyDown(key) is true;
    }
}
