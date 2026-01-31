using pj_server.Api;
using pj_server.Api.Domain.User;
using pj_server.Api.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pj_server.Api.bin;
using pj_server.Api.Domain.Gacha;

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

//NOTE:基本的に「dotnet ef database update」 でマイグレーションファイルを更新する
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDb>();
//     db.Database.Migrate();
// }

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

app.MapPost("gacha/execute/", (IServiceProvider provider, [FromBody] RequestGachaExecute request) =>
{
    var db = provider.GetService<AppDb>()!;
    var targetGachaTable = db.Gachas.FirstOrDefault(x => x.GachaId == request.GachaId);
    if (targetGachaTable == null)
    {
        throw new Exception("Gacha not found");
    }
    
    //ガチャ抽選
    var targetGachaContents = db.GachaContents
        .Where(x => x.GachaContentGroupId == targetGachaTable.GachaContentGroupId)
        .OrderBy(x => x.LotteryRatio)
        .ToList();

    int executeCount = request.ExecuteCount;
    var result = new List<int>(executeCount);
    var itemTable = db.Items;

    for (int i = 0; i < executeCount; i++)
    {
        var temp = Utility.WeightRandomChoose(targetGachaContents);
        var getItem = itemTable.FirstOrDefault(x => x.ItemId == temp.ItemId)!;
        result.Add(getItem.ItemId); 
    }
    
    return Results.Ok(new ResponseGachaExecute
    {
        ItemIds = result.ToArray()
    });
});


app.Run();