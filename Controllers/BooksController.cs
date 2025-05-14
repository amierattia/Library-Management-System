using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            var bookVMs = books.Select(b => new BookVM
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Quantity = b.Quantity
            }).ToList();

            return View(bookVMs);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Book ID not provided!";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToAction(nameof(Index));
            }

            var bookVM = new BookVM
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Quantity = book.Quantity
            };

            return View(bookVM);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Author,ISBN,Quantity")] BookVM bookVM)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = bookVM.Title,
                    Author = bookVM.Author,
                    ISBN = bookVM.ISBN,
                    Quantity = bookVM.Quantity
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Invalid data. Please check the fields!";
            return View(bookVM);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Book ID not provided!";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToAction(nameof(Index));
            }

            var bookVM = new BookVM
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Quantity = book.Quantity
            };

            return View(bookVM);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,ISBN,Quantity")] BookVM bookVM)
        {
            if (id != bookVM.BookId)
            {
                TempData["ErrorMessage"] = "Book ID mismatch!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _context.Books.FindAsync(id);
                    if (book != null)
                    {
                        book.Title = bookVM.Title;
                        book.Author = bookVM.Author;
                        book.ISBN = bookVM.ISBN;
                        book.Quantity = bookVM.Quantity;

                        _context.Update(book);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Book updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Book not found!";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(bookVM.BookId))
                    {
                        TempData["ErrorMessage"] = "Book not found!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred while updating the book!";
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Invalid data. Please check the fields!";
            return View(bookVM);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Book ID not provided!";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToAction(nameof(Index));
            }

            var bookVM = new BookVM
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Quantity = book.Quantity
            };

            return View(bookVM);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Book not found!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
