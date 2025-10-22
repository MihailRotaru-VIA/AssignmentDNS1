namespace DTOs.Comment;

public class GetManyCommentsDto
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }}