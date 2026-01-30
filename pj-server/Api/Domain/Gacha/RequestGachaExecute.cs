namespace pj_server.Api.Domain.Gacha;

[Serializable]
public class RequestGachaExecute
{
    public int GachaId { get; set; }
    public int ExecuteCount { get; set; }
}