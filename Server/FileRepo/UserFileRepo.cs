using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepo;

public class UserFileRepo : IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepo()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await LoadUsers();
        int maxId = users.Count > 0 ? users.Max(x => x.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        await SaveUsers(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await LoadUsers();
        User?  existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        { 
            throw new InvalidOperationException($"User with ID '{user.Id}' was not found.");
        }
        
        users.Remove(existingUser);
        users.Add(user);
        await SaveUsers(users);
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await LoadUsers();
        User? existingUser = users.SingleOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' was not found.");
        }
        users.Remove(existingUser);
        await SaveUsers(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await LoadUsers();
        User? existingUser = users.SingleOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' was not found.");
        }
        
        return existingUser;
    }

    public IQueryable<User> GetManyAsync()
    {
        return LoadUsers().Result.AsQueryable();
    }

    private async Task<List<User>> LoadUsers()
    {
        string  usersAsJson = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
    }

    private async Task SaveUsers(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }
}