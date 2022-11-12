using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Single_Page_Application.Models
{
	public enum Gender { Male = 1, Female }
	public class Author
	{
		public int AuthorID { get; set; }
		[Required, StringLength(30), Display(Name = "Author Name")]
		public string AuthorName { get; set; } = default!;
		[Required, StringLength(50), Display(Name = "Author Address")]
		public string AuthorAddress { get; set; } = default!;
		[EnumDataType(typeof(Gender))]
		public Gender Gender { get; set; }
		public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

	}

	public class Genre
	{
		public int GenreID { get; set; }
		[Required, StringLength(30), Display(Name = "Genre Name")]
		public string GenreName { get; set; } = default!;
		public virtual ICollection<Book> Books { get; set; } = new List<Book>();
	}

	public class Publisher
	{
		public int PublisherID { get; set; }
		[Required, StringLength(30), Display(Name = "Publisher Name")]
		public string PublisherName { get; set; } = default!;
		public virtual ICollection<Book> Books { get; set; } = new List<Book>();
	}

	public class Book
	{
		public int BookID { get; set; }
		[Required, StringLength(30), Display(Name = "Book Name")]
		public string BookName { get; set; } = default!;
		[Required, Column(TypeName = "money"), DisplayFormat(DataFormatString = "{0:0.00}")]
		public decimal Price { get; set; }
		[Required, Column(TypeName = "date"),
			Display(Name = "Publish Date"),
			DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
			ApplyFormatInEditMode = true)]
		public DateTime PublishDate { get; set; }
		[Required, StringLength(150)]
		public string Picture { get; set; } = default!;
		public bool Available { get; set; }
		[ForeignKey("Genre")]
		public int GenreID { get; set; }
		[ForeignKey("Publisher")]
		public int PublisherID { get; set; }
		public virtual Genre? Genre { get; set; } = default!;
		public virtual Publisher? Publisher { get; set; } = default!;
		public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
		public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
	}
	public class BookAuthor
	{
		[ForeignKey("Author")]
		public int AuthorID { get; set; }
		[ForeignKey("Book")]
		public int BookID { get; set; }
		public virtual Author Author { get; set; } = default!;
		public virtual Book Book { get; set; } = default!;

	}
	public class Customer
	{
		public int CustomerID { get; set; }
		[Required, StringLength(30), Display(Name = "Customer Name")]
		public string CustomersName { get; set; } = default!;
		[Required, StringLength(30), Display(Name = "Customer Phone")]
		public string CustomerPhone { get; set; } = default!;
		public virtual ICollection<SaleDetail> Sales { get; set; } = new List<SaleDetail>();
	}

	public class SaleDetail
	{
		[ForeignKey("Customer")]
		public int CustomerID { get; set; }
		[ForeignKey("Book")]
		public int BookID { get; set; }
		[Required]
		public int Quantity { get; set; }
		public virtual Customer? Customer { get; set; } = default!;
		public virtual Book? Book { get; set; } = default!;

	}
	public class BookSellerDbContext : DbContext
	{
		public BookSellerDbContext(DbContextOptions<BookSellerDbContext> options) : base(options) { }
		public DbSet<Author> Authors { get; set; } = default!;
		public DbSet<Genre> Genres { get; set; } = default!;
		public DbSet<Publisher> Publishers { get; set; } = default!;
		public DbSet<Book> Books { get; set; } = default!;
		public DbSet<BookAuthor> BookAuthors { get; set; } = default!;
		public DbSet<Customer> Customers { get; set; } = default!;
		public DbSet<SaleDetail> SaleDetails { get; set; } = default!;
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookID, ba.AuthorID });
			modelBuilder.Entity<SaleDetail>().HasKey(ba => new { ba.CustomerID, ba.BookID });
		}

	}
}
