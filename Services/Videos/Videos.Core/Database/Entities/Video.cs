namespace Videos.Core.Database.Entities;

public class Video
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string VideoUrl { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<VideoTag> VideoTags { get; set; }
}