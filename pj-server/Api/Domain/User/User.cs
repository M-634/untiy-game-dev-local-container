using Api.Domain.Common;

namespace Api.Domain.User;

public class User : BaseEntity
{
    public ulong Id { get; set; }
    public string Name { get; set; } =  string.Empty;
}