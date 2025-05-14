using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels;

public class StudentVm
{
    public int StudentId { get; set; }

    [Required]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Phone { get; set; }
}