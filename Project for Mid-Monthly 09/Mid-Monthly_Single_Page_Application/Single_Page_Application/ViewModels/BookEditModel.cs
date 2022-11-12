using Single_Page_Application.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Single_Page_Application.ViewModels
{
    public class BookEditModel
    {
		public int BookID { get; set; }
		[Required, StringLength(30), Display(Name = "Book Name")]
		public string BookName { get; set; } = default!;
		[Required, DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:0.00}")]
		public decimal Price { get; set; }
		[Required, DataType(DataType.Date),
			Display(Name = "Publish Date"),
			DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
			ApplyFormatInEditMode = true)]
		public DateTime PublishDate { get; set; }
		
		public IFormFile? Picture { get; set; } = default!;
		public bool Available { get; set; }
		[ForeignKey("Genre")]
		public int GenreID { get; set; }
		[ForeignKey("Publisher")]
		public int PublisherID { get; set; }
		public virtual Genre? Genre { get; set; } = default!;
		public virtual Publisher? Publisher { get; set; } = default!;
	}
}
