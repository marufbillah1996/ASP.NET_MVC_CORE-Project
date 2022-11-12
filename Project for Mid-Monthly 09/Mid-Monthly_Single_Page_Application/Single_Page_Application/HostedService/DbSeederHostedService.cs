using Single_Page_Application.Models;

namespace Single_Page_Application.HostedService
{
    public class DbSeederHostedService : IHostedService
    {
        IServiceProvider serviceProvider;
        public DbSeederHostedService(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {

                var db = scope.ServiceProvider.GetRequiredService<BookSellerDbContext>();

                await SeedDbAsync(db);

            }
        }
        public async Task SeedDbAsync(BookSellerDbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!db.Authors.Any())
            {
                var A1 = new Author { AuthorName = "Maruf", AuthorAddress = "Gazipur", Gender = Gender.Male };
                await db.Authors.AddAsync(A1);
                var A2 = new Author { AuthorName = "Billah", AuthorAddress = "Mirpur", Gender = Gender.Male };
                await db.Authors.AddAsync(A2);
                var G1 = new Genre { GenreName = "Genre 01", };
                await db.Genres.AddAsync(G1);
                var G2 = new Genre { GenreName = "Genre 02", };
                await db.Genres.AddAsync(G2);
                var P1 = new Publisher { PublisherName = "Publisher 01" };
                await db.Publishers.AddAsync(P1);
                var P2 = new Publisher { PublisherName = "Publisher 02" };
                await db.Publishers.AddAsync(P2);
                var B1 = new Book { BookName = "Book 01", Price = 200.00M, PublishDate = new DateTime(1998, 10, 10), Available = true, Publisher = P1, Picture = "1.jpg" };
                G1.Books.Add(B1);
                var B2 = new Book { BookName = "Book 02", Price = 250.00M, PublishDate = new DateTime(2000, 10, 10), Available = true, Publisher = P2, Picture = "2.jpg" };
                G1.Books.Add(B2);
                var C1 = new Customer { CustomersName = "Customer 01",CustomerPhone = "01684939987" };
                await db.Customers.AddAsync(C1);
                var C2 = new Customer { CustomersName = "Customer 02", CustomerPhone = "01712071254" };
                await db.Customers.AddAsync(C2);
                B1.SaleDetails.Add(new SaleDetail { Book = B1, Customer = C1, Quantity = 1 });
                B2.SaleDetails.Add(new SaleDetail { Book = B2, Customer = C2, Quantity = 2 });
                BookAuthor BA = new BookAuthor { Author = A1, Book = B1 };
                await db.BookAuthors.AddAsync(BA);
                db.SaveChanges();
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
