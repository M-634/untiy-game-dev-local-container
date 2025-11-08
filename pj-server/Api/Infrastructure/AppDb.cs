using Api.Domain.Item;
using Api.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Db;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }
    

    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemGroup> ItemGroups => Set<ItemGroup>();
    public DbSet<ItemLocalize> ItemLocalizes => Set<ItemLocalize>();
    public DbSet<User> USers => Set<User>();
    public DbSet<UserItem> UserItems => Set<UserItem>();


    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        
        // tables
        b.Entity<Item>().ToTable("m_item").HasKey(x => x.ItemId);
        b.Entity<ItemGroup>().ToTable("m_item_group").HasKey(x => x.ItemGroupId);
        b.Entity<ItemLocalize>().ToTable("m_item_localize").HasKey(x => x.ItemId);
        b.Entity<User>().ToTable("u_user").HasKey(x => x.UserId);
        b.Entity<UserItem>().ToTable("u_user_item").HasKey(x => new { x.UserId, x.ItemId });

        // props
        b.Entity<ItemGroup>().Property(x => x.ItemGroupName).HasMaxLength(Config.MaxLength64Letters).IsRequired();
        b.Entity<ItemLocalize>().Property(x => x.ItemName).HasMaxLength(Config.MaxLength128Letters).IsRequired();
        b.Entity<User>().Property(x => x.UserName).HasMaxLength(Config.MaxLength64Letters).IsRequired();
        
        // set indexes
        b.Entity<Item>().HasIndex(i => i.ItemGroupId);
        b.Entity<UserItem>().HasIndex(ui => ui.UserId);
        b.Entity<UserItem>().HasIndex(ui => ui.ItemId);
        
        // time stamp
        b.Entity<Item>().Property(x => x.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
        b.Entity<Item>().Property(x => x.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();;
        
        b.Entity<ItemGroup>().Property(x => x.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.Entity<ItemGroup>().Property(x => x.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        
        b.Entity<ItemLocalize>().Property(x => x.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.Entity<ItemLocalize>().Property(x => x.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        
        b.Entity<User>().Property(x => x.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.Entity<User>().Property(x => x.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        
        b.Entity<UserItem>().Property(x => x.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.Entity<UserItem>().Property(x => x.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        

        // fks and navigations
        b.Entity<Item>().HasOne(i => i.ItemGroup).WithMany().HasForeignKey(x => x.ItemGroupId)
            .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_m_item__item_group_id");
     
        b.Entity<Item>().HasOne(i => i.ItemLocalize).WithOne().HasForeignKey<ItemLocalize>(x => x.ItemId)
            .OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_m_item_localize__item_id");
        
        b.Entity<User>().HasMany(u => u.UserItems).WithOne().HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_u_user_item__user_id");
        
        b.Entity<UserItem>().HasOne(x => x.Item).WithMany().HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_u_user_item__item_id");

        
        // set seed
        var testItemGroups = new List<ItemGroup>
        {
            new() { ItemGroupId = 1, ItemGroupName = "回復薬" },
            new() { ItemGroupId = 2, ItemGroupName = "武器" }
        };
        
        b.Entity<ItemGroup>().HasData(testItemGroups);
        
        var testItems = new List<Item>
        {
            new() { ItemId = 1, ItemGroupId = 1, Rarity = 1, MaxPossessCount = 99 },
            new() { ItemId = 2, ItemGroupId = 1, Rarity = 2, MaxPossessCount = 99 },
            new() { ItemId = 3, ItemGroupId = 2, Rarity = 5, MaxPossessCount = 99 }
        };
        
        b.Entity<Item>().HasData(testItems);
        
        var testItemLocalize = new List<ItemLocalize>
        {
            new() { ItemId = 1, ItemName = "アイテムテスト1" },
            new() { ItemId = 2, ItemName = "アイテムテスト2" },
            new() { ItemId = 3, ItemName = "アイテムテスト3" }
        };

        b.Entity<ItemLocalize>().HasData(testItemLocalize);
    }
}