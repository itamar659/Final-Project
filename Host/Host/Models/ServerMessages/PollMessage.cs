namespace Host.Models.ServerMessages;
public class PollMessage
{
    public string Token { get; set; }
    public List<PollOption> Options { get; set; }
}
