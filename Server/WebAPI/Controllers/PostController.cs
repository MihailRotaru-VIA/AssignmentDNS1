using DTOs.Post;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<IResult> AddPost([FromBody] CreatePostDto request)
    {
        Post post = new(request.Title, request.Body, request.UserId);
        Post created = await _postRepository.AddAsync(post);

        return Results.Created($"/post/{created.Id}", created);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdatePost([FromBody] UpdatePostDto request, [FromRoute] int id)
    {
        Post postToUpdate = await _postRepository.GetSingleAsync(id);
        if (postToUpdate is null) return Results.NotFound();

        postToUpdate.Title = request.Title;
        postToUpdate.Body = request.Body;
        await _postRepository.UpdateAsync(postToUpdate);
        
        return Results.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        Post postToDelete = await _postRepository.GetSingleAsync(id);
        if (postToDelete is null) return Results.NotFound();

        await _postRepository.DeleteAsync(id);
        return Results.NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSinglePost([FromRoute] int id)
    {
        Post postToGet = await _postRepository.GetSingleAsync(id);
        if (postToGet is null) return Results.NotFound();

        GetSinglePostDto dto = new();
        dto.Id = postToGet.Id;
        dto.Title = postToGet.Title;
        dto.Body = postToGet.Body;
        dto.UserId = postToGet.UserId;
        return Results.Ok(dto);
    }

    [HttpGet]
    public async Task<IResult> GetManyPosts([FromQuery] string? title = null)
    {
        List<Post> posts = _postRepository.GetManyAsync().ToList();
        List<GetManyPostsDto> dtos = new();

        if (title is null)
        {
            foreach (Post post in posts)
            {
                GetManyPostsDto dto = new();
                dto.Id = post.Id;
                dto.Title = post.Title;
                dto.Body = post.Body;
                dto.UserId = post.UserId;
                dtos.Add(dto);
            }

            return Results.Ok(dtos);
        }

        foreach (Post post in posts)
        {
            if (title == post.Title)
            {
                GetManyPostsDto dto = new();
                dto.Id = post.Id;
                dto.Title = post.Title;
                dto.Body = post.Body;
                dto.UserId = post.UserId;
                dtos.Add(dto);
            }
        }

        return Results.Ok(dtos);
    }
    
}