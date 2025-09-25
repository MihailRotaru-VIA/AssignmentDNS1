using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepo;

public class CommentFileRepo : ICommentRepository
{
    private readonly string _filePath = "comments.txt";

    public CommentFileRepo()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = await LoadComments();
        int maxId = comments.Count > 0 ?  comments.Max(x => x.Id) : 0;
        comment.Id = maxId + 1;
        comments.Add(comment);
        await SaveComments(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        List<Comment> comments = await LoadComments();
        Comment? existingComment = comments.SingleOrDefault(x => x.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with id '{comment.Id}' not found");
        }
        
        comments.Remove(existingComment);
        comments.Add(comment);
        await SaveComments(comments);
    }

    public async Task DeleteAsync(int id)
    {
        List<Comment> comments = await LoadComments();
        Comment? existingComment = comments.SingleOrDefault(x => x.Id == id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with id '{id}' not found");
        }
        comments.Remove(existingComment);
        await SaveComments(comments);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        List<Comment> comments = await LoadComments();
        Comment? existingComment = comments.SingleOrDefault(x => x.Id == id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with id '{id}' not found");
        }
        return existingComment;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return LoadComments().Result.AsQueryable();
    }

    private async Task<List<Comment>> LoadComments()
    {
        string commentsAsJson  = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
    }

    private async Task SaveComments(List<Comment> comments)
    {
        string commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }
}