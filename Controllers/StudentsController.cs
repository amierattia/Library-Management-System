
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Invalid student ID.";
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                TempData["Error"] = "Student not found.";
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,Email,Phone")] StudentVm studentVm)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = studentVm.Name,
                    Email = studentVm.Email,
                    Phone = studentVm.Phone
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Please correct the form errors.";
            return View(studentVm);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Invalid student ID.";
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                TempData["Error"] = "Student not found.";
                return NotFound();
            }

            var studentVm = new StudentVm
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone
            };

            return View(studentVm);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Name,Email,Phone")] StudentVm studentVm)
        {
            if (id != studentVm.StudentId)
            {
                TempData["Error"] = "Student ID mismatch.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var student = await _context.Students.FindAsync(id);
                    if (student == null)
                    {
                        TempData["Error"] = "Student not found.";
                        return NotFound();
                    }

                    student.Name = studentVm.Name;
                    student.Email = studentVm.Email;
                    student.Phone = studentVm.Phone;

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Student updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(studentVm.StudentId))
                    {
                        TempData["Error"] = "Student no longer exists.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Please correct the form errors.";
            return View(studentVm);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Invalid student ID.";
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                TempData["Error"] = "Student not found.";
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Student already deleted or not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
