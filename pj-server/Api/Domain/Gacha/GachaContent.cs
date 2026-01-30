using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.Gacha;

public class GachaContent : BaseEntity
{
    public int Id { get; set; }
    public int GachaContentGroupId { get; set; }
    public int ItemId { get; set; }
    public float LotteryRatio { get; set; }

    public Item.Item Item { get; set; } = null!;
}