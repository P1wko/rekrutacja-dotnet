﻿using Microsoft.AspNetCore.Mvc;
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

		[HttpPost]
		public async Task<ActionResult<ToDoTask>> CreateTask(ToDoTask task)
		{
			_context.ToDoTasks.Add(task);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTask(int id, ToDoTask task)
		{
			if (id != task.Id)
			{
				return BadRequest();
			}

			_context.Entry(task).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> SetPercComplete(int id, [FromBody] int perc)
		{
			var task = await _context.ToDoTasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			task.PercComplete = perc;

			await _context.SaveChangesAsync();
			return Ok(task);
		}

		[HttpPatch("{id}/complete")]
		public async Task<IActionResult> SetTaskAsCompleted(int id)
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
		public async Task<IActionResult> DeleteTaskById(int id)
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
