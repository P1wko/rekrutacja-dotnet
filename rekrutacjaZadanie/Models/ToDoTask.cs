namespace rekrutacjaZadanie.Models
{
	public class ToDoTask
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int PercComplete { get; set; } = 0;
		public DateTime DateOfExpiry { get; set; }

	}
}
