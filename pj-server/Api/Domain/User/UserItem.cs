using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.User;

public class UserItem : BaseEntity
{
    public ulong UserId { get; set; }
    public int ItemId { get; set; }
    public int PossessCount { get; set; }

    public Item.Item Item { get; set; } = null!;
}
