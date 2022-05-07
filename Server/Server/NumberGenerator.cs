namespace Server;

internal struct NumberGenerator
{
    internal static string Generate()
    {
        return Guid.NewGuid().ToString();
    }

    internal static string Empty => Guid.Empty.ToString();
}
