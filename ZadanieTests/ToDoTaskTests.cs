using Microsoft.EntityFrameworkCore;
using rekrutacjaZadanie;
using rekrutacjaZadanie.Controllers;
using rekrutacjaZadanie.Models;

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
	}
}
