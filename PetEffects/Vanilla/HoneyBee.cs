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
        public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public float bottledHealth = 0.18f;
        public float honeyfinHealth = 0.25f;
        public float selfPotionIncrease = 0.1f;
        public int honeyOverdoseTime = 3600;
        public int bottledHoneyBuff = 1200;
        public int honeyfinHoneyBuff = 600;
        public float abilityHaste = 0.15f;
        public int range = 1600;
        public float currentAbilityHasteBonus = 0;
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (Player.GetModPlayer<GlobalPet>().PetInUse(ItemID.QueenBeePetItem))
            {
                healValue += (int)(healValue * selfPotionIncrease);
            }
        }
        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<HoneyOverdose>()))
            {
                Pet.abilityHaste += currentAbilityHasteBonus;
            }
        }
        public static void HealByHoneyBee(bool isBottledHoney, int healersWhoAmI, bool selfHeal)
        {
            HoneyBee healer = Main.player[healersWhoAmI].GetModPlayer<HoneyBee>();
            if (selfHeal)
            {
                Player player = healer.Player;
                int healAmount = (int)(player.statLifeMax2 * (isBottledHoney ? healer.bottledHealth : healer.honeyfinHealth) / 2);
                player.HealEffect(healAmount);
                if (player.statLife + healAmount > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }
                else
                {
                    player.statLife += healAmount;
                }
                player.AddBuff(BuffID.Honey, (isBottledHoney ? healer.bottledHoneyBuff : healer.honeyfinHoneyBuff) / 2);
                player.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(healer.honeyOverdoseTime * (1 / (1 + healer.abilityHaste))));
                player.GetModPlayer<HoneyBee>().currentAbilityHasteBonus = healer.abilityHaste;
            }
            else
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (i == healersWhoAmI || player.active == false || player.dead || player.Distance(healer.Player.Center) >= healer.range || player.HasBuff(ModContent.BuffType<HoneyOverdose>()))
                    {
                        continue;
                    }
                    int healAmount = (int)(player.statLifeMax2 * (isBottledHoney ? healer.bottledHealth : healer.honeyfinHealth));
                    player.HealEffect(healAmount);
                    if (player.statLife + healAmount > player.statLifeMax2)
                    {
                        player.statLife = player.statLifeMax2;
                    }
                    else
                    {
                        player.statLife += healAmount;
                    }
                    player.AddBuff(BuffID.Honey, isBottledHoney ? healer.bottledHoneyBuff : healer.honeyfinHoneyBuff);
                    player.AddBuff(ModContent.BuffType<HoneyOverdose>(), (int)(healer.honeyOverdoseTime * (1 / (1 + healer.abilityHaste))));
                    player.GetModPlayer<HoneyBee>().currentAbilityHasteBonus = healer.abilityHaste;
                }
            }
        }
    }
    public sealed class HoneyBeePotions : GlobalItem
    {
        public override bool ConsumeItem(Item item, Player player)
        {
            if (player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.QueenBeePetItem) && (item.type == ItemID.BottledHoney || item.type == ItemID.Honeyfin))
            {
                bool isBottledHoney = item.type == ItemID.BottledHoney;
                HoneyBee.HealByHoneyBee(isBottledHoney, player.whoAmI, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)PetsOverhaul.MessageType.honeyBeeHeal);
                    packet.Write(isBottledHoney);
                    packet.Write((byte)player.whoAmI);
                    packet.Send();
                }
            }
            return base.ConsumeItem(item, player);
        }
    }
    public sealed class QueenBeePetItem : GlobalItem
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
