namespace Server.Dto;

public class CreatePollDto
{
    public class Option
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public string? Token { get; set; }
    public List<Option>? Options { get; set; }
}
