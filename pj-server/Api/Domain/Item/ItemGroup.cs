using Api.Domain.Common;

namespace Api.Domain.Item;

public class ItemGroup : BaseEntity
{
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; }= string.Empty;
}