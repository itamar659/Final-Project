using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record PollOption
{
    public PollOption()
    {
        PollId = 0;
        RoomId = NumberGenerator.EmptyId;
        SongName = string.Empty;
        Votes = 0;
    }

    /// <summary>
    /// an id for the database
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// a unique id to every option in a single room
    /// </summary>
    //[Key, Column(Order = 0)]
    public int PollId { get; set; }

    /// <summary>
    /// the room that has this poll option
    /// </summary>
    //[Key, Column(Order = 1)]
    public string RoomId { get; set; }

    /// <summary>
    /// the song name
    /// </summary>
    public string SongName { get; set; }

    /// <summary>
    /// the number of votes this option received
    /// </summary>
    public int Votes { get; set; }
}