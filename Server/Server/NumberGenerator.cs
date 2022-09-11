namespace Server;

internal struct NumberGenerator
{
    private const int PinCodeLength = 5;
    private const string PinCodeCharacters = "0123456789";
    private static readonly Random _rand = Random.Shared;

    internal static string GenerateId()
    {
        return Guid.NewGuid().ToString();
    }

    internal static string GeneratePinCode()
    {
        return new string(Enumerable.Repeat(PinCodeCharacters, PinCodeLength)
            .Select(s => s[_rand.Next(s.Length)])
            .ToArray());
    }

    internal static string EmptyPinCode => new string('0', PinCodeLength);

    internal static string EmptyId => Guid.Empty.ToString();
}
