using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class TinyDeerclops : PetEffect
    {
        public List<(int storedDamage, int timer)> deerclopsTakenDamage = new();
        public int damageStoreTime = 300;
        public float healthTreshold = 0.4f;
        public int range = 480;
        public float slow = 0.4f;
        public int applyTime = 300;
        public int immuneTime = 180;
        public int cooldown = 1800;

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUse(ItemID.DeerclopsPetItem))
            {
                deerclopsTakenDamage.Add((info.Damage, damageStoreTime));
            }
        }
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.DeerclopsPetItem))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.DeerclopsPetItem))
            {
                if (deerclopsTakenDamage.Count > 0)
                {
                    for (int i = 0; i < deerclopsTakenDamage.Count; i++) //Pet will be reworked Post 3.0 update
                    {
                        (int storedDamage, int timer) value = deerclopsTakenDamage[i];
                        value.timer--;
                        deerclopsTakenDamage[i] = value;
                    }
                    deerclopsTakenDamage.RemoveAll(x => x.timer <= 0);
                    int totalDamage = 0;
                    deerclopsTakenDamage.ForEach(x => totalDamage += x.storedDamage);
                    if (totalDamage > Player.statLifeMax2 * healthTreshold && Pet.timer <= 0)
                    {
                        Pet.timer = Pet.timerMax;
                        if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                        {
                            SoundEngine.PlaySound(SoundID.DeerclopsScream with { PitchVariance = 0.4f, MaxInstances = 5 }, Player.Center);
                        }

                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && Player.Distance(npc.Center) < range)
                            {
                                NpcPet.AddSlow(new NpcPet.PetSlow(slow, applyTime, PetSlowIDs.Deerclops), npc);
                                if (npc.active && (npc.townNPC == false || npc.isLikeATownNPC == false || npc.friendly == false) && (npc.boss == false || NpcPet.NonBossTrueBosses.Contains(npc.type) == false))
                                {
                                    npc.AddBuff(BuffID.Confused, applyTime);
                                }

                                if (npc.active && (npc.townNPC == false || npc.isLikeATownNPC == false || npc.friendly == false))
                                {
                                    npc.AddBuff(BuffID.Frostburn, applyTime);
                                }
                            }
                        }
                        Player.SetImmuneTimeForAllTypes(immuneTime);
                    }
                }
            }
        }
    }
    public sealed class DeerclopsPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DeerclopsPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            TinyDeerclops tinyDeerclops = Main.LocalPlayer.GetModPlayer<TinyDeerclops>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DeerclopsPetItem")
                .Replace("<class>", PetTextsColors.ClassText(tinyDeerclops.PetClassPrimary, tinyDeerclops.PetClassSecondary))
                            .Replace("<treshold>", Math.Round(tinyDeerclops.healthTreshold * 100, 2).ToString())
                            .Replace("<tresholdTime>", Math.Round(tinyDeerclops.damageStoreTime / 60f, 2).ToString())
                            .Replace("<immunityTime>", Math.Round(tinyDeerclops.immuneTime / 60f, 2).ToString())
                            .Replace("<slowAmount>", Math.Round(tinyDeerclops.slow * 100, 2).ToString())
                            .Replace("<range>", Math.Round(tinyDeerclops.range / 16f, 2).ToString())
                            .Replace("<debuffTime>", Math.Round(tinyDeerclops.applyTime / 60f, 2).ToString())
                            .Replace("<cooldown>", Math.Round(tinyDeerclops.cooldown / 60f, 2).ToString())
                            ));
        }
    }
}