namespace MVCTest.Models
{
	public class TargetModel
	{
		public enum StatusTarget
		{
			Live,
			Eliminated,
			monitoredByAnAgent
		}

		public int Id { get; set; }
		public required string Name { get; set; }
		public string Role { get; set; }
		public string Image { get; set; }

		public int Location_x { get; set; } = -1;
		public int Location_y { get; set; } = -1;

		public StatusTarget Status { get; set; } = StatusTarget.Live;


	}
}
