using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUsersView
{
    private string _username;
    private string _password;
    private readonly IUserRepository _userRepository;

    public CreateUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            do
            {
                Console.Write("Please enter your username:\n> ");
                _username = Console.ReadLine()?.Trim();
                if (_username?.Length < 3)
                {
                    Console.WriteLine("Invalid username -- Username needs to be at least 3 characters");
                }
                
                List<User> users = _userRepository.GetManyAsync().ToList();
                foreach (User user in users)
                {
                    if (_username == user.Username)
                    {
                        Console.WriteLine("User already exists");
                        _username = string.Empty;
                    }
                }
            } while (_username is null || _username.Length < 3);
                
            do
            {
                Console.Write("Please enter your password:\n> ");
                _password = Console.ReadLine()?.Trim();
                if (_password?.Length < 1)
                {
                    Console.WriteLine("Invalid password -- Password needs to be at least 1 character");
                }
            } while (_password is null  || _password.Length < 1);
            
            while (true)
            {
                Console.WriteLine(
                    $"\nYou want to create a user with {_username} as an username.");
                Console.Write("Is this correct? (y/n): ");
                string? input = Console.ReadLine()?.Trim();
                if (input is null)
                {
                    Console.WriteLine("HUH?!");
                    continue;
                }
                
                if (string.Equals("y", input) || string.Equals("Y", input))
                {
                    User tempUser = new User(_username, _password); 
                    try
                    {
                        await _userRepository.AddAsync(tempUser);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    Console.WriteLine($"{_username} created successfully! ID: {tempUser.Id}");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey(true); 
                    return;
                }

                if (string.Equals("n", input) || string.Equals("N", input))
                {
                    break;
                }

               
            }
        }
    }
}