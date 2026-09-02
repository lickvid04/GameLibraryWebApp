using GameLibrary;
using GameLibrary.DTOs;
using GameLibrary.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddCors(); 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}



app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());




// Эндпоинты для user

var userGroup = app.MapGroup("/user");

userGroup.MapGet("/", async (AppDbContext db) =>
{
    var allUsers = await db.User.ToListAsync();
    return allUsers;
});

userGroup.MapGet("/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.User.FirstOrDefaultAsync(u => u.User_ID == id);

    return user is null ? Results.NotFound("Пользователь не найден") : Results.Ok(user);
});

userGroup.MapPost("/register", async (UserRegistrationDto userDto, AppDbContext db) =>
{
    var user = new User
    {
        Nickname = userDto.Nickname,
        Mail = userDto.Mail
    };
    user.SetPassword(userDto.Password);
    db.User.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok("Пользователь зарегистрирован");
});

userGroup.MapPost("/login", async (UserLoginDto userDto, AppDbContext db) =>
{
    var user = await db.User.FirstOrDefaultAsync(u => u.Mail == userDto.Mail && u.VerifyPassword(userDto.Password));
    return user is null ? Results.Unauthorized() : Results.Ok(user);
});



// Эндпоинты для game
var gameGroup = app.MapGroup("/gamelibrary");

gameGroup.MapGet("/", async (AppDbContext db) =>
{
    var allGames = await db.Games.ToListAsync();
    return allGames;
});
gameGroup.MapGet("/{id}", async (int id, AppDbContext db) =>
{
    var game = await db.Games.FirstOrDefaultAsync(g => g.Game_ID == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});

gameGroup.MapPost("/", async (Games game, AppDbContext db) =>
{
    db.Games.Add(game);
    await db.SaveChangesAsync();
    return Results.Ok("Игра добавлена");
});


app.UseDefaultFiles(); 
app.UseStaticFiles(); 
app.MapFallbackToFile("index.html");

app.Run();
