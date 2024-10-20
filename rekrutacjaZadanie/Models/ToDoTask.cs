using System.ComponentModel.DataAnnotations;

namespace rekrutacjaZadanie.Models
{
	public class ToDoTask				//basic ToDoTask object with validation
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Title is required")]
		[StringLength(100, ErrorMessage = "Title length can't be more than 100 characters")]
		public string Title { get; set; } = string.Empty;
		[StringLength(500, ErrorMessage = "Description length can't be more than 500 characters")]
		public string Description { get; set; } = string.Empty;
		[Range(0, 100, ErrorMessage = "Complete percentege must be a value between 0 and 100")]
		public int PercComplete { get; set; } = 0;
		public DateTime DateOfExpiry { get; set; }
	}
}
