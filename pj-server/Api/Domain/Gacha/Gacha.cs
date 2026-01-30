using pj_server.Api.Domain.Common;

namespace pj_server.Api.Domain.Gacha;

public class Gacha : BaseEntity
{
    public int GachaId { get; set; }
    public int GachaContentGroupId { get; set; }
    public int GachaButtonGroupId { get; set; }
    public DateTime OpenAt { get; set; }
    public DateTime CloseAt { get; set; }
}
