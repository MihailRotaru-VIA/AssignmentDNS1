using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private IUserRepository _userRepository;

    public ManageUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[1] Create new user");
            Console.WriteLine("[2] Update an existing user");
            Console.WriteLine("[3] Delete an existing user");
            Console.WriteLine("[4] View all users");
            Console.Write("[5] X Exit\n> ");

            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Please enter a VALID choice!");
                continue;
            }

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
                    await RedirectToList();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Please enter a VALID choice!");
                    break;
            }
            
            Console.WriteLine();
        }
    }

    private async Task RedirectToCreate()
    {
        CreateUsersView createUsersView = new CreateUsersView(_userRepository);
        await createUsersView.StartAsync();
    }

    private async Task RedirectToUpdate()
    {
        //UpdateUsersView updateUsersView = new UpdateUsersView(_userRepository);
        //await updateUsersView.StartAsync();
    }

    private async Task RedirectToDelete()
    {
        DeleteUsersView deleteUsersView = new DeleteUsersView(_userRepository);
        await deleteUsersView.StartAsync();
    }

    private async Task RedirectToList()
    {
        ListUsersView listUsersView = new ListUsersView(_userRepository);
        await listUsersView.StartAsync();
    }
}