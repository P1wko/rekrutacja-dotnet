using Microsoft.EntityFrameworkCore;
using rekrutacjaZadanie;
using rekrutacjaZadanie.Controllers;
using rekrutacjaZadanie.Models;
using System.ComponentModel.DataAnnotations;

namespace ZadanieTests
{
	public class ToDoTaskTests
	{
		private readonly ApplicationDbContext _context;
		private readonly ToDoTaskController _controller;

		public ToDoTaskTests()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TasksTestDb").Options;
			_context = new ApplicationDbContext(options);
			_controller = new ToDoTaskController(_context);
		}

		[Fact]
		public async Task AddTask_ShouldAddTaskToDatabase()
		{
			var task = new ToDoTask { Title = "Test" };

			await _controller.CreateTask(task);
			var savedTask = await _context.ToDoTasks.FindAsync(task.Id);

			Assert.NotNull(savedTask);
			Assert.Equal("Test", savedTask.Title);
		}



		[Fact]
		public void AddTask_WithInvalidTitle_ShouldFailValidation()
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
		public void AddTask_WithInvalidPercComplete_ShouldFailValidation()
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
