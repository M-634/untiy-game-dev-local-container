using MasterMemory;
using MessagePack;

namespace pj_master.Schemas;

[MemoryTable("m_item"), MessagePackObject(true)]
public partial class Item : IMasterDefine
{
    [PrimaryKey]
    public required int ItemId { get; init; }
    
    [SecondaryKey(0), NonUnique]
    public required int ItemGroupId { get; init; }
    
    public required int Rarity { get; init; }
    
    public required int MaxPossessCount { get; init; }
}

[MemoryTable("m_item_group"), MessagePackObject(true)]
public partial class ItemGroup : IMasterDefine
{
    [PrimaryKey]
    public required int ItemGroupId { get; init; }
    
    public required string ItemGroupName { get; init; }
}

[MemoryTable("m_item_localize"), MessagePackObject(true)]
public partial class ItemLocalize : IMasterDefine
{
    [PrimaryKey]   
    public required int ItemId { get; init; }
    
    public required string ItemName { get; init; } 
}
