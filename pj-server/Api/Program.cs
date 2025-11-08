using Api;
using Api.Infrastructure.Db;
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


app.Run();