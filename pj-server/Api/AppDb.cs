using Api.Domain.Common;
using Api.Domain.Item;
using Api.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class AppDb : DbContext
{
    public DbSet<User> UUSer => Set<User>();
    public DbSet<UserItem> UserItem => Set<UserItem>();
    
    public DbSet<Item> MItems => Set<Item>();
    public DbSet<ItemLocalize> MItemLocalizes => Set<ItemLocalize>();
    
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // tables
        b.Entity<Item>().ToTable("m_item").HasKey(x => x.ItemId);
        b.Entity<ItemLocalize>().ToTable("m_item_localize").HasKey(x => x.ItemId);
        b.Entity<User>().ToTable("u_user").HasKey(x => x.Id);
        b.Entity<UserItem>().ToTable("u_user_item").HasKey(x => new { x.UserId, x.ItemId });

        // props
        b.Entity<ItemLocalize>().Property(x => x.ItemName).HasMaxLength(Config.MaxLength128Letters).IsRequired();
        b.Entity<User>().Property(x => x.Name).HasMaxLength(Config.MaxLength64Letters).IsRequired();

        // FKs
        b.Entity<ItemLocalize>()
            .HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<UserItem>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<UserItem>()
            .HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // dotnet ef 実行時に DI が効かなくても自力でプロバイダを構成する
        if (!optionsBuilder.IsConfigured)
        {
            // 環境変数で上書き可能。未設定なら localhost へ。
            var cs = Environment.GetEnvironmentVariable("EF_CONNECTION") ?? "Server=localhost;Port=3306;Database=game;User=app;Password=appsecret;";
            optionsBuilder.UseMySql(cs, Config.MySqlServerVersion);
        }
    }
    
}
