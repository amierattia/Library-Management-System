using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BorrowRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BorrowRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BorrowRecords.Include(b => b.Book).Include(b => b.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BorrowRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowRecord = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.BorrowRecordId == id);
            if (borrowRecord == null)
            {
                return NotFound();
            }

            return View(borrowRecord);
        }

        // GET: BorrowRecords/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "ISBN");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Name");
            return View();
        }


        // POST: BorrowRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowRecordId,StudentId,BookId,BorrowDate,ReturnDate")] borrowRecordVM borrowRecordVM)
        {
            if (ModelState.IsValid)
            {
                // تحويل الـ ViewModel إلى Model
                var borrowRecord = new BorrowRecord
                {
                    StudentId = borrowRecordVM.StudentId,
                    BookId = borrowRecordVM.BookId,
                    BorrowDate = borrowRecordVM.BorrowDate,
                    ReturnDate = borrowRecordVM.ReturnDate
                };

                _context.Add(borrowRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
    
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "ISBN", borrowRecordVM.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Name", borrowRecordVM.StudentId);
            return View(borrowRecordVM);
        }


        // GET: BorrowRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowRecord = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.BorrowRecordId == id);

            if (borrowRecord == null)
            {
                return NotFound();
            }

            
            var borrowRecordVM = new borrowRecordVM
            {
                BorrowRecordId = borrowRecord.BorrowRecordId,
                StudentId = borrowRecord.StudentId,
                BookId = borrowRecord.BookId,
                BorrowDate = borrowRecord.BorrowDate,
                ReturnDate = borrowRecord.ReturnDate,
                StudentName = borrowRecord.Student.Name,
                BookISBN = borrowRecord.Book.ISBN
            };

            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "ISBN", borrowRecordVM.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Name", borrowRecordVM.StudentId);
            return View(borrowRecordVM);
        }

        
// POST: BorrowRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowRecordId,StudentId,BookId,BorrowDate,ReturnDate")] borrowRecordVM borrowRecordVM)
        {
            if (id != borrowRecordVM.BorrowRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var borrowRecord = new BorrowRecord
                    {
                        BorrowRecordId = borrowRecordVM.BorrowRecordId,
                        StudentId = borrowRecordVM.StudentId,
                        BookId = borrowRecordVM.BookId,
                        BorrowDate = borrowRecordVM.BorrowDate,
                        ReturnDate = borrowRecordVM.ReturnDate
                    };

                    _context.Update(borrowRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowRecordExists(borrowRecordVM.BorrowRecordId))
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

            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "ISBN", borrowRecordVM.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "Name", borrowRecordVM.StudentId);
            return View(borrowRecordVM);
        }


        // GET: BorrowRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowRecord = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.BorrowRecordId == id);
            if (borrowRecord == null)
            {
                return NotFound();
            }

            return View(borrowRecord);
        }

        // POST: BorrowRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowRecord = await _context.BorrowRecords.FindAsync(id);
            if (borrowRecord != null)
            {
                _context.BorrowRecords.Remove(borrowRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowRecordExists(int id)
        {
            return _context.BorrowRecords.Any(e => e.BorrowRecordId == id);
        }
    }
}
