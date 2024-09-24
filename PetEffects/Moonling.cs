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
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class Moonling : PetEffect
    {
        public float meleeDr = 0.15f;
        public float meleeSpd = 0.2f;
        public float meleeDmg = 0.2f;
        public int rangedPen = 15;
        public float rangedDmg = 0.15f;
        public int rangedCr = 10;
        public float rangedCrDmg = 0.1f;
        public int magicMana = 150;
        public float magicDmg = 0.2f;
        public int magicCrit = 10;
        public float magicManaCost = 0.1f;
        public float sumWhipRng = 0.60f;
        public float sumWhipSpd = 0.3f;
        public int sumMinion = 3;
        public int sumSentry = 3;
        public int defense = 10;
        public StatModifier HighestDamage
        {
            get
            {
                StatModifier[] stats = { Player.GetDamage<MeleeDamageClass>(), Player.GetDamage<RangedDamageClass>(), Player.GetDamage<MagicDamageClass>(), Player.GetDamage<SummonDamageClass>() };
                return stats.MaxBy(x => x.Additive);
            }
            set { }
        }
        public int currentTooltip = 0; //0=Melee 1=Ranged 2=Magic 3=Summoner

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MoonLordPetItem))
            {
                if (HighestDamage == Player.GetDamage<MeleeDamageClass>())
                {
                    Player.endurance += meleeDr;
                    Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpd;
                    Player.GetDamage<MeleeDamageClass>() += meleeDmg;
                    Player.statDefense += defense;
                }
                else if (HighestDamage == Player.GetDamage<RangedDamageClass>())
                {
                    Player.GetArmorPenetration<RangedDamageClass>() += rangedPen;
                    Player.GetDamage<RangedDamageClass>() += rangedDmg;
                    Player.GetCritChance<RangedDamageClass>() += rangedCr;
                }
                else if (HighestDamage == Player.GetDamage<MagicDamageClass>())
                {
                    Player.statManaMax2 += magicMana;
                    Player.GetDamage<MagicDamageClass>() += magicDmg;
                    Player.GetCritChance<MagicDamageClass>() += magicCrit;
                    Player.manaCost -= magicManaCost;
                }
                else if (HighestDamage == Player.GetDamage<SummonDamageClass>())
                {
                    Player.whipRangeMultiplier += sumWhipRng;
                    Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += sumWhipSpd;
                    Player.maxMinions += sumMinion;
                    Player.maxTurrets += sumSentry;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Keybinds.PetTooltipSwap.JustPressed)
            {
                currentTooltip++;
                if (currentTooltip > 3)
                    currentTooltip = 0;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MoonLordPetItem) && modifiers.DamageType == DamageClass.Ranged && HighestDamage == Player.GetDamage<RangedDamageClass>())
            {
                modifiers.CritDamage += rangedCrDmg;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentTooltip", currentTooltip);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CurrentTooltip", out int tooltip))
            {
                currentTooltip = tooltip;
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
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Moonling moonling = Main.LocalPlayer.GetModPlayer<Moonling>();
            Player player = Main.LocalPlayer;

            string tooltip = moonling.currentTooltip switch //Current tooltip is separate from current class, tooltips can be switched with Pet Tooltip Swap keybind.
            {
                0 => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MeleeTooltip")
                                        .Replace("<dr>", Math.Round(moonling.meleeDr * 100, 2).ToString())
                                        .Replace("<meleeSpd>", Math.Round(moonling.meleeSpd * 100, 2).ToString())
                                        .Replace("<meleeDmg>", Math.Round(moonling.meleeDmg * 100, 2).ToString())
                                        .Replace("<def>", moonling.defense.ToString()),
                1 => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.RangedTooltip"
                                        ).Replace("<armorPen>", moonling.rangedPen.ToString())
                                        .Replace("<rangedCrit>", moonling.rangedCr.ToString())
                                        .Replace("<rangedCritDmg>", Math.Round(moonling.rangedCrDmg * 100, 2).ToString())
                                        .Replace("<rangedDmg>", Math.Round(moonling.rangedDmg * 100, 2).ToString()),
                2 => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MagicTooltip")
                                        .Replace("<mana>", moonling.magicMana.ToString())
                                        .Replace("<manaCost>", Math.Round(moonling.magicManaCost * 100, 2).ToString())
                                        .Replace("<magicCrit>", moonling.magicCrit.ToString())
                                        .Replace("<magicDmg>", Math.Round(moonling.magicDmg * 100, 2).ToString()),
                3 => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SummonerTooltip")
                                        .Replace("<sumRange>", Math.Round(moonling.sumWhipRng * 100, 2).ToString())
                                        .Replace("<sumSpd>", Math.Round(moonling.sumWhipSpd * 100, 2).ToString())
                                        .Replace("<sumMax>", moonling.sumMinion.ToString()),
                _ => "",
            };

            string currentClass = (moonling.HighestDamage == player.GetDamage<MeleeDamageClass>()) ? PetTextsColors.ClassText(PetClasses.Melee) :
            (moonling.HighestDamage == player.GetDamage<RangedDamageClass>()) ? PetTextsColors.ClassText(PetClasses.Ranged) :
            (moonling.HighestDamage == player.GetDamage<MagicDamageClass>()) ? PetTextsColors.ClassText(PetClasses.Magic) :
            (moonling.HighestDamage == player.GetDamage<SummonDamageClass>()) ? PetTextsColors.ClassText(PetClasses.Summoner) :
            PetTextsColors.ClassText(PetClasses.None);

            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MoonLordPetItem")
                .Replace("<class>", PetTextsColors.ClassText(moonling.PetClassPrimary, moonling.PetClassSecondary))
                .Replace("<currentClass>", currentClass)
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.PetTooltipSwap))
                .Replace("<tooltip>", tooltip)
                        ));
        }
    }
}
