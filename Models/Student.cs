using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;


public class Student
{
    public int StudentId { get; set; }

    [Required]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Phone { get; set; }

    public ICollection<BorrowRecord> BorrowRecords { get; set; }
}
