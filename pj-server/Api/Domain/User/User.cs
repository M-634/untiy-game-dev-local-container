using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.User;

public class User : BaseEntity
{
    public ulong UserId { get; set; }
    public string UserName { get; set; } =  string.Empty;

    public List<UserItem> UserItems { get; } = new();
}