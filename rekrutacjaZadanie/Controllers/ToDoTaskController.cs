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
		public async Task<ActionResult<IEnumerable<ToDoTask>>> GetAllTasks()		//Get all tasks
		{
			var items = await _context.ToDoTasks.ToListAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ToDoTask>> GetTaskById(int id)				//Get one task by Id
		{
			var task = await _context.ToDoTasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}
			return Ok(task);
		}

		[HttpGet("incoming")]
		public async Task<ActionResult<IEnumerable<ToDoTask>>> GetIncomingTasks()	//Get tasks with date closer than one week
		{
			var now = DateTime.Now;

			var incomingTasks = await _context.ToDoTasks.Where(task => task.DateOfExpiry <= now.AddDays(7) && task.DateOfExpiry >= now).ToListAsync();

			if (!incomingTasks.Any())
			{
				return NotFound();
			}
			return Ok(incomingTasks);
		}

		[HttpPost]
		public async Task<ActionResult<ToDoTask>> CreateTask([FromBody]ToDoTask task)	//Create new task
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			_context.ToDoTasks.Add(task);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTask(int id, ToDoTask task)				//Update task at the specific ID
		{
			if (id != task.Id)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Entry(task).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> SetPercComplete(int id, [FromBody] int perc)	//Set new % complete for task at specific ID
		{
			var task = await _context.ToDoTasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			task.PercComplete = perc;

			await _context.SaveChangesAsync();
			return Ok(task);
		}

		[HttpPatch("{id}/complete")]
		public async Task<IActionResult> SetTaskAsCompleted(int id)			//Mark task with specific ID as completed by setting % complete on 100
		{
			var task = await _context.ToDoTasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			task.PercComplete = 100;

			await _context.SaveChangesAsync();
			return Ok(task);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTaskById(int id)				//Delete task at specific ID
		{
			var task = await _context.ToDoTasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			_context.ToDoTasks.Remove(task);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
