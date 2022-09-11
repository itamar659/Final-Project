namespace Client.Models
{
    public record Room
    {
        public string RoomId { get; set; }
        public string Name { get; set; }
        public int OnlineUsers { get; set; }
        public int TotalUsers { get; set; }
        public string SongName { get; set; }
        public string Picture { get; set; }
        public string StatusComment { get; set; }
        public bool IsOnline { get; set; }
    }
}
