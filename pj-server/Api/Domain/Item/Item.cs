using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.Item;

public class Item : BaseEntity
{
    public int ItemId { get; set; }
    public int ItemGroupId { get; set; }
    public int Rarity { get; set; }
    public int MaxPossessCount { get; set; }
    
    public ItemGroup ItemGroup { get; set; } = null!; 
    public ItemLocalize ItemLocalize { get; set; } = null!; 

}
