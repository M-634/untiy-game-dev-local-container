using pj_server.Api.Domain.Gacha;

namespace pj_server.Api.bin;

public static class Utility
{
    public static GachaContent WeightRandomChoose(IReadOnlyList<GachaContent> items)
    {
        float totalWeight = items.Sum(x => x.LotteryRatio);
        float randValue = (float)Random.Shared.NextDouble() * totalWeight;

        foreach (var item in items)
        {
            randValue -= item.LotteryRatio;
            if (randValue < 0)
            {
                return item; // 当選アイテム
            }
        }
        // 理論上ここには来ないはず（万が一のために最後のアイテムを返すなど）
        return items.Last();
    }
}