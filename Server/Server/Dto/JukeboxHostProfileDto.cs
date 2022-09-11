namespace Server.Dto;

public class JukeboxHostProfileDto
{
    public JukeboxHostProfileDto()
    {
        Hostname = string.Empty;
        Summary = string.Empty;
        Description = string.Empty;
        BannerUrl = string.Empty;
        AvatarUrl = string.Empty;
    }

    /// <summary>
    /// friendly unique name for display
    /// </summary>
    public string Hostname { get; set; }

    /// <summary>
    /// host summary
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// host description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// a banner image url
    /// </summary>
    public string BannerUrl { get; set; }

    /// <summary>
    /// an avatar image url
    /// </summary>
    public string AvatarUrl { get; set; }
}
