using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Authorization;


namespace Project.Controllers
{
    public class Books : Controller
    {
        private readonly CoreModelsDataContext _context;

        public Books(CoreModelsDataContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? page)
        {
            var coreModelsDataContext = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookPublisher);
            
            // return View(await coreModelsDataContext.ToListAsync());

            int totalNumberOfPages = coreModelsDataContext.Count();

            int pages = (totalNumberOfPages / 5);

            if ((totalNumberOfPages % 5) !=  0) pages += 1;
            if (page == null)
                return View(
                    new ListWithPaginationModel<Book>(
                        coreModelsDataContext.Take<Book>(5).ToList(),
                        pages,
                        1
                    ));
            
            int numberToSkip = (page.Value-1) * 5;

            return View(new ListWithPaginationModel<Book>(
                coreModelsDataContext.Skip<Book>(numberToSkip).Take<Book>(5).ToList(),
                pages,
                page.Value));
        }

        // GET: Books/Details/5

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [Authorize("ADMIN")]
        public IActionResult Create()
        {
            string? currentUser = User.Identity?.Name;
            Console.WriteLine("yup");
            Console.WriteLine(currentUser);
            Console.WriteLine("heeeere \n \n\n\n\n");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ISBN,Title,Summary,Format,Language,AuthorId,Genre,Price,NumberOfPages,PublisherId,DatePublished")]
            Book book)
        {
            if (ModelState.IsValid)
            {
                var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\books\\";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        Console.WriteLine(filePath);
                        using (var inputStream = new FileStream(filePath + formFile.FileName, FileMode.Create))
                        {
                            // read file to stream
                            await formFile.CopyToAsync(inputStream);
                            // stream to byte array
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                            // +@item.get file name
                            string fName = formFile.FileName;
                        }

                        if (formFile.Name == "file")
                            book.FileName = formFile.FileName;
                        if (formFile.Name == "cover")
                            book.BookCover = formFile.FileName;
                    }
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            return View(book);
        }

        [HttpGet("/Books/Search/{key}")]
        public async Task<IActionResult> Search(string key)
        {
            List<Book> books = _context.SearchBook(key);
            // Console.WriteLine(books.Count);
            return Json(books);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind(
                "ISBN,Title,Summary,Format,Language,FileName,AuthorId,Genre,Price,NumberOfPages,PublisherId,DatePublished,BookCover")]
            Book book)
        {
            if (id != book.ISBN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ISBN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookAuthor)
                .Include(b => b.BookPublisher)
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return (_context.Books?.Any(e => e.ISBN == id)).GetValueOrDefault();
        }
    }
}