using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.Item;

public class ItemLocalize : BaseEntity
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
}