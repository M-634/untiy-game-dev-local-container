using pj_server.Api;
using pj_server.Api.Domain.User;
using pj_server.Api.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Default") ??
         Environment.GetEnvironmentVariable("EF_CONNECTION") ??
         "Server=db;Port=3306;Database=game;User=app;Password=appsecret;";

builder.Services.AddDbContext<AppDb>(opt => opt.UseMySql(cs, Config.MySqlServerVersion));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.Migrate();
}

//NOTE: DockerコンテナではHttps証明書を用意していないのでリダイレクトしない
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// rooting api by minimal api
app.MapGet("/health", () => Results.Ok(new { ok = true }));
app.MapGet("/test", () => Results.Ok(new { ok = "test" }));
app.MapGet("/test/items", async (AppDb db) =>
{
    var items = await db.Items.Include(i => i.ItemLocalize).ToListAsync();
    return Results.Ok(items);
});

app.MapPost("create/user/", async (IServiceProvider provider, [FromBody] CreateUserFormModel value) =>
{
    var db = provider.GetService<AppDb>()!;
    var users = db.USers;
    var lastId = users.OrderBy(user => user.UserId).LastOrDefault()?.UserId ?? 0;
    
    var addUser = new User
    {
        UserId = lastId + 1u,
        UserName = value?.Name ?? string.Empty,
    };
    
    users.Add(addUser);
    await db.SaveChangesAsync();
   
    return Results.Ok(new {value});
});

// app.MapPost("delete/user/{id?}", async (IServiceProvider provider, ulong? id) =>
// {
//     await using var db = provider.GetService<AppDb>()!;
//     var users = await db.USers.ToListAsync();
//     
// });


app.Run();