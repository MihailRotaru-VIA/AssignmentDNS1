using CLI.UI;
using FileRepo;
using RepositoryContracts;

Console.WriteLine("Starting the CLI, please wait...\n");
IUserRepository userRepository = new UserFileRepo();
ICommentRepository commentRepository = new CommentFileRepo();
IPostRepository postRepository = new PostFileRepo();   

ClientApp clientApp = new ClientApp(userRepository, commentRepository, postRepository);
await clientApp.StartAsync();   