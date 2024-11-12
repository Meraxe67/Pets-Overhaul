using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Estee : PetEffect
    {
        public override int PetItemID => ItemID.CelestialWand;
        public override PetClasses PetClassPrimary => PetClasses.Magic;
        public float manaIncrease = 0.15f;
        public float manaMagicIncreasePer1 = 0.001f;
        public float penaltyMult = 0.6f;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                int manaMult;
                Player.statManaMax2 += (int)(Player.statManaMax2 * manaIncrease);
                if (Player.statManaMax2 >= Player.statManaMax)
                {
                    manaMult = Player.statManaMax2 - Player.statManaMax;
                }
                else
                {
                    manaMult = 0;
                }
                if (Player.GetTotalDamage<MagicDamageClass>().Additive > 1f)
                {
                    Player.GetDamage<MagicDamageClass>() -= (Player.GetTotalDamage<MagicDamageClass>().Additive - 1f) * penaltyMult;
                }
                Player.GetDamage<MagicDamageClass>() += manaMult * manaMagicIncreasePer1;
                if (Player.armor[0].netID == ItemID.SpectreHood && Player.armor[0].netID == ItemID.SpectreRobe && Player.armor[0].netID == ItemID.SpectrePants)
                {
                    Player.GetDamage<MagicDamageClass>() -= 0.15f;
                }
                if (Player.manaSick)
                {
                    Player.GetDamage<MagicDamageClass>() -= Player.manaSickReduction * 0.25f;
                }
            }
        }
    }
    public sealed class CelestialWand : PetTooltip
    {
        public override PetEffect PetsEffect => estee;
        public static Estee estee
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Estee pet))
                    return pet;
                else
                    return ModContent.GetInstance<Estee>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.CelestialWand")
                        .Replace("<maxMana>", Math.Round(estee.manaIncrease * 100, 2).ToString())
                        .Replace("<dmgPenalty>", estee.penaltyMult.ToString())
                        .Replace("<manaToDmg>", Math.Round(estee.manaMagicIncreasePer1 * 100, 2).ToString());
    }
}
