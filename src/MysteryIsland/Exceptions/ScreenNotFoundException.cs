using MysteryIsland.Screens;

namespace MysteryIsland.Exceptions;

[Serializable]
public class ScreenNotFoundException : Exception
{
    public ScreenNotFoundException() { }
    public ScreenNotFoundException(string message) : base(message) { }
    public ScreenNotFoundException(string message, Exception inner) : base(message, inner) { }
    protected ScreenNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    public ScreenNotFoundException(ScreenName name)
        : this(message: $"An instance of a screen with name {name} was not found.")
    { }
}
