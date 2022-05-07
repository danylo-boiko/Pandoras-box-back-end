namespace Videos.Core.Database.Entities;

public class VideoTag
{
    public int VideoId { get; set; }
    public int TagId { get; set; }
    public Video Video { get; set; }
}