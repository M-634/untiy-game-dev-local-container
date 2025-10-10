var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB 接続
var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDb>(o =>
    o.UseMySql(cs, ServerVersion.AutoDetect(cs)));

var app = builder.Build();

// Swagger (Development のとき有効)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ルーティング
app.MapGet("/health", () => Results.Ok(new { ok = true }));

app.MapGet("/api/items", async (AppDb db) =>
{
    var items = await db.Items.OrderBy(i => i.id).ToListAsync();
    return Results.Ok(items);
});

app.Run();

// --- 以下は DbContext/Entity 定義 ---
public class AppDb: DbContext
{
    public AppDb(DbContextOptions<AppDb> opt) : base(opt) {}
    public DbSet<Item> Items => Set<Item>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserItem> UserItems => Set<UserItem>();
}

public class Item { public int id {get;set;} public string code {get;set;} = ""; public string name {get;set;} = ""; public byte rarity {get;set;} }
public class User { public long id {get;set;} public string user_name {get;set;} = ""; public string password_hash {get;set;} = ""; }
public class UserItem { public long id {get;set;} public long user_id {get;set;} public string item_code {get;set;} = ""; public int count {get;set;} }
