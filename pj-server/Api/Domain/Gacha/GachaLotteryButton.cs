using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.Gacha;

public class GachaLotteryButton : BaseEntity
{
    public int Id { get; set; }
    public int GachaButtonGroupId { get; set; }
    public int GachaExecuteId { get; set; }
}