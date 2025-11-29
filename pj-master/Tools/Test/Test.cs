using pj_master.Schemas;

namespace pj_master.Tools.Test;

public class Test
{
    internal static void LoadDatabase(string rootPath)
    {
        var filePath = Path.Combine(rootPath, "master.bytes");
        var bin = File.ReadAllBytes(filePath);
        
        // MasterMemory の DB 復元
        var db = new MemoryDatabase(bin);

        // 例: 全アイテム列挙
        foreach (var x in db.ItemTable.All)
        {
            Console.WriteLine($"{x.ItemId}: group={x.ItemGroupId}, rarity={x.Rarity}");
        }
        
        foreach (var x in db.ItemGroupTable.All)
        {
            Console.WriteLine($"{x.ItemGroupId}: {x.ItemGroupName}");
        }
        
        foreach (var x in db.ItemLocalizeTable.All)
        {
            Console.WriteLine($"{x.ItemId}: {x.ItemName}");
        }
    }
}