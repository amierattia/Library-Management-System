using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels;

public class borrowRecordVM
{
    public int BorrowRecordId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required]
    public DateTime BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }
    
    
    public string? StudentName { get; set; }
    public string? BookISBN { get; set; }
}