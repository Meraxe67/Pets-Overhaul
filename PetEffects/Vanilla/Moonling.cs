using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Moonling : ModPlayer
    {
        public float meleeDr = 0.15f;
        public float meleeSpd = 0.2f;
        public float meleeDmg = 0.2f;
        public int rangedPen = 20;
        public float rangedDmg = 0.15f;
        public int rangedCr = 20;
        public int magicMana = 150;
        public float magicDmg = 0.2f;
        public int magicCrit = 10;
        public float magicManaCost = 0.1f;
        public float sumDmg = 0.1f;
        public float sumWhipRng = 0.55f;
        public float sumWhipSpd = 0.2f;
        public int sumMinion = 2;
        public int sumSentry = 2;
        public int defense = 10;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MoonLordPetItem))
            {
                StatModifier[] stats = { Player.GetDamage<MeleeDamageClass>(), Player.GetDamage<RangedDamageClass>(), Player.GetDamage<MagicDamageClass>(), Player.GetDamage<SummonDamageClass>() };
                StatModifier highestDamage = stats.MaxBy(x => x.Additive);
                if (highestDamage == Player.GetDamage<MeleeDamageClass>())
                {
                    Player.endurance += meleeDr;
                    Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpd;
                    Player.GetDamage<MeleeDamageClass>() += meleeDmg;
                    Player.statDefense += defense;
                }
                else if (highestDamage == Player.GetDamage<RangedDamageClass>())
                {
                    Player.GetArmorPenetration<RangedDamageClass>() += rangedPen;
                    Player.GetDamage<RangedDamageClass>() += rangedDmg;
                    Player.GetCritChance<RangedDamageClass>() += rangedCr;
                }
                else if (highestDamage == Player.GetDamage<MagicDamageClass>())
                {
                    Player.statManaMax2 += magicMana;
                    Player.GetDamage<MagicDamageClass>() += magicDmg;
                    Player.GetCritChance<MagicDamageClass>() += magicCrit;
                    Player.manaCost -= magicManaCost;
                }
                else if (highestDamage == Player.GetDamage<SummonDamageClass>())
                {
                    Player.GetDamage<SummonDamageClass>() += sumDmg;
                    Player.whipRangeMultiplier += sumWhipRng;
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += sumWhipSpd;
                    Player.maxMinions += sumMinion;
                    Player.maxTurrets += sumSentry;
                }
            }
        }
    }
    public sealed class MoonLordPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MoonLordPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Moonling moonling = Main.LocalPlayer.GetModPlayer<Moonling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MoonLordPetItem")
                        .Replace("<sumRange>", Math.Round(moonling.sumWhipRng * 100, 2).ToString())
                        .Replace("<sumSpd>", Math.Round(moonling.sumWhipSpd * 100, 2).ToString())
                        .Replace("<sumDmg>", Math.Round(moonling.sumDmg * 100, 2).ToString())
                        .Replace("<sumMax>", moonling.sumMinion.ToString())
                        .Replace("<mana>", moonling.magicMana.ToString())
                        .Replace("<manaCost>", Math.Round(moonling.magicManaCost * 100, 2).ToString())
                        .Replace("<magicCrit>", moonling.magicCrit.ToString())
                        .Replace("<magicDmg>", Math.Round(moonling.magicDmg * 100, 2).ToString())
                        .Replace("<armorPen>", moonling.rangedPen.ToString())
                        .Replace("<rangedCrit>", moonling.rangedCr.ToString())
                        .Replace("<rangedDmg>", Math.Round(moonling.rangedDmg * 100, 2).ToString())
                        .Replace("<dr>", Math.Round(moonling.meleeDr * 100, 2).ToString())
                        .Replace("<meleeSpd>", Math.Round(moonling.meleeSpd * 100, 2).ToString())
                        .Replace("<meleeDmg>", Math.Round(moonling.meleeDmg * 100, 2).ToString())
                        .Replace("<def>", moonling.defense.ToString())
                        ));
        }
    }
}
