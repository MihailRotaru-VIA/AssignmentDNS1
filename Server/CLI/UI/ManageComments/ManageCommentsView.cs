using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView
{
    private ICommentRepository _commentRepository;
    private IUserRepository _userRepository;
    private IPostRepository _postRepository;

    public ManageCommentsView(ICommentRepository commentRepository,  IUserRepository userRepository, IPostRepository postRepository)
    {
        this._commentRepository = commentRepository;
        this._userRepository = userRepository;
        this._postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[1] Create Comment");
            Console.WriteLine("[2] Edit Comment");
            Console.WriteLine("[3] Delete Comment");
            Console.WriteLine("[4] View All Comments");
            Console.WriteLine("[5] X Exit");
            Console.Write("> ");
            string? input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case 1:
                        RedirectToCreate(_commentRepository, _userRepository, _postRepository);
                        break;
                    case 2:
                        RedirectToEdit(_commentRepository);
                        break;
                    case 3:
                        RedirectToDelete(_commentRepository);
                        break;
                    case 4:
                        RedirectToViewAllComments(_commentRepository);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }

    private async Task RedirectToCreate(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        CreateCommentsView createComments = new CreateCommentsView(_commentRepository,  _userRepository, _postRepository);
        await createComments.StartAsync();
    }

    private async Task RedirectToEdit(ICommentRepository commentRepository)
    {
        //UpdateCommentsView updateComments = new UpdateCommentsView(_commentRepository);
        //await updateComments.StartAsync();
    }

    private async Task RedirectToDelete(ICommentRepository commentRepository)
    {
        //RemoveCommentsView removeComments = new RemoveCommentsView(_commentRepository);
        //await removeComments.StartAsync();
    }

    private async Task RedirectToViewAllComments(ICommentRepository commentRepository)
    {
        //ListCommentsView listComments = new ListCommentsView(_commentRepository);
        //await listComments.StartAsync();
    }
}