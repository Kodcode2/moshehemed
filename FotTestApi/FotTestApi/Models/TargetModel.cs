using System.ComponentModel.DataAnnotations;

namespace FotTestApi.Models
{
	public enum StatusTarget
	{
		Live,
		Eliminated,
		monitoredByAnAgent
	}
	public class TargetModel
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public string Role { get; set; }
		public string Image { get; set; }

		public int Location_x { get; set; } = -1;
		public int Location_y { get; set; } = -1;

		public StatusTarget Status { get; set; } = StatusTarget.Live;


	}
}
