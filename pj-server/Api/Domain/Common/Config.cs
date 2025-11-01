using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Common;

public static class Config
{
    public const int MaxLength64Letters = 64;
    public const int MaxLength128Letters = 128;
    public static readonly MySqlServerVersion MySqlServerVersion = new(new Version(8, 4, 0));
}