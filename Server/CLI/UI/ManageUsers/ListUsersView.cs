using System.Net;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private List<User> _users;
    private IUserRepository _userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        _users = new();
        _userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            _users = _userRepository.GetManyAsync().ToList();
            Console.WriteLine("[ID]    [Username]");
            foreach (User user in _users)
            {
                Console.WriteLine($"[{user.Id}]     {user.Username}");
            }
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey(true);
            return;
        }
    }
}