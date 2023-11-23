using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class SuspiciousLookingTentacleEffect : ModPlayer
    {
        GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.SuspiciousLookingTentacle && Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                Player.statDefense += moonlord.CurrentDef;
                Player.moveSpeed += moonlord.CurrentMs;
                Player.GetDamage<GenericDamageClass>() += moonlord.CurrentDmg;
                Player.GetCritChance<GenericDamageClass>() += moonlord.CurrentCrit;
                Player.slotsMinions -= moonlord.CurrentMinSlot;
                Player.whipRangeMultiplier += moonlord.CurrentWhip;
                Player.statManaMax2 += moonlord.CurrentMana;

            }
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (Player.miscEquips[1].type == ItemID.SuspiciousLookingTentacle && Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                healValue += (int)(moonlord.CurrentPot * healValue);
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (Player.miscEquips[1].type == ItemID.SuspiciousLookingTentacle && Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                scale += moonlord.CurrentSize;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.miscEquips[1].type == ItemID.SuspiciousLookingTentacle && Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingTentacle moonlord))
            {
                if (modifiers.DamageType == DamageClass.Ranged)
                {
                    modifiers.ScalingArmorPenetration += moonlord.CurrentPen;
                    modifiers.CritDamage += moonlord.CurrentCrDmg;
                }
                if (modifiers.DamageType == DamageClass.Melee && GlobalPet.LifestealCheck(target))
                    Pet.Lifesteal(Player.statDefense * 0.1f, moonlord.CurrentHeal);
            }
        }
    }
    public sealed class SuspiciousLookingTentacle : GlobalItem
    {
        public int CurrentDef => defPer * defRoll;
        public int defPer = 1;
        public int defRoll = 0;
        public int defMax = 5;

        public float CurrentMs => msPer * msRoll;
        public float msPer = 0.004f;
        public int msRoll = 0;
        public int msMax = 20;

        public float CurrentDmg => dmgPer * dmgRoll;
        public float dmgPer = 0.0025f;
        public int dmgRoll = 0;
        public int dmgMax = 20;

        public float CurrentCrit => critPer * critRoll;
        public float critPer = 0.25f;
        public int critRoll = 0;
        public int critMax = 20;

        public float CurrentPen => penPer * penRoll;
        public float penPer = 0.03f;
        public int penRoll = 0;
        public int penMax = 5;

        public float CurrentCrDmg => crDmgPer * crDmgRoll;
        public float crDmgPer = 0.01f;
        public int crDmgRoll = 0;
        public int crDmgMax = 5;

        public float CurrentMinSlot => minPer * minRoll;
        public float minPer = 0.2f;
        public int minRoll = 0;
        public int minMax = 5;

        public float CurrentWhip => whipPer * whipRoll;
        public float whipPer = 0.02f;
        public int whipRoll = 0;
        public int whipMax = 5;

        public float CurrentSize => sizePer * sizeRoll;
        public float sizePer = 0.04f;
        public int sizeRoll = 0;
        public int sizeMax = 5;

        public float CurrentHeal => healPer * healRoll;
        public float healPer = 0.03f;
        public int healRoll = 0;
        public int healMax = 5;

        public float CurrentPot => potPer * potRoll;
        public float potPer = 0.05f;
        public int potRoll = 0;
        public int potMax = 5;

        public int CurrentMana => manaPer * manaRoll;
        public int manaPer = 15;
        public int manaRoll = 0;
        public int manaMax = 5;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SuspiciousLookingTentacle;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (crDmgRoll <= 0)
                crDmgRoll = Main.rand.Next(crDmgMax) + 1;
            if (critRoll <= 0)
                critRoll = Main.rand.Next(critMax) + 1;
            if (defRoll <= 0)
                defRoll = Main.rand.Next(defMax) + 1;
            if (dmgRoll <= 0)
                dmgRoll = Main.rand.Next(dmgMax) + 1;
            if (healRoll <= 0)
                healRoll = Main.rand.Next(healMax) + 1;
            if (manaRoll <= 0)
                manaRoll = Main.rand.Next(manaMax) + 1;
            if (minRoll <= 0)
                minRoll = Main.rand.Next(minMax) + 1;
            if (msRoll <= 0)
                msRoll = Main.rand.Next(msMax) + 1;
            if (penRoll <= 0)
                penRoll = Main.rand.Next(penMax) + 1;
            if (potRoll <= 0)
                potRoll = Main.rand.Next(potMax) + 1;
            if (sizeRoll <= 0)
                sizeRoll = Main.rand.Next(sizeMax) + 1;
            if (whipRoll <= 0)
                whipRoll = Main.rand.Next(whipMax) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("MlCrDmg", crDmgRoll);
            tag.Add("MlCrit", critRoll);
            tag.Add("MlDef", defRoll);
            tag.Add("MlDmg", dmgRoll);
            tag.Add("MlHeal", healRoll);
            tag.Add("MlMana", manaRoll);
            tag.Add("MlMin", minRoll);
            tag.Add("MlMs", msRoll);
            tag.Add("MlPen", penRoll);
            tag.Add("MlPot", potRoll);
            tag.Add("MlSize", sizeRoll);
            tag.Add("MlWhip", whipRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            crDmgRoll = tag.GetInt("MlCrDmg");
            critRoll = tag.GetInt("MlCrit");
            defRoll = tag.GetInt("MlDef");
            dmgRoll = tag.GetInt("MlDmg");
            healRoll = tag.GetInt("MlHeal");
            manaRoll = tag.GetInt("MlMana");
            minRoll = tag.GetInt("MlMin");
            msRoll = tag.GetInt("MlMs");
            penRoll = tag.GetInt("MlPen");
            potRoll = tag.GetInt("MlPot");
            sizeRoll = tag.GetInt("MlSize");
            whipRoll = tag.GetInt("MlWhip");
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.SuspiciousLookingTentacle")

                        .Replace("<crDmgPer>", Math.Round(crDmgPer * 100, 2).ToString())
                        .Replace("<currentCrDmg>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentCrDmg * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), crDmgRoll, crDmgMax))
                        .Replace("<crDmgRoll>", GlobalPet.LightPetRarityColorConvert(crDmgRoll.ToString(), crDmgRoll, crDmgMax))
                        .Replace("<crDmgMax>", GlobalPet.LightPetRarityColorConvert(crDmgMax.ToString(), crDmgRoll, crDmgMax))

                        .Replace("<critPer>", Math.Round(critPer, 2).ToString())
                        .Replace("<currentCrit>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentCrit, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), critRoll, critMax))
                        .Replace("<critRoll>", GlobalPet.LightPetRarityColorConvert(critRoll.ToString(), critRoll, critMax))
                        .Replace("<critMax>", GlobalPet.LightPetRarityColorConvert(critMax.ToString(), critRoll, critMax))

                        .Replace("<defPer>", defPer.ToString())
                        .Replace("<currentDef>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentDef.ToString(), defRoll, defMax))
                        .Replace("<defRoll>", GlobalPet.LightPetRarityColorConvert(defRoll.ToString(), defRoll, defMax))
                        .Replace("<defMax>", GlobalPet.LightPetRarityColorConvert(defMax.ToString(), defRoll, defMax))

                        .Replace("<dmgPer>", Math.Round(dmgPer * 100, 2).ToString())
                        .Replace("<currentDmg>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentDmg * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), dmgRoll, dmgMax))
                        .Replace("<dmgRoll>", GlobalPet.LightPetRarityColorConvert(dmgRoll.ToString(), dmgRoll, dmgMax))
                        .Replace("<dmgMax>", GlobalPet.LightPetRarityColorConvert(dmgMax.ToString(), dmgRoll, dmgMax))

                        .Replace("<healPer>", Math.Round(healPer * 100, 2).ToString())
                        .Replace("<healAmount>", (Main.LocalPlayer.statDefense * 0.1f).ToString())
                        .Replace("<currentHeal>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentHeal * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), healRoll, healMax))
                        .Replace("<healRoll>", GlobalPet.LightPetRarityColorConvert(healRoll.ToString(), healRoll, healMax))
                        .Replace("<healMax>", GlobalPet.LightPetRarityColorConvert(healMax.ToString(), healRoll, healMax))

                        .Replace("<manaPer>", manaPer.ToString())
                        .Replace("<currentMana>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMana.ToString(), manaRoll, manaMax))
                        .Replace("<manaRoll>", GlobalPet.LightPetRarityColorConvert(manaRoll.ToString(), manaRoll, manaMax))
                        .Replace("<manaMax>", GlobalPet.LightPetRarityColorConvert(manaMax.ToString(), manaRoll, manaMax))

                        .Replace("<minPer>", minPer.ToString())
                        .Replace("<currentMin>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMinSlot.ToString(), minRoll, minMax))
                        .Replace("<minRoll>", GlobalPet.LightPetRarityColorConvert(minRoll.ToString(), minRoll, minMax))
                        .Replace("<minMax>", GlobalPet.LightPetRarityColorConvert(minMax.ToString(), minRoll, minMax))

                        .Replace("<msPer>", Math.Round(msPer * 100, 2).ToString())
                        .Replace("<currentMs>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentMs * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), msRoll, msMax))
                        .Replace("<msRoll>", GlobalPet.LightPetRarityColorConvert(msRoll.ToString(), msRoll, msMax))
                        .Replace("<msMax>", GlobalPet.LightPetRarityColorConvert(msMax.ToString(), msRoll, msMax))

                        .Replace("<penPer>", Math.Round(penPer * 100, 2).ToString())
                        .Replace("<currentPen>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentPen * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), penRoll, penMax))
                        .Replace("<penRoll>", GlobalPet.LightPetRarityColorConvert(penRoll.ToString(), penRoll, penMax))
                        .Replace("<penMax>", GlobalPet.LightPetRarityColorConvert(penMax.ToString(), penRoll, penMax))

                        .Replace("<potPer>", Math.Round(potPer * 100, 2).ToString())
                        .Replace("<currentPot>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentPot * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), potRoll, potMax))
                        .Replace("<potRoll>", GlobalPet.LightPetRarityColorConvert(potRoll.ToString(), potRoll, potMax))
                        .Replace("<potMax>", GlobalPet.LightPetRarityColorConvert(potMax.ToString(), potRoll, potMax))

                        .Replace("<sizePer>", Math.Round(sizePer * 100, 2).ToString())
                        .Replace("<currentSize>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentSize * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), sizeRoll, sizeMax))
                        .Replace("<sizeRoll>", GlobalPet.LightPetRarityColorConvert(sizeRoll.ToString(), sizeRoll, sizeMax))
                        .Replace("<sizeMax>", GlobalPet.LightPetRarityColorConvert(sizeMax.ToString(), sizeRoll, sizeMax))

                        .Replace("<whipPer>", Math.Round(whipPer * 100, 2).ToString())
                        .Replace("<currentWhip>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentWhip * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), whipRoll, whipMax))
                        .Replace("<whipRoll>", GlobalPet.LightPetRarityColorConvert(whipRoll.ToString(), whipRoll, whipMax))
                        .Replace("<whipMax>", GlobalPet.LightPetRarityColorConvert(whipMax.ToString(), whipRoll, whipMax))

                        ));
        }
    }
}
