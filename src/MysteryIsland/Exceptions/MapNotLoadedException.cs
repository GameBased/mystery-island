namespace MysteryIsland.Exceptions;

public class MapNotLoadedException : Exception
{
    public MapNotLoadedException(string message) : base(message)
    {
    }

    public MapNotLoadedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public MapNotLoadedException()
    {
    }
}
