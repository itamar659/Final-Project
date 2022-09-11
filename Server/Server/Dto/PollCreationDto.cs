using Server.Models;

namespace Server.Dto;

public class PollCreationDto
{
	public PollCreationDto()
	{
		Token = NumberGenerator.EmptyId;
		Options = new List<PollOption>();
	}

	public string Token { get; set; }

	public List<PollOption> Options { get; set; }
}
