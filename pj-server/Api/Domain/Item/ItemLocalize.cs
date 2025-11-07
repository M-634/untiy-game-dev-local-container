using Api.Domain.Common;

namespace Api.Domain.Item;

public class ItemLocalize : BaseEntity
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
}