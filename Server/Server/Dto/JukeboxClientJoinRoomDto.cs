namespace Server.Dto;

public class JukeboxClientJoinRoomDto
{
    public JukeboxClientJoinRoomDto()
    {
        Token = NumberGenerator.EmptyId;
        RoomId = NumberGenerator.EmptyId;
        PinCode = NumberGenerator.EmptyId;
    }

    /// <summary>
    /// a unique token to reference this client
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// A unique room id to join to
    /// </summary>
    public string RoomId { get; set; }

    /// <summary>
    /// a validation code to enter the room
    /// </summary>
    public string PinCode { get; set; }
}
