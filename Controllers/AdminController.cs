using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers;
[Authorize]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Dashboard(string table = "Books")
    {
        ViewData["Books"] = _context.Books.Count();
        ViewData["Students"] = _context.Students.Count();
        ViewData["Borrows"] = _context.BorrowRecords.Count();

        ViewData["TableController"] = table;

        switch (table)
        {
            case "Students":
                ViewData["TableData"] = _context.Students.ToList();
                break;
            case "BorrowRecords":
                ViewData["TableData"] = _context.BorrowRecords
                    .Include(br => br.Student)
                    .Include(br => br.Book)
                    .ToList();
                break;
            default:
                ViewData["TableData"] = _context.Books.ToList();
                break;
        }

        return View();
    }
}
