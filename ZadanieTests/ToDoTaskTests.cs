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
		public async Task GetTaskById_ShouldReturnNotFound_WhenIdDoesNotExist()		//Test getting task by ID when ID does not exist
		{
			var response = await _client.GetAsync("/ToDoTask/99999");
			Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
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
