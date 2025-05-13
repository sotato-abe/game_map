public enum EnchantType
{
    // バフ
    Increase,       // 増強 (ATKが1アップする。)
    Firm,           // 堅固 (DEFが１アップする。)
    Adrenalin,      // アドレナリン（ATK・SPDを１上げる）
    Curing,         // 硬化（DEFを１上げ、SPDを１下げる）
    Acceleration,   // 加速（SPDを１上げる）
    Immunity,       // 免疫（毒をのデバフを受け付けない）
    Lucky,          // 幸運（LUKを１上げる）
    Clear,          // クリア（デバフをランダムで１つ解除する）
    SmokeScreen,    // 煙幕 (自身へ全てのアタックの当たり判定を25％下げる)
    Splinter,       // トゲ (アタックを受ける時、攻撃者のLIFEに5のダメージを与える)
    Reflection,     // 反射 (アタックを受ける時、攻撃者のBTRYに5のダメージを与える)
    Gaze,           // 凝視 (ターゲットの弱点・ステータスを表示する)
    PoisonHand,     // 毒手 (アタック時ターゲットに毒を付与する)

    // デバフ
    Hurt,           // 怪我 (ターン開始時にLIFEを１失う)
    Poison,         // 毒 (ターン開始時にLIFEを１失う)
    Leakage,        // 漏電（ターン開始時にBTRYを１失う）
    Paralysis,      // 混乱（ターゲット指定がランダムになる（自分も含まれる））
    Restraint,      // 拘束（SPDを１下げる）
    Bug,            // バグ（コマンド実行時にLIFEを１失う）
    Atrophy,        // 萎縮 ATKを1ダウンする。
    Fatigue,        // 疲労 DEFを1ダウンする。
    UnLuckey,       // 不運 LUKを1ダウンする。
    Crack,          // 亀裂 ガードを1ダウンする。
    Cipher,         // 暗号 コストを5％アップする。
    Lock,           // ロック ランダムでコマンドが１枚実行不能になる。
    SoftBody,       // 軟体 アタック系のコマンドが使用できなくなる。
    MeatHead,       // 脳筋 アタック系のコマンドしか使用できなくなる。

    // 両方
    HellFire,       // 業火 (ターン開始時にLIFEを１失う。物理攻撃を受けたときに攻撃者に火傷を付与する。)
    Indignation,    // 憤怒 (ATKを20%アップさせ、確率を30％下げる（下限は5%）)
    Petrifaction,   // 石化 (ダメージを受けない、そのかわり行動で。)
    Sleep,          // 睡眠 (1ターン行動できないが、ターン終了後にLIFEを5％回復する。攻撃されると解除される。)
}