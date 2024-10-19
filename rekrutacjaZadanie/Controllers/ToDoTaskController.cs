using Microsoft.AspNetCore.Mvc;
using rekrutacjaZadanie.Models;
using Microsoft.EntityFrameworkCore;

namespace rekrutacjaZadanie.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ToDoTaskController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ToDoTaskController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ToDoTask>>> GetAllTasks()
		{
			var items = await _context.ToDoTasks.ToListAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ToDoTask>> GetTaskById(int id)
		{
			var task = await _context.ToDoTasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}
			return Ok(task);
		}

		[HttpGet("incoming")]
		public async Task<ActionResult<IEnumerable<ToDoTask>>> GetIncomingTasks()
		{
			var now = DateTime.Now;

			var incomingTasks = await _context.ToDoTasks.Where(task => task.DateOfExpiry <= now.AddDays(7) && task.DateOfExpiry >= now).ToListAsync();

			if (!incomingTasks.Any())
			{
				return NotFound();
			}
			return Ok(incomingTasks);
		}
	}
}
