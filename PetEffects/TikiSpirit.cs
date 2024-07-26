using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class TikiSpirit : PetEffect
    {
        public int whipCritBonus = 10;
        public int nonWhipCrit = 8;
        public float atkSpdToDmgConversion = 0.30f;
        public float atkSpdToRangeConversion = 0.15f;

        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.TikiTotem))
            {
                if (Player.GetTotalAttackSpeed<SummonMeleeSpeedDamageClass>() > Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>())
                {
                    Player.GetDamage<SummonDamageClass>() += (Player.GetTotalAttackSpeed<SummonMeleeSpeedDamageClass>() - Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>()) * atkSpdToDmgConversion;
                    Player.whipRangeMultiplier += (Player.GetTotalAttackSpeed<SummonMeleeSpeedDamageClass>() - Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>()) * atkSpdToRangeConversion;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.TikiTotem))
            {
                if (ProjectileID.Sets.IsAWhip[proj.type])
                {
                    if (proj.CritChance + whipCritBonus >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(proj.CritChance + whipCritBonus, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
                else if (proj.IsMinionOrSentryRelated)
                {
                    if (proj.CritChance - nonWhipCrit >= 100)
                    {
                        modifiers.SetCrit();
                    }
                    else if (Main.rand.NextBool(proj.CritChance - nonWhipCrit, 100))
                    {
                        modifiers.SetCrit();
                    }
                    else
                    {
                        modifiers.DisableCrit();
                    }
                }
            }
        }
    }
    public sealed class TikiTotem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.TikiTotem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            TikiSpirit tikiSpirit = Main.LocalPlayer.GetModPlayer<TikiSpirit>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.TikiTotem")
                .Replace("<class>", PetColors.ClassText(tikiSpirit.PetClassPrimary, tikiSpirit.PetClassSecondary))
                       .Replace("<atkSpdToDmg>", Math.Round(tikiSpirit.atkSpdToDmgConversion * 100, 2).ToString())
                       .Replace("<atkSpdToRange>", Math.Round(tikiSpirit.atkSpdToRangeConversion * 100, 2).ToString())
                       .Replace("<nonWhipCrit>", tikiSpirit.nonWhipCrit.ToString())
                       .Replace("<whipCrit>", tikiSpirit.whipCritBonus.ToString())
                       ));
        }
    }
}
