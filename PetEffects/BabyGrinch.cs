using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyGrinch : PetEffect
    {
        public override int PetItemID => ItemID.BabyGrinchMischiefWhistle;
        public float winterDmg = 0.15f;
        public int winterCrit = 10;
        public float grinchSlow = 0.9f;
        public int grinchRange = 400;

        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped())
            {
                if (item.netID == ItemID.ChristmasTreeSword || item.netID == ItemID.Razorpine || item.netID == ItemID.ElfMelter || item.netID == ItemID.ChainGun || item.netID == ItemID.BlizzardStaff || item.netID == ItemID.SnowmanCannon || item.netID == ItemID.NorthPole)
                {
                    damage += winterDmg;
                }
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped())
            {
                if (item.netID == ItemID.ChristmasTreeSword || item.netID == ItemID.Razorpine || item.netID == ItemID.ElfMelter || item.netID == ItemID.ChainGun || item.netID == ItemID.BlizzardStaff || item.netID == ItemID.SnowmanCannon || item.netID == ItemID.NorthPole)
                {
                    crit += winterCrit;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.Snow, grinchRange, 20);
                Player.resistCold = true;

                foreach (var npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < grinchRange)
                    {
                        NpcPet.AddSlow(new NpcPet.PetSlow(grinchSlow, 1, PetSlowIDs.Grinch), npc);
                    }
                }
            }
        }
    }
    public sealed class BabyGrinchMischiefWhistle : PetTooltip
    {
        public override PetEffect PetsEffect => babyGrinch;
        public static BabyGrinch babyGrinch
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyGrinch pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyGrinch>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BabyGrinchMischiefWhistle")
                .Replace("<slowAmount>", Math.Round(babyGrinch.grinchSlow * 100, 2).ToString())
                .Replace("<slowRange>", Math.Round(babyGrinch.grinchRange / 16f, 2).ToString())
                .Replace("<dmg>", Math.Round(babyGrinch.winterDmg * 100, 2).ToString())
                .Replace("<crit>", babyGrinch.winterCrit.ToString());
    }
}

