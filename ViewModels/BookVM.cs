using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels;

public class BookVM
{
    public int BookId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Author { get; set; }

    [Required]
    public string ISBN { get; set; }

    [Range(0, 1000)]
    public int Quantity { get; set; }
}