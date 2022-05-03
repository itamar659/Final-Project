#nullable disable

namespace Server;

internal class SessionKeyGenerator
{
    internal static string Generate()
    {
        return Guid.NewGuid().ToString();
    }
}
