public enum RarityType
{
    Common, // コモン：７０％
    Rare, // レア：２０％
    Epic, // エピック：６％
    Legendary, // レジェンダリー：３％
    Mythic, // ミシック：１％
}

// レアリティによっての確率を取得
// 例：RarityType.Epic.GetProbability(); // 6.0
public static class RarityTypeExtensions
{
    public static double GetProbability(this RarityType rarity)
    {
        return rarity switch
        {
            RarityType.Common => 70.0,
            RarityType.Rare => 20.0,
            RarityType.Epic => 6.0,
            RarityType.Legendary => 3.0,
            RarityType.Mythic => 1.0,
            _ => 0.0
        };
    }
}
