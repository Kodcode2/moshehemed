namespace MVCTest.Models
{
	public class MissionModel
	{
		public enum StatusMission
		{
			Proposal,
			action,
			Actioncompleted,
		}
			public int Id { get; set; }
			public AgentModel Agent { get; set; }
			public TargetModel Target { get; set; }
			public required string RemainingTime { get; set; }
			public string? ActualExecutionTime { get; set; }
			public StatusMission Status { get; set; }
			public int IdAgent { get; set; }
			public int IdTarget { get; set; }

		}
	}