using PetsOverhaul.Config;
using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class TinyDeerclops : ModPlayer
    {
        public List<(int storedDamage, int timer)> deerclopsTakenDamage = new();
        public int damageStoreTime = 300;
        public float healthTreshold = 0.4f;
        public int range = 480;
        public float slow = 0.4f;
        public int applyTime = 300;
        public int immuneTime = 180;
        public int cooldown = 1800;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
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
                Pet.timerMax = cooldown;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.DeerclopsPetItem))
            {
                if (deerclopsTakenDamage.Count > 0)
                {
                    for (int i = 0; i < deerclopsTakenDamage.Count; i++) //List'lerde struct'lar bir nevi readonly olarak çalıştığından, değeri alıp tekrar atıyoruz
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
                            SoundEngine.PlaySound(SoundID.DeerclopsScream with { PitchVariance = 0.4f, MaxInstances = 5 }, Player.position);
                        }

                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && Player.Distance(npc.Center) < range)
                            {
                                npc.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.Deerclops, slow, applyTime,npc);
                                if (npc.active && (npc.townNPC == false || npc.isLikeATownNPC == false || npc.friendly == false) && (npc.boss == false || npc.GetGlobalNPC<NpcPet>().nonBossTrueBosses[npc.type] == false))
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
}
public sealed class DeerclopsPetItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.DeerclopsPetItem;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
        {
            return;
        }

        TinyDeerclops tinyDeerclops = Main.LocalPlayer.GetModPlayer<TinyDeerclops>();
        tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DeerclopsPetItem")
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