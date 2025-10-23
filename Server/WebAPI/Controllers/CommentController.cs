using DTOs.Comment;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public CommentController(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IResult> AddComment([FromBody] CreateCommentDto request)
    {
        Comment comment = new(request.Body, request.PostId, request.UserId);
        Comment created = await _commentRepository.AddAsync(comment);

        return Results.Created($"comment/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateComment([FromBody] UpdateCommentDto request, [FromRoute] int id)
    {
        Comment commentToUpdate = await _commentRepository.GetSingleAsync(id);
        if (commentToUpdate is null) return Results.NotFound();

        commentToUpdate.Body = request.Body;
        await _commentRepository.UpdateAsync(commentToUpdate);
        return Results.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        Comment commentToDelete = await _commentRepository.GetSingleAsync(id);
        if (commentToDelete is null) return Results.NotFound();

        await _commentRepository.DeleteAsync(id);
        return Results.NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleComment([FromRoute] int id)
    {
        Comment commentToGet = await _commentRepository.GetSingleAsync(id);
        if (commentToGet is null) Results.NotFound();

        GetSingleCommentDto dto = new();
        dto.Id = commentToGet.Id;
        dto.Body = commentToGet.Body;
        dto.UserId = commentToGet.UserId;
        dto.PostId = commentToGet.PostId;
        
        return Results.Ok(dto);
    }

    [HttpGet("/comments")]
    public async Task<IResult> GetManyComments()
    {
        List<Comment> comments = _commentRepository.GetManyAsync().ToList();
        List<GetManyCommentsDto> dtos = new();

        foreach (Comment comment in comments)
        {
                GetManyCommentsDto dto = new();
                dto.Id = comment.Id;
                dto.Body = comment.Body;
                dto.UserId = comment.UserId;
                dto.PostId = comment.PostId;
                dtos.Add(dto);
        }

        return Results.Ok(dtos);
    }

    [HttpGet("/user/{userId:int}/comments")]
    public async Task<IResult> GetManyCommentsByUser([FromRoute] int userId, [FromQuery] int? postId = null)
    {
        List<Comment> comments = _commentRepository.GetManyAsync().ToList();
        List<GetManyCommentsDto> dtos = new();

        if (postId is not null)
        {
            foreach (Comment comment in comments)
            {
                if (comment.UserId == userId && comment.PostId == postId)
                {
                    GetManyCommentsDto dto = new();
                    dto.Id = comment.Id;
                    dto.Body = comment.Body;
                    dto.UserId = comment.UserId;
                    dto.PostId = comment.PostId;
                    dtos.Add(dto);
                }
            }

            return Results.Ok(dtos);
        }

        foreach (Comment comment in comments)
        {
            if (comment.UserId == userId)
            {
                GetManyCommentsDto dto = new();
                dto.Id = comment.Id;
                dto.Body = comment.Body;
                dto.UserId = comment.UserId;
                dto.PostId = comment.PostId;
                dtos.Add(dto);
            }
        }

        return Results.Ok(dtos);
    }

    [HttpGet("/post/{postId:int}/comments")]
    public async Task<IResult> GetManyCommentsByPostId([FromRoute] int postId, [FromQuery] string? username = null)
    {
        List<User> users = _userRepository.GetManyAsync().ToList();
        List<Comment> comments = _commentRepository.GetManyAsync().ToList();
        List<GetManyCommentsDto> dtos = new();

        if (username is not null)
        {
            foreach (Comment comment in comments)
            {
                foreach (User user in users)
                {
                    if (comment.PostId == postId && user.Username.Contains(username) && comment.UserId == user.Id)
                    {
                        GetManyCommentsDto dto = new();
                        dto.Id = comment.Id;
                        dto.Body = comment.Body;
                        dto.UserId = comment.UserId;
                        dto.PostId = comment.PostId;
                        dtos.Add(dto);
                    }
                }
            }

            return Results.Ok(dtos);
        }
        
        foreach (Comment comment in comments)
        {
            if (comment.PostId == postId)
            {
                GetManyCommentsDto dto = new();
                dto.Id = comment.Id;
                dto.Body = comment.Body;
                dto.UserId = comment.UserId;
                dto.PostId = comment.PostId;
                dtos.Add(dto);
            }
        }

        return Results.Ok(dtos);
    }
}