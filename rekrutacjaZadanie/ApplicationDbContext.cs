using Microsoft.EntityFrameworkCore;
using rekrutacjaZadanie.Models;

namespace rekrutacjaZadanie
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<ToDoTask> ToDoTasks { get; set; }
	}
}
