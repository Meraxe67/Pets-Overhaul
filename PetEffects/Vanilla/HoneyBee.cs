using PetsOverhaul.Buffs;
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
    public sealed class HoneyBee : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float bottledHealth = 0.18f;
        public float honeyfinHealth = 0.25f;
        public float selfPotionIncrease = 0.1f;
        public int honeyOverdoseTime = 3600;
        public int bottledHoneyBuff = 1200;
        public int honeyfinHoneyBuff = 600;
        public float abilityHaste = 0.15f;
        public int range = 1600;
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (Player.GetModPlayer<GlobalPet>().PetInUse(ItemID.QueenBeePetItem))
            {
                healValue += (int)(healValue * selfPotionIncrease);
            }
        }
    }
    public sealed class HoneyBeePotions : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool ConsumeItem(Item item, Player player)
        {
            if (player.TryGetModPlayer(out HoneyBee honeyBee) && honeyBee.Pet.PetInUse(ItemID.QueenBeePetItem))
            {
                if (item.type == ItemID.BottledHoney)
                {
                    player.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(honeyBee.honeyOverdoseTime * (1 / (1 + player.GetModPlayer<GlobalPet>().abilityHaste))));
                    if (player.active && player.HasBuff(ModContent.BuffType<HoneyOverdose>()) == false)
                    {
                        player.statLife += (int)(player.statLifeMax2 * honeyBee.bottledHealth) / 2;
                            player.HealEffect((int)(player.statLifeMax2 * honeyBee.bottledHealth) / 2);
                    }
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player targetPlayer = Main.player[i];
                        if (targetPlayer.active && targetPlayer.HasBuff(ModContent.BuffType<HoneyOverdose>()) == false && player.Distance(targetPlayer.Center) < honeyBee.range && player.whoAmI != targetPlayer.whoAmI)
                        {
                            if (targetPlayer.statLife + (int)(targetPlayer.statLifeMax2 * honeyBee.bottledHealth) > targetPlayer.statLifeMax2)
                            {
                                targetPlayer.statLife = targetPlayer.statLifeMax2;
                            }
                            else
                            {
                                targetPlayer.statLife += (int)(targetPlayer.statLifeMax2 * honeyBee.bottledHealth);
                            }
                                targetPlayer.HealEffect((int)(targetPlayer.statLifeMax2 * honeyBee.bottledHealth));
                            targetPlayer.AddBuff(BuffID.Honey, 1200);
                            targetPlayer.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(honeyBee.honeyOverdoseTime * (1 / (1 + player.GetModPlayer<GlobalPet>().abilityHaste))));

                        }
                    }
                }
                if (item.type == ItemID.Honeyfin)
                {
                    player.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(honeyBee.honeyOverdoseTime * (1 / (1 + player.GetModPlayer<GlobalPet>().abilityHaste))));
                    if (player.active && player.HasBuff(ModContent.BuffType<HoneyOverdose>()) == false)
                    {
                        player.statLife += (int)(player.statLifeMax2 * honeyBee.honeyfinHealth) / 2;
                            player.HealEffect((int)(player.statLifeMax2 * honeyBee.honeyfinHealth) / 2);
                    }
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player targetPlayer = Main.player[i];
                        if (targetPlayer.active && targetPlayer.HasBuff(ModContent.BuffType<HoneyOverdose>()) == false && player.Distance(targetPlayer.Center) < honeyBee.range && player.whoAmI != targetPlayer.whoAmI)
                        {
                            if (targetPlayer.statLife + (int)(targetPlayer.statLifeMax2 * honeyBee.honeyfinHealth) > targetPlayer.statLifeMax2)
                            {
                                targetPlayer.statLife = targetPlayer.statLifeMax2;
                            }
                            else
                            {
                                targetPlayer.statLife += (int)(targetPlayer.statLifeMax2 * honeyBee.honeyfinHealth);
                            }
                                targetPlayer.HealEffect((int)(targetPlayer.statLifeMax2 * honeyBee.honeyfinHealth));
                            targetPlayer.AddBuff(BuffID.Honey, 600);
                            targetPlayer.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(honeyBee.honeyOverdoseTime * (1 / (1 + player.GetModPlayer<GlobalPet>().abilityHaste))));
                        }
                    }
                }

            }
            return true;
        }
    }
    sealed public class QueenBeePetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.QueenBeePetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            HoneyBee honeyBee = Main.LocalPlayer.GetModPlayer<HoneyBee>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.QueenBeePetItem")
                       .Replace("<extraHeal>", Math.Round(honeyBee.selfPotionIncrease * 100, 2).ToString())
                       .Replace("<range>", Math.Round(honeyBee.range / 16f, 2).ToString())
                       .Replace("<bottledHealth>", Math.Round(honeyBee.bottledHealth * 100, 2).ToString())
                       .Replace("<honeyfinHealth>", Math.Round(honeyBee.honeyfinHealth * 100, 2).ToString())
                       .Replace("<bottledHoneyTime>", Math.Round(honeyBee.bottledHoneyBuff / 60f, 2).ToString())
                       .Replace("<honeyfinHoneyTime>", Math.Round(honeyBee.honeyfinHoneyBuff / 60f, 2).ToString())
                       .Replace("<abilityHaste>", Math.Round(honeyBee.abilityHaste * 100, 2).ToString())
                       ));
        }
    }
}
