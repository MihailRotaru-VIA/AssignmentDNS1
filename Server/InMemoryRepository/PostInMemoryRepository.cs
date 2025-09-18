using Entities;
using RepositoryContracts;

namespace InMemoryRepository;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new();

    public PostInMemoryRepository()
    {
        AddPostDummies();
    }
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);

        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? postToGet = posts.SingleOrDefault(p => p.Id == id);
        if (postToGet is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return Task.FromResult(postToGet);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
    
    private void AddPostDummies()
    {
        Post post = new Post("How is your day going guys?", "My day is going very good!", 2);
        post.Id = 1;
        posts.Add(post);
        post = new Post("Today im feeling seek", "IDK why but i feel very bad today, any thoughts?", 4);
        post.Id = 2;
        posts.Add(post);
        post = new Post("I dont like this app", "This app is way too primitive, im going back to Reddit", 1);
        post.Id = 3;
        posts.Add(post);
        post = new Post("How is this legal?", "My landlord is telling i will be evicted if i dont pay rent! Like WHO ARE YOU?!", 1);
        post.Id = 4;
        posts.Add(post);
        post = new Post("I LOVE VIA", "VIA UC is like a second home to me lol", 3);
        post.Id = 5;
        posts.Add(post);
    }
}