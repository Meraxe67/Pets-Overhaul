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
    public sealed class Bernie : ModPlayer
    {
        public int bernieRange = 3200;
        private int timer = 0;
        public int burnDrain = 6;
        public int maxBurning = 5;
        public int EnemiesBurning { get; internal set; }
                public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BerniePetItem))
            {
                timer++;
            }
        }
        public override void PostUpdateEquips()
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
                        if (timer % 2 == 1)
                        {
                            for (int a = 0; a < NPC.maxBuffs; a++)
                            {
                                if (Pet.burnDebuffs[npc.buffType[a]])
                                {
                                    npc.buffTime[a]++;
                                }
                            }

                        }
                        for (int a = 0; a < NPC.maxBuffs; a++)
                        {
                            if (Pet.burnDebuffs[npc.buffType[a]])
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
                if (timer >= burnDrain)
                {
                    if (EnemiesBurning > 5)
                    {
                        EnemiesBurning = 5;
                    }

                    Pet.Lifesteal(burnDrain * 2 * EnemiesBurning, 0.005f * (Pet.abilityHaste + 1f), respectLifeStealCap: false);
                    Pet.Lifesteal(burnDrain * 4 * EnemiesBurning, 0.005f * (Pet.abilityHaste + 1f), respectLifeStealCap: false, manaSteal: true);
                    timer = 0;
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Bernie bernie = Main.LocalPlayer.GetModPlayer<Bernie>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BerniePetItem")
                .Replace("<burnRange>", Math.Round(bernie.bernieRange / 16f, 2).ToString())
                .Replace("<burnDrainMana>", Math.Round(bernie.burnDrain * 4 * 0.05f, 2).ToString())
                .Replace("<burnDrainHealth>", Math.Round(bernie.burnDrain * 2 * 0.05f, 2).ToString())
                .Replace("<maxDrain>", bernie.maxBurning.ToString())
            ));
        }
    }
}
