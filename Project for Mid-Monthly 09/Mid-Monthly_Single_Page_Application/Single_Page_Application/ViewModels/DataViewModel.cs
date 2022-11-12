using Single_Page_Application.Models;

namespace Single_Page_Application.ViewModels
{
    public class DataViewModel
    {
        public int SelectedOrderId { get; set; }
        public IEnumerable<Author> Authors { get; set; } = default!;
        public IEnumerable<BookAuthor> BookAuthors { get; set; } = default!;
        public IEnumerable<Book> Books { get; set; } = default!;
        public IEnumerable<Genre> Genres { get; set; } = default!;
        public IEnumerable<Publisher> Publishers { get; set; } = default!;
        public IEnumerable<Customer> Customers { get; set; } = default!;
        public IEnumerable<SaleDetail> SaleDetails { get; set; } = default!;
    }
}
