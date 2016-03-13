﻿namespace IkaStylist.Models
{
    ///<summary>部位</summary>
    public enum GearKind
    {
        [EnumLiteral("アタマ")]
        Head,
        [EnumLiteral("フク")]
        Cloth,
        [EnumLiteral("クツ")]
        Shoes
    }

    #region ギアパワー列挙型
    public enum GearPowerKind
    {
        [EnumLiteral("なし")]
        None,	//0
        [EnumLiteral("攻撃力アップ")]
        DamageUp,	//1
        [EnumLiteral("防御力アップ")]
        DefenseUp,	//2
        [EnumLiteral("インク効率アップ(メイン)")]
        InkSaverMain,	//3
        [EnumLiteral("インク効率アップ(サブ)")]
        InkSaverSub,	//4
        [EnumLiteral("インク回復力アップ")]
        InkRecoveryUp,	//5
        [EnumLiteral("ヒト移動速度アップ")]
        RunSpeedUp,	//6
        [EnumLiteral("イカダッシュ速度アップ")]
        SwimSpeedUp,	//7
        [EnumLiteral("スペシャル増加量アップ")]
        SpecialChargeUp,	//8
        [EnumLiteral("スペシャル時間延長")]
        SpecialDurationUp,	//9
        [EnumLiteral("復活時間短縮")]
        QuickRespawn,	//10
        [EnumLiteral("スペシャル減少量ダウン")]
        SpecialSaver,	//11
        [EnumLiteral("スーパージャンプ時間短縮")]
        QuickSuperJump,	//12
        [EnumLiteral("ボム飛距離アップ")]
        BombRangeUp,	//13
        [EnumLiteral("スタートダッシュ")]
        OpeningGambit,	//14
        [EnumLiteral("ラストスパート")]
        LastDitchEffort,	//15
        [EnumLiteral("逆境強化")]
        Tenacity,	//16
        [EnumLiteral("カムバック")]
        Comeback,	//17
        [EnumLiteral("マーキングガード")]
        ColdBlooded,	//18
        [EnumLiteral("イカニンジャ")]
        NinjaSquid,	//19
        [EnumLiteral("うらみ")]
        Haunt,	//20
        [EnumLiteral("スタートレーダー")]
        Recon,	//21
        [EnumLiteral("ボムサーチ")]
        BombSniffer,	//22
        [EnumLiteral("安全シューズ")]
        InkResistanceUp,	//23
        [EnumLiteral("ステルスジャンプ")]
        StealthJump	//24
    }
    #endregion
}