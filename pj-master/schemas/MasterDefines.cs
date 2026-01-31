using MasterMemory;
using MessagePack;
using System;

//Unity上だと'required' 修飾子を使用できないので警告を無視する
#pragma warning disable CS8618

namespace MasterDefine
{
    [MemoryTable("m_item_group"), MessagePackObject(true)]
    public class ItemGroup : IMasterDefine
    {
        [PrimaryKey] 
        public int ItemGroupId { get; init; }

        public string ItemGroupName { get; init; }
    }

    [MemoryTable("m_item"), MessagePackObject(true)]
    public class Item : IMasterDefine
    {
        [PrimaryKey] public int ItemId { get; init; }

        [SecondaryKey(0), NonUnique]
        public int ItemGroupId { get; init; }

        public int Rarity { get; init; }

        public int MaxPossessCount { get; init; }
    }


    [MemoryTable("m_item_localize"), MessagePackObject(true)]
    public class ItemLocalize : IMasterDefine
    {
        [PrimaryKey] 
        public int ItemId { get; init; }

        public string ItemName { get; init; }
    }


    //=== Sample Gacha Masters ===  

    [MemoryTable("m_gacha"), MessagePackObject(true)]
    public class Gacha : IMasterDefine
    {
        [PrimaryKey]
        public int GachaId { get; init; }
        public int GachaContentGroupId { get; init; }
        public int GachaButtonGroupId { get; init; }
        public DateTime OpenAt { get; set; }
        public DateTime CloseAt { get; set; }
    }


    [MemoryTable("m_gacha_content"), MessagePackObject(true)]
    public class GachaContent : IMasterDefine
    {
        [PrimaryKey] public int Id { get; init; }

        [SecondaryKey(0), NonUnique]
        public int GachaContentGroupId { get; init; }
        public int ItemId { get; init; }
        public float LotteryRatio { get; init; }
    }

    [MemoryTable("m_gacha_lottery_button"), MessagePackObject(true)]
    public class GachaLotteryButton : IMasterDefine
    {
        [PrimaryKey] public int Id { get; init; }

        [SecondaryKey(0), NonUnique]
        public int GachaButtonGroupId { get; init; }
        public int GachaExecuteId { get; init; }
    }
}