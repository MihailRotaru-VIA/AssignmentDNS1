using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ViewPostsView
{
    private IPostRepository _postRepository;
    private ICommentRepository _commentRepository;

    public ViewPostsView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[ID]    [UserID]    [Title]");
            List<Post> posts = _postRepository.GetManyAsync().ToList();
            List<Comment> comments = _commentRepository.GetManyAsync().ToList();
            foreach (Post post in posts)
            {
                Console.WriteLine($"[{post.Id}]       [{post.UserId}]        [{post.Title}]");
            }
            Console.WriteLine("[0] X Exit");
            Console.WriteLine("Which post do you want to see?");
            Console.Write("> ");
            string? postId = Console.ReadLine()?.Trim();
            if (int.TryParse(postId, out int id))
            {
                if (id == 0)
                {
                    return;
                }
                foreach (Post post in posts)
                {
                    if (post.Id == id)
                    {
                        Console.Clear();
                        Console.WriteLine($"POST_ID: {post.Id}    Title: {post.Title}");
                        Console.WriteLine(post.Body);
                        Console.WriteLine($"Written by user_id: {post.UserId}");
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Comments: ");
                        Console.Write("\n");
                        foreach (Comment comment in comments)
                        {
                            if (comment.PostId == id)
                            {
                                Console.WriteLine($"Comment from [{comment.UserId}]: {comment.Body}");
                            }
                        }
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid post id!!");
                continue;
            }
            
        }
    }
}