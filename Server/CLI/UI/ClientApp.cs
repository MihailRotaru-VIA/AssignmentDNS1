using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class ClientApp
{
    private IUserRepository _userRepository;
    private IPostRepository  _postRepository;
    private ICommentRepository _commentRepository;

    public ClientApp(IUserRepository userRepository, ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("CLI started SUCCESSFULLY!\n");

        while (true)
        {
            Console.WriteLine("[1] Users Page");
            Console.WriteLine("[2] Posts Page");
            Console.WriteLine("[3] Comments Page");
            Console.WriteLine("[4] X Exit");
            Console.Write("> ");
            
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Please enter a VALID choice!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ManageUsersView manageUsers = new ManageUsersView(_userRepository);
                    await manageUsers.StartAsync();
                    break;
                case 2:
                    ManagePostsView managePosts = new ManagePostsView(_postRepository, _userRepository,  _commentRepository);
                    await managePosts.StartAsync();
                    break;
                case 3:
                    ManageCommentsView manageComments = new ManageCommentsView(_commentRepository, _userRepository, _postRepository);
                    await manageComments.StartAsync();
                    break;
                case 4:
                    Console.WriteLine("See you again!");
                    return;
                default:
                    Console.WriteLine("Please enter a VALID choice!");
                    break;
            }
            
            Console.WriteLine();
        }
    }
}