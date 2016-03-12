using System.Collections.Generic;

namespace IkaStylist.Models
{
    static public class IkaUtil
    {
        //ギアパワーのIDをキーに名前を引ける辞書。逆引きのために作った。
        static public Dictionary<int, string> GearPowerDict = new Dictionary<int, string>(){
            { (int)GearPowerKind.None, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.None ) ) },
            { (int)GearPowerKind.DamageUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.DamageUp ) ) },
            { (int)GearPowerKind.DefenseUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.DefenseUp ) ) },
            { (int)GearPowerKind.InkSaverMain, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.InkSaverMain ) ) },
            { (int)GearPowerKind.InkSaverSub, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.InkSaverSub ) ) },
            { (int)GearPowerKind.InkRecoveryUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.InkRecoveryUp ) ) },
            { (int)GearPowerKind.RunSpeedUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.RunSpeedUp ) ) },
            { (int)GearPowerKind.SwimSpeedUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.SwimSpeedUp ) ) },
            { (int)GearPowerKind.SpecialChargeUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.SpecialChargeUp ) ) },
            { (int)GearPowerKind.SpecialDurationUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.SpecialDurationUp ) ) },
            { (int)GearPowerKind.QuickRespawn, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.QuickRespawn ) ) },
            { (int)GearPowerKind.SpecialSaver, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.SpecialSaver ) ) },
            { (int)GearPowerKind.QuickSuperJump, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.QuickSuperJump ) ) },
            { (int)GearPowerKind.BombRangeUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.BombRangeUp ) ) },
            { (int)GearPowerKind.OpeningGambit, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.OpeningGambit ) ) },
            { (int)GearPowerKind.LastDitchEffort, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.LastDitchEffort ) ) },
            { (int)GearPowerKind.Tenacity, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.Tenacity ) ) },
            { (int)GearPowerKind.Comeback, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.Comeback ) ) },
            { (int)GearPowerKind.ColdBlooded, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.ColdBlooded ) ) },
            { (int)GearPowerKind.NinjaSquid, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.NinjaSquid ) ) },
            { (int)GearPowerKind.Haunt, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.Haunt ) ) },
            { (int)GearPowerKind.Recon, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.Recon ) ) },
            { (int)GearPowerKind.BombSniffer, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.BombSniffer ) ) },
            { (int)GearPowerKind.InkResistanceUp, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.InkResistanceUp ) ) },
            { (int)GearPowerKind.StealthJump, EnumLiteralAttribute.GetLiteral( (GearPowerKind)( GearPowerKind.StealthJump ) ) },
        };
    }
}
