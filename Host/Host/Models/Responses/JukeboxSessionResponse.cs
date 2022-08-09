namespace Host.Models.Responses;
public class JukeboxSessionResponse
{
    public string OwnerName { get; set; }

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }
}
