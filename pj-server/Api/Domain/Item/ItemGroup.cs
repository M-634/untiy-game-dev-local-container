using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.Item;

public class ItemGroup : BaseEntity
{
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; }= string.Empty;
}