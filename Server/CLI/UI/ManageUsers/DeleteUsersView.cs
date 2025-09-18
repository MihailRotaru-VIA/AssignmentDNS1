using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class DeleteUsersView
{
    private IUserRepository _userRepository;

    public DeleteUsersView(IUserRepository userRepository)
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
            bool found = false;
            bool cancelled = false;
            bool wrongPass = false;
            if (int.TryParse(input, out int id))
            {
                foreach (User user in users)
                {
                    if (id == 0)
                    {
                        return;
                    }
                    
                    if (id == user.Id)
                    {
                        found = true; 
                        
                        Console.WriteLine("Are you sure you want to delete this user? (y/n)");
                        Console.Write("> ");
                        string? input2 = Console.ReadLine();
                        if (input2 == "y" || input2 == "Y")
                        {
                            Console.WriteLine("Please enter the password");
                            Console.Write("> ");
                            string? password = Console.ReadLine()?.Trim();
                            if (password == user.Password)
                            {
                                await _userRepository.DeleteAsync(id);
                                Console.WriteLine("User deleted successfully!");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey(true);
                                return;
                            }

                            Console.WriteLine("Invalid password!");
                            wrongPass = true;
                            break;
                        }

                        cancelled = true;
                        break;
                    }
                }
                
                if (!found)
                {
                    Console.WriteLine($"The user with the id: {id} doesn't exist");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    continue;
                }

                if (cancelled || wrongPass)
                {
                    continue;
                }
            }
        }
    }
}