using Entities;
using RepositoryContracts;

namespace InMemoryRepository;

public class CommentInMemoryRepository :  ICommentRepository
{
    private List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        AddCommentsDummies();
    }
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

    public void AddCommentsDummies()
    {
        Comment comment = new Comment("Glad to hear your day is going well!", 1, 3);
        comment.Id = 1;
        comments.Add(comment);

        comment = new Comment("Mine is pretty chill so far.", 1, 5);
        comment.Id = 2;
        comments.Add(comment);

        comment = new Comment("Having a rough morning but it's getting better.", 1, 2);
        comment.Id = 3;
        comments.Add(comment);

        comment = new Comment("Maybe you’re catching a cold, get some rest!", 2, 1);
        comment.Id = 4;
        comments.Add(comment);

        comment = new Comment("Drink plenty of water and tea, that usually helps me.", 2, 5);
        comment.Id = 5;
        comments.Add(comment);

        comment = new Comment("Hope you feel better soon!", 2, 3);
        comment.Id = 6;
        comments.Add(comment);

        comment = new Comment("Ouch, harsh. What don’t you like about it?", 3, 4);
        comment.Id = 7;
        comments.Add(comment);

        comment = new Comment("Reddit has more features, true, but give this app some time.", 3, 2);
        comment.Id = 8;
        comments.Add(comment);

        comment = new Comment("I actually prefer this app, it's more chill.", 3, 5);
        comment.Id = 9;
        comments.Add(comment);

        comment = new Comment("Yikes, that sounds serious. Do you have a contract?", 4, 3);
        comment.Id = 10;
        comments.Add(comment);

        comment = new Comment("You should probably check your rights as a tenant.", 4, 2);
        comment.Id = 11;
        comments.Add(comment);

        comment = new Comment("If you’re in Denmark, contact Lejernes Landsorganisation.", 4, 5);
        comment.Id = 12;
        comments.Add(comment);
        
        comment = new Comment("Haha same here, VIA feels like home.", 5, 1);
        comment.Id = 13;
        comments.Add(comment);

        comment = new Comment("What programme are you studying at VIA?", 5, 4);
        comment.Id = 14;
        comments.Add(comment);

        comment = new Comment("Totally agree, love the campus vibe.", 5, 2);
        comment.Id = 15;
        comments.Add(comment);
    }
}