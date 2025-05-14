using System.ComponentModel.DataAnnotations;
namespace LibraryManagementSystem.Models;


public class Book
{
    public int BookId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Author { get; set; }

    [Required]
    public string ISBN { get; set; }

    [Range(0, 1000)]
    public int Quantity { get; set; }

    public ICollection<BorrowRecord> BorrowRecords { get; set; }
}
