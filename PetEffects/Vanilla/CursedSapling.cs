using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class CursedSapling : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float whipSpeed = 0.00625f;
        public float whipRange = 0.01f;
        public float pumpkinWeaponDmg = 0.1f;
        public float ravenDmg = 0.25f;
        public int maxMinion = 1;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.CursedSapling))
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
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.CursedSapling))
            {
                Player.maxMinions += maxMinion;
                if (Player.HeldItem.type == ItemID.ScytheWhip)
                {
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += Player.maxMinions * whipSpeed * 4;
                    Player.whipRangeMultiplier += Player.maxMinions * whipRange * 4;
                }
                else if (Player.HeldItem.CountsAsClass<SummonMeleeSpeedDamageClass>())
                {
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += Player.maxMinions * whipSpeed;
                    Player.whipRangeMultiplier += Player.maxMinions * whipRange;
                }
            }
        }
    }
    public sealed class CursedSaplingItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CursedSapling;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            CursedSapling cursedSapling = Main.LocalPlayer.GetModPlayer<CursedSapling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.CursedSapling")
                        .Replace("<minionSlot>", cursedSapling.maxMinion.ToString())
                        .Replace("<dmg>", Math.Round(cursedSapling.pumpkinWeaponDmg * 100, 2).ToString())
                        .Replace("<ravenDmg>", Math.Round(cursedSapling.ravenDmg * 100, 2).ToString())
                        .Replace("<whipRange>", Math.Round(cursedSapling.whipRange * 100, 2).ToString())
                        .Replace("<whipSpeed>", Math.Round(cursedSapling.whipSpeed * 100, 2).ToString())
                        ));
        }
    }
}
