using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepo;

public class PostFileRepo : IPostRepository
{
    private readonly string _filePath = "posts.json";

    public PostFileRepo()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadPosts();
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;
        posts.Add(post);
        await SavePosts(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadPosts();
        Post?  existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' was not found.");
        }
        
        posts.Remove(existingPost);
        posts.Add(post);
        await SavePosts(posts);
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadPosts();
        Post? existingPost = posts.SingleOrDefault(p => p.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' was not found.");
        }
        
        posts.Remove(existingPost);
        await SavePosts(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadPosts();
        Post? existingPost = posts.SingleOrDefault(p => p.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' was not found.");
        }
        return existingPost;
    }

    public IQueryable<Post> GetManyAsync()
    {
        return LoadPosts().Result.AsQueryable();
    }

    private async Task<List<Post>> LoadPosts()
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
    }

    private async Task SavePosts(List<Post> posts)
    {
        string postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postsAsJson);
    }
}