using System.Text.Json.Serialization;

namespace ForTest.Models
{
	public class ToDoModel
	{
		[JsonPropertyName("id")]
		public int id { get; set; }

		[JsonPropertyName("todo")]
		public string? todoText { get; set; }

		[JsonPropertyName("completed")]
		public bool completed { get; set; }

		[JsonPropertyName("userId")]
		public int userId { get; set; }
	}
}
