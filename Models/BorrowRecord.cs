using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;



public class BorrowRecord
{
    public int BorrowRecordId { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    [DataType(DataType.Date)]
    public DateTime BorrowDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ReturnDate { get; set; }
}
