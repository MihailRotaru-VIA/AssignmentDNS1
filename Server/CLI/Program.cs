using CLI.UI;
using InMemoryRepository;
using RepositoryContracts;

Console.WriteLine("Starting the CLI, please wait...\n");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

ClientApp clientApp = new ClientApp(userRepository, commentRepository, postRepository);
await clientApp.StartAsync();