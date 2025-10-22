using FileRepo;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPostRepository, PostFileRepo>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepo>();
builder.Services.AddScoped<IUserRepository, UserFileRepo>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();
