using Api.Domain.Common;

namespace Api.Domain.User;

public class User : BaseEntity
{
    public ulong UserId { get; set; }
    public string UserName { get; set; } =  string.Empty;

    public List<UserItem> UserItems { get; } = new();
}