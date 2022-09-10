namespace Client.Models.ServerMessages;

public class ClientMessage
{
    public ClientMessage()
    {
        Token = string.Empty;
        Username = string.Empty;
        RoomId = string.Empty;
        AvatarUrl = string.Empty;
    }

    public string Token { get; set; }

    public string Username { get; set; }

    public string RoomId { get; set; }

    public string AvatarUrl { get; set; }
}
