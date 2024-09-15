namespace ZigZag.Library.DataAccess.Models.Requests;

public class BookRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Isbn { get; set; }
    public required string PublishedDate { get; set; }
}

