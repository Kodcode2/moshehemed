using ForTest.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ForTest.Controllers
{
	[Route("[controller]")]
	public class ToDoController : Controller
	{
		private readonly IHttpClientFactory _clientFactory;

		public ToDoController(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			var httpClient = _clientFactory.CreateClient();
			var res = await httpClient.GetAsync("https://dummyjson.com/todos");
			if (res.IsSuccessStatusCode)
			{
				var content = await res.Content.ReadAsStringAsync();
				ToDosModel? todos = JsonSerializer.Deserialize<ToDosModel>(
					content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				if (todos != null)
				{
					return View(todos.Todos);
				}
			}
			return BadRequest();
		}

		[HttpGet("/ToDo/Create")]
		public ActionResult CreatePost()
		{
			return View(new ToDoModel());
		}

		[HttpPost]
		public async Task<ActionResult> Create(ToDoModel newTodo)
		{
			var httpClient = _clientFactory.CreateClient();
			var httpContent = new StringContent(JsonSerializer.Serialize(
				newTodo),
				Encoding.UTF8, "application/json");
			var result = await httpClient.PostAsync("https://dummyjson.com/todos", httpContent);
            var responseContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

            if (result.IsSuccessStatusCode)
			{
				return View("Ok");
			}
			return View("NotFound");
		}

		[HttpGet("/ToDo/Update/{id}")]
		public IActionResult Update(int id)
		{
			return View(new ToDoModel() { id = id });
		}

		[HttpPut]
		public async Task<IActionResult> Update(ToDoModel todo1)
		{
			var httpClient = _clientFactory.CreateClient();
			var httpContent = new StringContent(JsonSerializer.Serialize(todo1), Encoding.UTF8, "application/json");
			var result = await httpClient.PutAsync($"https://jsonplaceholder.typicode.com/posts/{todo1.id}", httpContent);
			if (result.IsSuccessStatusCode)
			{
				return Created("Succeed", todo1);
			}
			return BadRequest();
		}
	}
}
