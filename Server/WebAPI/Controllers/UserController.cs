using DTOs.Comment;
using DTOs.User;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using RepositoryContracts;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IResult> AddUser([FromBody] CreateUserDto request)
    {
        User user = new(request.Username, request.Password);
        User created = await _userRepository.AddAsync(user);
        
        return Results.Created($"/user/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateUser([FromBody] UpdateUserDto request, [FromRoute] int id)
    {
        User userToUpdate = await _userRepository.GetSingleAsync(id);
        if(userToUpdate is null) return Results.NotFound(); 
        
        userToUpdate.Username = request.Username;
        userToUpdate.Password = request.Password;
        await _userRepository.UpdateAsync(userToUpdate);
        return Results.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteUser([FromRoute] int id)
    {
        User userToDelete = await _userRepository.GetSingleAsync(id);
        if (userToDelete is null) return Results.NotFound();

        await _userRepository.DeleteAsync(id);
        return Results.NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleUser([FromRoute] int id)
    {
        User userToGet = await _userRepository.GetSingleAsync(id);
        if(userToGet is null) return Results.NotFound();
        
        GetSingleUserDto dto = new();
        dto.Id = userToGet.Id;
        dto.Username = userToGet.Username;
        
        return Results.Ok(dto);
    }

    [HttpGet("/users")]
    public async Task<IResult> GetManyUsers([FromQuery] string? username = null)
    {
        List<User> users = _userRepository.GetManyAsync().ToList();
        List<GetManyUsersDto> dtos = new();
        if (username is null)
        {
            foreach (User user in users)
            {
                GetManyUsersDto dto = new();
                dto.Id = user.Id;
                dto.Username = user.Username;
                dtos.Add(dto);
            }

            return Results.Ok(dtos);
        }
        
        foreach (User user in users)
        {
            if (user.Username.Contains(username))
            {
                GetManyUsersDto dto = new();
                dto.Id = user.Id;
                dto.Username = user.Username;
                dtos.Add(dto);
            }
        }

        return Results.Ok(dtos);
    }
}