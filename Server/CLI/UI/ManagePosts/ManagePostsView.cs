using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private IPostRepository _postRepository;
    private IUserRepository _userRepository;
    private ICommentRepository _commentRepository;

    public ManagePostsView(IPostRepository postRepository,  IUserRepository userRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[1] Create Post");
            Console.WriteLine("[2] Edit Post");
            Console.WriteLine("[3] Delete Post");
            Console.WriteLine("[4] View All Posts");
            Console.WriteLine("[5] X Exit");
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case 1:
                        await RedirectToCreate();
                        break;
                    case 2:
                        await RedirectToUpdate();
                        break;
                    case 3:
                        await RedirectToDelete();
                        break;
                    case 4:
                        await RedirectToView();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number");
            }
        }
    }
    
    private async Task RedirectToCreate()
    {
        CreatePostsView createPosts = new  CreatePostsView(_postRepository, _userRepository);
        await createPosts.StartAsync();
    }

    private async Task RedirectToUpdate()
    {
        //UpdatePostsView updatePosts = new UpdatePostsView(_postRepository);
        //await updatePosts.StartAsync();
    }

    private async Task RedirectToDelete()
    {
        //DeletePostsView deletePosts = new DeletePostsView(_postRepository);
        //await deletePosts.StartAsync();
    }

    private async Task RedirectToView()
    {
        ViewPostsView viewPosts = new ViewPostsView(_postRepository, _commentRepository);
        await viewPosts.StartAsync();
    }
}