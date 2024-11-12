using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Glommer : PetEffect
    {
        public override int PetItemID => ItemID.GlommerPetItem;
        public int glommerSanityTime = 60;
        public int glommerSanityRecover = 2;
        public float glommerSanityAura = 0.3f;
        public int glommerSanityRange = 1040;

        public override PetClasses PetClassPrimary => PetClasses.Magic;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
        public override void PreUpdate()
        {
            if (PetIsEquipped(false))
            {
                Pet.SetPetAbilityTimer(glommerSanityTime);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false) && Main.rand.NextBool(18000))
            {
                Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), ModContent.ItemType<GlommersGoop>());
            }

            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.ShimmerTorch, glommerSanityRange, 100);
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player plr = Main.player[i];
                    if (Player.Distance(plr.Center) < glommerSanityRange && plr.active && plr.whoAmI != 255)
                    {
                        plr.GetModPlayer<GlobalPet>().abilityHaste += glommerSanityAura;
                        plr.AddBuff(ModContent.BuffType<SanityAura>(), 1);
                    }
                }
                Player.statManaMax2 += (int)(Pet.abilityHaste * Player.statManaMax2);
                if (Pet.timer <= 0 && Player.statMana != Player.statManaMax2)
                {
                    Player.statMana += glommerSanityRecover;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class GlommerPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => glommer;
        public static Glommer glommer
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Glommer pet))
                    return pet;
                else
                    return ModContent.GetInstance<Glommer>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.GlommerPetItem")
                        .Replace("<sanityRange>", Math.Round(glommer.glommerSanityRange / 16f, 2).ToString())
                        .Replace("<sanityAmount>", Math.Round(glommer.glommerSanityAura * 100, 2).ToString())
                        .Replace("<currentHaste>", Math.Round(glommer.Pet.abilityHaste * 100, 2).ToString())
                        .Replace("<manaRecover>", glommer.glommerSanityRecover.ToString())
                        .Replace("<manaRecoverCd>", Math.Round(glommer.glommerSanityTime / 60f, 2).ToString())
                        .Replace("<goop>", ModContent.ItemType<GlommersGoop>().ToString());
    }
}
