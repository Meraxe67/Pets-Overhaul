using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class ShadowOrbEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.ShadowOrb && Player.miscEquips[1].TryGetGlobalItem(out ShadowOrb shadowOrb))
            {
                Player.statManaMax2 += shadowOrb.CurrentMana;
                Player.moveSpeed += shadowOrb.CurrentHarvExp;
                Pet.harvestingFortune += shadowOrb.CurrentHarvFort;
            }
        }
    }
    public sealed class ShadowOrb : GlobalItem
    {
        public int baseMana = 20;
        public int manaPerRoll = 2;
        public int manaMaxRoll = 10;
        public int manaRoll = 0;
        public int CurrentMana => baseMana + manaPerRoll * manaRoll;

        public float baseMs = 0.025f;
        public float msPerRoll = 0.005f;
        public int msMaxRoll = 15;
        public int msRoll = 0;
        public float CurrentHarvExp => baseMs + msPerRoll * msRoll;

        public int baseHarvFort = 5;
        public int harvFortPerRoll = 1;
        public int harvFortMaxRoll = 15;
        public int harvFortRoll = 0;
        public int CurrentHarvFort => baseHarvFort + harvFortPerRoll * harvFortRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShadowOrb;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (manaRoll <= 0)
            {
                manaRoll = Main.rand.Next(manaMaxRoll) + 1;
            }

            if (msRoll <= 0)
            {
                msRoll = Main.rand.Next(msMaxRoll) + 1;
            }

            if (harvFortRoll <= 0)
            {
                harvFortRoll = Main.rand.Next(harvFortMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)manaRoll);
            writer.Write((byte)msRoll);
            writer.Write((byte)harvFortRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            manaRoll = reader.ReadByte();
            msRoll = reader.ReadByte();
            harvFortRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("ShadowMana", manaRoll);
            tag.Add("ShadowExp", msRoll); //exp stats are obsolete
            tag.Add("ShadowFort", harvFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("ShadowMana", out int mana))
            {
                manaRoll = mana;
            }

            if (tag.TryGet("ShadowExp", out int exp))
            {
                msRoll = exp;
            }

            if (tag.TryGet("ShadowFort", out int fort))
            {
                harvFortRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.ShadowOrb")

                        .Replace("<manaBase>", baseMana.ToString())
                        .Replace("<manaPer>", manaPerRoll.ToString())

                        .Replace("<expBase>", Math.Round(baseMs * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(msPerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseHarvFort.ToString())
                        .Replace("<fortPer>", harvFortPerRoll.ToString())

                        .Replace("<currentMana>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMana.ToString(), manaRoll, manaMaxRoll))
                        .Replace("<manaRoll>", GlobalPet.LightPetRarityColorConvert(manaRoll.ToString(), manaRoll, manaMaxRoll))
                        .Replace("<manaMaxRoll>", GlobalPet.LightPetRarityColorConvert(manaMaxRoll.ToString(), manaRoll, manaMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentHarvExp * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), msRoll, msMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(msRoll.ToString(), msRoll, msMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(msMaxRoll.ToString(), msRoll, msMaxRoll))

                        .Replace("<currentFort>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentHarvFort.ToString(), harvFortRoll, harvFortMaxRoll))
                        .Replace("<fortRoll>", GlobalPet.LightPetRarityColorConvert(harvFortRoll.ToString(), harvFortRoll, harvFortMaxRoll))
                        .Replace("<fortMaxRoll>", GlobalPet.LightPetRarityColorConvert(harvFortMaxRoll.ToString(), harvFortRoll, harvFortMaxRoll))
                        ));
            if (manaRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + GlobalPet.lowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
