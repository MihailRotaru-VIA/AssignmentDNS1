using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UpdateUsersView
{
    private IUserRepository _userRepository;

    public UpdateUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select which user you want to delete:");
            List<User> users = _userRepository.GetManyAsync().ToList();
            Console.WriteLine("[ID]    [Username]");
            foreach (User user in users)
            {
                Console.WriteLine($"[{user.Id}]    {user.Username}");
            }
            Console.WriteLine("[0] X Exit");
            Console.Write("> ");
            string? input = Console.ReadLine();
            
            // will do later 
        }
    }
}