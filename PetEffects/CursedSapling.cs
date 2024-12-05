using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class CursedSapling : PetEffect
    {
        public override int PetItemID => ItemID.CursedSapling;
        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public float whipSpeed = 0.007f;
        public float whipRange = 0.01f;
        public float pumpkinWeaponDmg = 0.1f;
        public float ravenDmg = 0.175f;
        public int maxMinion = 1;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped())
            {
                if (item.netID == ItemID.StakeLauncher || item.netID == ItemID.TheHorsemansBlade || item.netID == ItemID.BatScepter || item.netID == ItemID.CandyCornRifle || item.netID == ItemID.ScytheWhip || item.netID == ItemID.JackOLanternLauncher)
                {
                    damage += pumpkinWeaponDmg;
                }
                if (item.netID == ItemID.RavenStaff)
                {
                    damage += ravenDmg;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.maxMinions += maxMinion;
                if (Player.HeldItem.type == ItemID.ScytheWhip)
                {
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += Player.maxMinions * whipSpeed * 2;
                    Player.whipRangeMultiplier += Player.maxMinions * whipRange * 2;
                }
                else if (Player.HeldItem.CountsAsClass<SummonMeleeSpeedDamageClass>())
                {
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += Player.maxMinions * whipSpeed;
                    Player.whipRangeMultiplier += Player.maxMinions * whipRange;
                }
            }
        }
    }
    public sealed class CursedSaplingItem : PetTooltip
    {
        public override PetEffect PetsEffect => cursedSapling;
        public static CursedSapling cursedSapling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CursedSapling pet))
                    return pet;
                else
                    return ModContent.GetInstance<CursedSapling>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.CursedSapling")
                        .Replace("<minionSlot>", cursedSapling.maxMinion.ToString())
                        .Replace("<dmg>", Math.Round(cursedSapling.pumpkinWeaponDmg * 100, 2).ToString())
                        .Replace("<ravenDmg>", Math.Round(cursedSapling.ravenDmg * 100, 2).ToString())
                        .Replace("<whipRange>", Math.Round(cursedSapling.whipRange * 100, 2).ToString())
                        .Replace("<whipSpeed>", Math.Round(cursedSapling.whipSpeed * 100, 2).ToString());
    }
}
