namespace Tags.Core.Database.Entities;

public class Tag
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}