using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentsView
{
    private string _body;
    private int _postId;
    private int _userId;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public CreateCommentsView(ICommentRepository commentRepository, IUserRepository userRepository,
        IPostRepository postRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            do
            {
                Console.WriteLine("Enter the id of the user: ");
                Console.Write("> ");
                string? userId = Console.ReadLine();
                List<User> users = _userRepository.GetManyAsync().ToList();
                if (int.TryParse(userId, out int id))
                {
                    foreach (User user in users)
                    {
                        if (id == user.Id)
                        {
                            Console.WriteLine("Enter password:");
                            Console.Write("> ");
                            string? password = Console.ReadLine()?.Trim();
                            if (password is not null && password == user.Password)
                            {
                                _userId = id;
                                break;
                            }

                            Console.WriteLine("Invalid password");
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You entered an invalid id.");
                    continue;
                }
            } while (_userId == 0);
            
            do
            {
                Console.WriteLine("Enter the id of the post: ");
                Console.Write("> ");
                string? postId = Console.ReadLine();
                List<Post> posts = _postRepository.GetManyAsync().ToList();
                if (int.TryParse(postId, out int id))
                {
                    foreach (Post post in posts)
                    {
                        if (id == post.Id)
                        {
                            _postId = id;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You entered an invalid id.");
                    continue;
                }
            } while (_postId == 0);
            
            do
            {
                Console.WriteLine("Enter the body of the comment: ");
                Console.Write("> ");
                _body = Console.ReadLine();
                if (_body?.Length < 5)
                {
                    Console.WriteLine("Invalid body -- Body should be longer than 5 characters.");
                }
            } while (_body is null || _body.Length < 5);
            
            Console.WriteLine("\n");
            Console.WriteLine("PREVIEW: ");
            Console.WriteLine(_body);
            Console.WriteLine($"Comment by {_userId}, on post {_postId}");
            Console.WriteLine("Are you sure you want to comment? (y/n)");
            Console.Write("> ");
            string? choice = Console.ReadLine();
            if (choice == "y" || choice == "Y")
            {
                Comment tempComment = new Comment(_body, _postId, _userId);
                await  _commentRepository.AddAsync(tempComment);
                Console.WriteLine("Comment added!!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return;
            }
        }
    }
}