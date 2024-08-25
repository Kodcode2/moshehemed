namespace FotTestApi.Models
{
	public class KillModel
	{
		public AgentModel Agent { get; set; }
		public TargetModel Trget { get; set; }
		public DateTime Time { get; set; }
		public int AgentId { get; set; }
		public int TargetId { get; set; }

	}
}

