using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using rekrutacjaZadanie;
using rekrutacjaZadanie.Controllers;
using rekrutacjaZadanie.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace ZadanieTests
{
	public class ToDoTaskTests : IClassFixture<WebApplicationFactory<ToDoTaskController>>
	{
		private readonly HttpClient _client;
		private readonly ApplicationDbContext _context;
		private readonly ToDoTaskController _controller;

		public ToDoTaskTests(WebApplicationFactory<ToDoTaskController> factory)
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TasksTestDb").Options;
			_context = new ApplicationDbContext(options);
			_controller = new ToDoTaskController(_context);
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetTaskById_ShouldReturnTask_WhenIdExists()
		{
			var task = new ToDoTask { Title = "Test task" };
			_context.ToDoTasks.Add(task);
			await _context.SaveChangesAsync();

			var response = await _client.GetAsync($"/ToDoTask/{task.Id}");

			response.EnsureSuccessStatusCode();
			var returnedTask = JsonConvert.DeserializeObject<ToDoTask>(await response.Content.ReadAsStringAsync());
			Assert.Equal(task.Id, returnedTask?.Id);
		}

		[Fact]
		public async Task GetTaskById_ShouldReturnNotFound_WhenIdDoesNotExist()
		{
			var response = await _client.GetAsync("/ToDoTask/99999");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task CreateTask_ShouldCreateTask_WhenModelIsValid()				//Test adding new task to database
		{
			var task = new ToDoTask { Title = "Test" };
			var content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");

			var response = await _client.PostAsync("/ToDoTask", content);

			response.EnsureSuccessStatusCode();
			var createdTask = JsonConvert.DeserializeObject<ToDoTask>(await response.Content.ReadAsStringAsync());
			Assert.Equal("Test", createdTask?.Title);
		}

		[Fact]
		public void AddTask_WithInvalidTitle_ShouldFailValidation()		//Test validation for Title propertie in ToDoTask object
		{
			var task = new ToDoTask { Title = "" };

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(task);

			var isValid = Validator.TryValidateObject(task, validationContext, validationResults);

			Assert.False(isValid);
			Assert.Single(validationResults);
			Assert.Equal("Title is required", validationResults[0].ErrorMessage);

		}

		[Fact]
		public void AddTask_WithInvalidDesc_ShouldFailValidation()     //Test validation for Title propertie in ToDoTask object
		{
			var task = new ToDoTask { Title = "Test", Description = "asssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss" };

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(task);

			var isValid = Validator.TryValidateObject(task, validationContext, validationResults, true);

			Assert.False(isValid);
			Assert.Single(validationResults);
			Assert.Equal("Description length can't be more than 500 characters", validationResults[0].ErrorMessage);

		}

		[Fact]
		public void AddTask_WithInvalidPercComplete_ShouldFailValidation()	//Test validation for PercComplete propertie in ToDoTask
		{
			var task = new ToDoTask { Title = "Test", PercComplete=120};

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(task);

			var isValid = Validator.TryValidateObject(task, validationContext, validationResults, true);

			Assert.False(isValid);
			Assert.Single(validationResults);
			Assert.Equal("Complete percentege must be a value between 0 and 100", validationResults[0].ErrorMessage);
		}
	}
}
