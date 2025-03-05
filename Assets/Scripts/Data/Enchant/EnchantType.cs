public enum EnchantType
{
    Hurt, // 怪我 (毎ターンLIFEを１失う)
    Poison, // 毒 (毎ターンLIFEを２失う)
    Leakage, // 漏電（毎ターンBTRYを１失う）
    Depression, // 鬱（毎ターンSOULを１失う）
    Paralysis, // 混乱（ターゲットがランダムになる）
    AttackUp, // ATKアップ（自身のATKを１上げる）
    DefenseUp, // DEFアップ（自身のDEFを１上げる）
    SpeedUp, // SPDアップ（自身のSPDを１上げる）
    Immunity, // 免疫（毒を受け付けない）
    Clear, // ターゲットのバフ・デバフ解除
}