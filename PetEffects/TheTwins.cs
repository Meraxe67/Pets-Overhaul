using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
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
    public sealed class TheTwins : PetEffect
    {
        public int healthDmgCd = 48;
        public int closeRange = 112;
        public int longRange = 560;
        public float defLifestealDmgMult = 0.0001f;
        public float regularEnemyHpDmg = 0.01f;
        public float bossHpDmg = 0.001f;
        public int infernoTime = 240;
        public float defMult = 1.5f;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.TwinsPetItem))
            {
                Pet.SetPetAbilityTimer(healthDmgCd);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.TwinsPetItem))
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.CursedTorch, closeRange, 6);
                GlobalPet.CircularDustEffect(Player.Center, DustID.RedTorch, longRange, 30);
                if (Player.Distance(target.Center) > longRange && Pet.timer <= 0)
                {
                    if (target.boss == false || NpcPet.NonBossTrueBosses.Contains(target.type) == false)
                    {
                        modifiers.FlatBonusDamage += (int)(target.lifeMax * regularEnemyHpDmg);
                    }
                    else
                    {
                        modifiers.FlatBonusDamage += (int)(target.lifeMax * bossHpDmg);
                    }
                    Pet.timer = Pet.timerMax;
                }

            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.Distance(target.Center) < closeRange && GlobalPet.LifestealCheck(target) && Pet.PetInUseWithSwapCd(ItemID.TwinsPetItem))
            {
                target.AddBuff(BuffID.CursedInferno, infernoTime);
                if (Pet.timer <= 0) 
                {
                    Pet.PetRecovery(Player.statDefense * defMult * (Player.endurance + 1f), hit.Damage * defLifestealDmgMult);
                    Pet.timer = Pet.timerMax;
                }

            }
        }
    }
    public sealed class TwinsPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.TwinsPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            TheTwins theTwins = Main.LocalPlayer.GetModPlayer<TheTwins>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.TwinsPetItem")
                .Replace("<class>", PetTextsColors.ClassText(theTwins.PetClassPrimary, theTwins.PetClassSecondary))
                        .Replace("<closeRange>", Math.Round(theTwins.closeRange / 16f, 2).ToString())
                        .Replace("<cursedTime>", Math.Round(theTwins.infernoTime / 60f, 2).ToString())
                        .Replace("<defLifesteal>", theTwins.defMult.ToString())
                        .Replace("<dealtDmgLifesteal>", Math.Round(theTwins.defLifestealDmgMult * 10000, 2).ToString())
                        .Replace("<longRange>", Math.Round(theTwins.longRange / 16f, 2).ToString())
                        .Replace("<hpDmg>", Math.Round(theTwins.regularEnemyHpDmg * 100, 2).ToString())
                        .Replace("<bossHpDmg>", Math.Round(theTwins.bossHpDmg * 100, 2).ToString())
                        .Replace("<hpDmgCooldown>", Math.Round(theTwins.healthDmgCd / 60f, 2).ToString())
                        ));
        }
    }
}
