using Entities;
using RepositoryContracts;

namespace InMemoryRepository;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new();

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(p => p.Id) + 1 : 1;
        users.Add(user);
        
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(p => p.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' was not found.");
        }
        
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToBeDeleted = users.SingleOrDefault(p => p.Id == id);
        if (userToBeDeleted is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' was not found.");
        }
        
        users.Remove(userToBeDeleted);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToGet = users.SingleOrDefault(p => p.Id == id);
        if (userToGet is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' was not found.");
        }
        
        return Task.FromResult(userToGet);
    }

    public IQueryable<User> GetManyAsync()
    {
        return  users.AsQueryable();
    }
}