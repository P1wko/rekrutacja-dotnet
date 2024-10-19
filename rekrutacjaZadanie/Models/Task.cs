namespace rekrutacjaZadanie.Models
{
	public class Task
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int PercComplete { get; set; }
		public DateTime DateOfExpiry { get; set; }
	}
}
