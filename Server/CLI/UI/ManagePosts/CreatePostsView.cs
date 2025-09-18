using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostsView
{
    private string _title;
    private string _body;
    private int _userId;
    private IPostRepository _postRepository;
    private IUserRepository _userRepository;

    public CreatePostsView(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
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
                Console.WriteLine("Enter the title of the post: ");
                Console.Write("> ");
                _title = Console.ReadLine();
                if (_title?.Length < 5)
                {
                    Console.WriteLine("Invalid title -- Title should be longer than 5 characters.");
                }
            } while (_title is null || _title.Length < 5);

            do
            {
                Console.WriteLine("Enter the body of the post: ");
                Console.Write("> ");
                _body = Console.ReadLine();
                if (_body?.Length < 5)
                {
                    Console.WriteLine("Invalid body -- Body should be longer than 5 characters.");
                }
            } while (_body is null || _body.Length < 5);
            
            Console.WriteLine("Preview: ");
            Console.WriteLine(_title);
            Console.WriteLine(_body);
            Console.WriteLine($"By {_userId}");
            Console.WriteLine("Are you sure you want to post it? (y/n)");
            Console.Write("> ");
            string? choice = Console.ReadLine();
            if (choice == "y" || choice == "y")
            {
                Post post = new Post(_title, _body, _userId);
                await _postRepository.AddAsync(post);
                Console.WriteLine("Post added!!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return;
            }
        }
    }
}