namespace MVCTest.Models
{
	public class AgentModel
	{
		
		public enum StatusAgent
		{
			Active,
			Inactive,

		}
		
			public int Id { get; set; }
			public required string Nickname { get; set; }

			public int Location_x { get; set; }

			public int Location_y { get; set; }
			public string? Image { get; set; }
			public List<MissionModel> Missions { get; set; } = [];
			public StatusAgent Status { get; set; } = StatusAgent.Inactive;
		}
	}



