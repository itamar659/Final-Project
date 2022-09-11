namespace Server.Dto;

public class PollVoteDto
{
    public PollVoteDto()
    {
        Token = NumberGenerator.EmptyId;
        PollId = -1;
    }

    /// <summary>
    /// a unique token to reference this client
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// a unique id to every option in a single room
    /// </summary>
    public int PollId { get; set; }
}
