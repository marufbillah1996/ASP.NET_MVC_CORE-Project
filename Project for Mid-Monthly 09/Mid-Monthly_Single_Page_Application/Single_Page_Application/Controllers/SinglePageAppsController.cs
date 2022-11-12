using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Single_Page_Application.Models;
using Single_Page_Application.ViewModels;

namespace Single_Page_Application.Controllers
{
    public class SinglePageAppsController : Controller
    {
        BookSellerDbContext db;
        IWebHostEnvironment env;
        public SinglePageAppsController(BookSellerDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        public async Task<IActionResult> Index()
        {
            var id = 0;
            if (db.Customers.Any())
            {
                id = db.Customers.ToList()[0].CustomerID;
            }
            DataViewModel data = new DataViewModel();
            data.SelectedOrderId = id;
            data.Authors = await db.Authors.ToListAsync();
            data.Genres = await db.Genres.ToListAsync();
            data.Publishers = await db.Publishers.ToListAsync();
            data.BookAuthors = await db.BookAuthors.ToListAsync();
            data.Customers = await db.Customers.ToListAsync();
            data.Books = await db.Books.ToListAsync();
            data.SaleDetails = await db.SaleDetails.Include(x => x.Book).Where(oi => oi.CustomerID == id).ToListAsync();


            return View(data);
        }
        #region child actions
        public async Task<IActionResult> GetSelectedSaleDetails(int id)
        {

            var SaleDetails = await db.SaleDetails.Include(x => x.Book).Include(x=>x.Customer).Where(oi => oi.CustomerID == id).ToListAsync();
            return PartialView("_SaleDetailsTable", SaleDetails);
        }
        public IActionResult CreateAuthor()
        {
            return PartialView("_CreateAuthor");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(Author a)
        {
            if (ModelState.IsValid)
            {
                await db.Authors.AddAsync(a);
                await db.SaveChangesAsync();
                return Json(a);
            }
            return BadRequest("Unexpected error");
        }
        public async Task<IActionResult> EditAuthor(int id)
        {
            var data = await db.Authors.FirstOrDefaultAsync(c => c.AuthorID == id);
            return PartialView("_EditAuthor", data);
        }
        [HttpPost]
        public async Task<IActionResult> EditAuthor(Author a)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(a);
        
            }
            return BadRequest("Unexpected error");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (!await db.BookAuthors.Include(x=>x.Book).AnyAsync(o => o.AuthorID == id))
            {
                var o = new Author { AuthorID = id };
                db.Entry(o).State = EntityState.Deleted;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = "Data deleted" });
            }
            return Json(new { success = false, message = "Cannot delete, item has related child." });
        }
        //genre work start here
        public IActionResult CreateGenre()
        {
            return PartialView("_CreateGenre");
        }
        [HttpPost]
        public async Task<IActionResult> CreateGenre(Genre g)
        {
            if (ModelState.IsValid)
            {
                await db.Genres.AddAsync(g);
                await db.SaveChangesAsync();
                return Json(g);
            }
            return BadRequest("Unexpected error");
        }
        public async Task<IActionResult> EditGenre(int id)
        {
            var data = await db.Genres.FirstOrDefaultAsync(c => c.GenreID == id);
            return PartialView("_EditGenre", data);
        }
        [HttpPost]
        public async Task<IActionResult> EditGenre(Genre g)
        {
            if (ModelState.IsValid)
            {
                db.Entry(g).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(g);

            }
            return BadRequest("Unexpected error");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (!await db.Books.AnyAsync(o => o.GenreID == id))
            {
                var o = new Genre { GenreID = id };
                db.Entry(o).State = EntityState.Deleted;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = "Data deleted" });
            }
            return Json(new { success = false, message = "Cannot delete, item has related child." });
        }
        //genre  work end here
        //publisher work start here
        public IActionResult CreatePublisher()
        {
            return PartialView("_CreatePublisher");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePublisher(Publisher p)
        {
            if (ModelState.IsValid)
            {
                await db.Publishers.AddAsync(p);
                await db.SaveChangesAsync();
                return Json(p);
            }
            return BadRequest("Unexpected error");
        }
        public async Task<IActionResult> EditPublisher(int id)
        {
            var data = await db.Publishers.FirstOrDefaultAsync(c => c.PublisherID == id);
            return PartialView("_EditPublisher", data);
        }
        [HttpPost]
        public async Task<IActionResult> EditPublisher(Publisher p)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(p);

            }
            return BadRequest("Unexpected error");
        }
        [HttpPost]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            if (!await db.Books.AnyAsync(o => o.PublisherID == id))
            {
                var o = new Publisher { PublisherID = id };
                db.Entry(o).State = EntityState.Deleted;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = "Data deleted" });
            }
            return Json(new { success = false, message = "Cannot delete, item has related child." });
        }
        //Publisher  work end here
        //work with book start here
        public async Task<IActionResult> CreateBook()
        {
            //var fromDatabasegenre = new SelectList(await db.Genres.ToListAsync(), "GenreID", "GenreName");
            //ViewData["Genres"] = fromDatabasegenre;
            //var fromDatabasepublisher = new SelectList(await db.Publishers.ToListAsync(), "PublisherID", "PublisherName");
            //ViewData["Publishers"] = fromDatabasepublisher;
            ViewData["Genres"] = await db.Genres.ToListAsync();
            ViewData["Publishers"] = await db.Publishers.ToListAsync();
            return PartialView("_CreateBook");
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookInputModel b)
        {
           
            if (ModelState.IsValid)
            {
                var book = new Book { BookName = b.BookName, Price = b.Price,PublishDate = b.PublishDate,GenreID=b.GenreID,PublisherID=b.PublisherID ,Available=b.Available};
                string fileName = Guid.NewGuid() + Path.GetExtension(b.Picture.FileName);
                string savePath = Path.Combine(this.env.WebRootPath, "Pictures", fileName);
                var fs = new FileStream(savePath, FileMode.Create);
                b.Picture.CopyTo(fs);
                fs.Close();
                book.Picture = fileName;
                await db.Books.AddAsync(book);
                await db.SaveChangesAsync();
                var x = GetGenrePublisher(book.BookID);
                return Json(x);


            }
            ViewData["Genres"] = await db.Genres.ToListAsync();
            ViewData["Publishers"] = await db.Publishers.ToListAsync();

            //var fromDatabasegenre = new SelectList(await db.Genres.ToListAsync(), "GenreID", "GenreName");
            //    ViewData["Genres"] = fromDatabasegenre;
            //var fromDatabasepublisher = new SelectList(await db.Publishers.ToListAsync(), "PublisherID", "PublisherName");
            //ViewData["Publishers"] = fromDatabasepublisher;

            return BadRequest("Falied to insert product");
        }
        public async Task<IActionResult> EditBook(int id)
        {
            ViewData["Genres"] = await db.Genres.ToListAsync();
            ViewData["Publishers"] = await db.Publishers.ToListAsync();
            var data = await db.Books.FirstAsync(x => x.BookID == id);
            ViewData["CurrentPic"] = data.Picture;
            return PartialView("_EditBook", new BookEditModel { BookID = data.BookID, BookName = data.BookName, Price = data.Price, PublishDate = data.PublishDate, GenreID = data.GenreID, PublisherID = data.PublisherID,Available=data.Available });
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BookEditModel b)
        {
            if (ModelState.IsValid)
            {
                var book = await db.Books.FirstAsync(x => x.BookID == b.BookID);
                book.BookName = b.BookName;
                book.Price = b.Price;
                book.PublishDate = b.PublishDate;
                book.GenreID = b.GenreID;
                book.PublisherID = b.PublisherID;
                book.Available = b.Available;
                if (b.Picture != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(b.Picture.FileName);
                    string savePath = Path.Combine(this.env.WebRootPath, "Pictures", fileName);
                    var fs = new FileStream(savePath, FileMode.Create);
                    b.Picture.CopyTo(fs);
                    fs.Close();
                    book.Picture = fileName;
                }

                
                await db.SaveChangesAsync();
                var x = GetGenrePublisher(book.BookID);
                return Json(x);


            }
            ViewData["Genres"] = await db.Genres.ToListAsync();
            ViewData["Publishers"] = await db.Publishers.ToListAsync();
            return BadRequest();
        }
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (!await db.SaleDetails.AnyAsync(o => o.BookID == id))
            {
                var o = new Book { BookID = id };
                db.Entry(o).State = EntityState.Deleted;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = "Data deleted" });
            }
            return Json(new { success = false, message = "Cannot delete, item has related child." });
        }
        private Book? GetGenrePublisher(int id)
        {
            return db.Books.Include(x => x.Genre).Include(x=>x.Publisher).FirstOrDefault(x => x.BookID == id);

        }
        //work with book end here

        public async Task<IActionResult> CreateCustomer()
        {
            ViewData["Books"] = await db.Books.ToListAsync();
            
            return PartialView("_CreateCustomer");
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer o, int[] BookID, int[] Quantity)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < BookID.Length; i++)
                {
                    o.Sales.Add(new SaleDetail{ BookID = BookID[i], Quantity = Quantity[i] });
                }
                await db.Customers.AddAsync(o);

                await db.SaveChangesAsync();


                var c = await GetCustomer(o.CustomerID);
                return Json(c);
            }
            return BadRequest();
        }
        private async Task<Customer?> GetCustomer(int id)
        {
            var o = await db.Customers.FirstOrDefaultAsync(x => x.CustomerID == id);
            return o;
        }
        public async Task<IActionResult> EditCustomer(int id)
        {
            ViewData["Books"] = await db.Books.ToListAsync();
            
            var data = await db.Customers
                .Include(x => x.Sales).ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.CustomerID == id);
            return PartialView("_EditCustomer", data);

        }
        [HttpPost]
        public async Task<IActionResult> EditCustomer(Customer c)
        {
            if (ModelState.IsValid)
            {
                var existing = await db.Customers.FirstAsync(x => x.CustomerID == c.CustomerID) ;
                existing.CustomersName = c.CustomersName;
                existing.CustomerPhone = c.CustomerPhone;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return Json(existing);
            }

            return BadRequest();
        }
        public async Task<IActionResult> CreateItem()
        {
            ViewData["Books"] = await db.Books.ToListAsync();
            return PartialView("_CreateItem");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var o = new Customer { CustomerID = id };
            db.Entry(o).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Data deleted" });
        }
        public async Task<IActionResult> CreateSaleDetails(int id)
        {
            ViewData["CustomerID"] = id;
            ViewData["Books"] = await db.Books.ToListAsync();
            return PartialView("_CreateSaleDetails");
        }
        [HttpPost]
        public async Task<IActionResult> CreateSaleDetails(SaleDetail sd)
        {
            if (ModelState.IsValid)
            {
                await db.SaleDetails.AddAsync(sd);
                await db.SaveChangesAsync();
                var o = await GetSaleDetails(sd.CustomerID,sd.BookID);
                return Json(o);
            }
            ViewData["Customers"] = await db.Customers.ToListAsync();
            ViewData["Books"] = await db.Books.ToListAsync();
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSaleDetails([FromQuery] int oid, [FromQuery] int pid)
        {

            var o = new SaleDetail { BookID = pid, CustomerID = oid };
            db.Entry(o).State = EntityState.Deleted;

            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Data deleted" });

        }
        private async Task<SaleDetail> GetSaleDetails(int cid, int bid)
        {
            var sd = await db.SaleDetails
                .Include(o => o.Customer)
                .Include(o => o.Book)
                .FirstAsync(x => x.CustomerID == cid && x.BookID == bid);
            return sd;
        }
        #endregion
    }
}
