using System.ComponentModel.DataAnnotations;

namespace ZigZag.Library.DataAccess.Models;

public class Book
{
    public int Id { get; set; }
    [MaxLength(50, ErrorMessage = "The Title cannot exceed 50 characters.")]
    public string? Title { get; set; }
    [MaxLength(100, ErrorMessage = "The Author name cannot exceed 100 characters.")]
    public string? Author { get; set; }
    [MaxLength(13, ErrorMessage = "The ISBN cannot exceed 13 characters.")]
    public string? Isbn { get; set; }
    [Required]
    public DateOnly PublishedDate { get; set; }
}
