using Entities;
using RepositoryContracts;

namespace InMemoryRepository;

public class CommentInMemoryRepository :  ICommentRepository
{
    private List<Comment> comments;

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(p => p.Id) + 1 : 1;
        comments.Add(comment);
        
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(p => p.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with id '{comment.Id}' not found");
        }
        
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToDelete = comments.SingleOrDefault(p => p.Id == id);
        if(commentToDelete is null)
        {
            throw new InvalidOperationException($"Comment with id '{id}' not found");
        }
        
        comments.Remove(commentToDelete);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToGet = comments.SingleOrDefault(p => p.Id == id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException($"Comment with id '{id}' not found");
        }
        
        return Task.FromResult(commentToGet);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }
}