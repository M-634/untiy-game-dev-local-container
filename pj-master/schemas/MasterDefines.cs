using MasterMemory;
using MessagePack;

namespace pj_master.Schemas;

[MemoryTable("m_item_group"), MessagePackObject(true)]
public class ItemGroup : IMasterDefine
{
    [PrimaryKey]
    public required int ItemGroupId { get; init; }
    
    public required string ItemGroupName { get; init; }
}

[MemoryTable("m_item"), MessagePackObject(true)]
public class Item : IMasterDefine
{
    [PrimaryKey]
    public required int ItemId { get; init; }
    
    [SecondaryKey(0), NonUnique]
    public required int ItemGroupId { get; init; }
    
    public required int Rarity { get; init; }
    
    public required int MaxPossessCount { get; init; }
}


[MemoryTable("m_item_localize"), MessagePackObject(true)]
public class ItemLocalize : IMasterDefine
{
    [PrimaryKey]   
    public required int ItemId { get; init; }
    
    public required string ItemName { get; init; } 
}


//=== Sample Gacha Masters ===  

[MemoryTable("m_gacha"), MessagePackObject(true)]
public class Gacha : IMasterDefine
{
    [PrimaryKey]
    public required int GachaId { get; init; }
    public required int GachaContentGroupId { get; init; }
    public required int GachaButtonGroupId { get; init; }
    public DateTime OpenAt { get; set; }
    public DateTime CloseAt { get; set; }
}


[MemoryTable("m_gacha_content"), MessagePackObject(true)]
public class GachaContent : IMasterDefine
{
    [PrimaryKey]
    public required int Id { get; init; }
    
    [SecondaryKey(0), NonUnique]
    public required int GachaContentGroupId { get; init; }
    public required int ItemId { get; init; }
    public required float LotteryRatio { get; init; }
}

[MemoryTable("m_gacha_lottery_button"), MessagePackObject(true)]
public class GachaLotteryButton : IMasterDefine
{
    [PrimaryKey]
    public required int Id { get; init; }
    
    [SecondaryKey(0), NonUnique]
    public required int GachaButtonGroupId { get; init; }
    public required int GachaExecuteId { get; init; }
}










