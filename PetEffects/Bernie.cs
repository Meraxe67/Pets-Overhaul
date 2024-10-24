using PetsOverhaul.Config;
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
    public sealed class Bernie : PetEffect
    {
        public int bernieRange = 1000;
        public int burnDrain = 60; //maxtimer
        public int maxBurning = 5;
        public int manaDrain = 4;
        public int healthDrain = 3;
        public int EnemiesBurning { get; internal set; }
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BerniePetItem))
            {
                Pet.SetPetAbilityTimer(burnDrain);
            }
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BerniePetItem) && drawInfo.shadow == 0f)
            {
                drawInfo.DustCache.AddRange(GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, bernieRange, 100));
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BerniePetItem))
            {
                Player.buffImmune[BuffID.Burning] = true;
                Player.buffImmune[BuffID.OnFire] = true;
                Player.buffImmune[BuffID.OnFire3] = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.buffImmune[BuffID.CursedInferno] = true;
                Player.buffImmune[BuffID.ShadowFlame] = true;
                Player.buffImmune[BuffID.Frostburn2] = true;
                EnemiesBurning = 0;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && Player.Distance(npc.Center) < bernieRange)
                    {
                        if (Main.rand.NextBool())
                        {
                            for (int a = 0; a < NPC.maxBuffs; a++)
                            {
                                if (GlobalPet.BurnDebuffs.Contains(npc.buffType[a]))
                                {
                                    npc.buffTime[a]++;
                                }
                            }
                        }
                        for (int a = 0; a < NPC.maxBuffs; a++)
                        {
                            if (GlobalPet.BurnDebuffs.Contains(npc.buffType[a]))
                            {
                                EnemiesBurning++;
                                break;
                            }
                            if (EnemiesBurning >= 5)
                            {
                                break;
                            }
                        }
                    }
                }
                if (Pet.timer <= 0 && EnemiesBurning > 0)
                {
                    if (EnemiesBurning > 5)
                    {
                        EnemiesBurning = 5;
                    }

                    Pet.PetRecovery(burnDrain * healthDrain * EnemiesBurning, 0.005f, isLifesteal: false);
                    Pet.PetRecovery(burnDrain * manaDrain * EnemiesBurning, 0.005f, isLifesteal: false, manaSteal: true);
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class BerniePetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BerniePetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            Bernie bernie = Main.LocalPlayer.GetModPlayer<Bernie>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BerniePetItem")
                .Replace("<class>", PetTextsColors.ClassText(bernie.PetClassPrimary, bernie.PetClassSecondary))
                .Replace("<burnRange>", Math.Round(bernie.bernieRange / 16f, 2).ToString())
                .Replace("<burnDrainMana>", Math.Round(bernie.burnDrain * bernie.manaDrain * 0.05f, 2).ToString())
                .Replace("<burnDrainHealth>", Math.Round(bernie.burnDrain * bernie.healthDrain * 0.05f, 2).ToString())
                .Replace("<maxDrain>", bernie.maxBurning.ToString())
            ));
        }
    }
}
