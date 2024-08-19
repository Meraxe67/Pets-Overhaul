using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyRedPanda : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public float regularAtkSpd = 0.05f;
        public float jungleBonusSpd = 0.04f;
        public int bambooChance = 50;
        public int alertTime = 300;
        public int alertCd = 1800;
        public float alertMs = 0.03f;
        public float alertAs = 0.015f;
        public int alertAggro = 125;
        public int alertRadius = 2400;
        private int alertEnemies = 1;
        public int alertEnemiesMax = 6;
        private int alertTimer = 0;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BambooLeaf))
            {
                Pet.SetPetAbilityTimer(alertCd);
                alertTimer--;
                if (alertTimer <= 0)
                {
                    alertTimer = 0;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BambooLeaf))
            {
                Player.GetAttackSpeed<GenericDamageClass>() += regularAtkSpd + (Player.ZoneJungle ? jungleBonusSpd : 0);
                if (alertTimer > 0)
                {
                    Player.GetAttackSpeed<GenericDamageClass>() += alertAs * alertEnemies;
                    Player.moveSpeed += alertMs * alertEnemies;
                    Player.aggro -= alertAggro * alertEnemies;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.timer <= 0 && Pet.PetInUseWithSwapCd(ItemID.BambooLeaf) && Keybinds.UsePetAbility.JustPressed)
            {
                SoundEngine.PlaySound(SoundID.Item37 with { Pitch = 1f }, Player.position);
                EmoteBubble.MakePlayerEmote(Player, EmoteID.EmotionAlert);
                alertEnemies = 1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && Player.Distance(npc.Center) < alertRadius)
                    {
                        alertEnemies++;
                    }
                }
                if (alertEnemies > alertEnemiesMax)
                {
                    alertEnemies = alertEnemiesMax;
                }
                alertTimer = alertTime;
                Pet.timer = Pet.timerMax;
            }
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            BabyRedPanda panda = player.GetModPlayer<BabyRedPanda>();
            if (PickerPet.PickupChecks(item, ItemID.BambooLeaf, out ItemPet _) && item.type == ItemID.BambooBlock)
            {
                for (int i = 0; i < GlobalPet.Randomizer(panda.bambooChance * item.stack); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), item.type, 1);
                }
            }
        }
    }
    public sealed class BambooLeaf : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BambooLeaf;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyRedPanda babyRedPanda = Main.LocalPlayer.GetModPlayer<BabyRedPanda>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BambooLeaf")
                .Replace("<class>", PetColors.ClassText(babyRedPanda.PetClassPrimary, babyRedPanda.PetClassSecondary))
                .Replace("<keybind>", Keybinds.UsePetAbility.GetAssignedKeys(GlobalPet.PlayerInputMode).Count > 0 ? Keybinds.UsePetAbility.GetAssignedKeys(GlobalPet.PlayerInputMode)[0] : $"[c/{Colors.RarityTrash.Hex3()}:{Language.GetTextValue("Mods.PetsOverhaul.KeybindMissing")}]")
                .Replace("<alertAs>", Math.Round(babyRedPanda.alertAs * 100, 2).ToString())
                .Replace("<alertMs>", Math.Round(babyRedPanda.alertMs * 100, 2).ToString())
                .Replace("<alertAggro>", babyRedPanda.alertAggro.ToString())
                .Replace("<alertRadius>", Math.Round(babyRedPanda.alertRadius / 16f, 2).ToString())
                .Replace("<alertMax>", babyRedPanda.alertEnemiesMax.ToString())
                .Replace("<alertDuration>", Math.Round(babyRedPanda.alertTime / 60f, 2).ToString())
                .Replace("<alertCd>", Math.Round(babyRedPanda.alertCd / 60f, 2).ToString())
                .Replace("<atkSpd>", Math.Round(babyRedPanda.regularAtkSpd * 100, 2).ToString())
                .Replace("<jungleAtkSpd>", Math.Round(babyRedPanda.jungleBonusSpd * 100, 2).ToString())
                .Replace("<bambooChance>", babyRedPanda.bambooChance.ToString())
            ));
        }
    }
}
