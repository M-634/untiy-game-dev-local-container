using Api.Domain.Common;

namespace Api.Domain.Item;

public class Item : BaseEntity
{
    public int ItemId { get; set; }
    public int ItemGroupId { get; set; }
    public int Rarity { get; set; }
    public int MaxPossessCount { get; set; }
}
