using Microsoft.Xna.Framework;
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
    public sealed class Lizard : ModPlayer
    {
        public bool onLizardSteroidsOrNah = false;
        public int transformCd = 3600;
        public int transformTime = 1800;
        private int transformTimer = 0;
        public int maxSteroidCount = 10;
        public int steroidCount = 10;
        public float lizardLifesteal = 0.01f;
        public float lizardLifestealHealth = 0.01f;
        public float dmgMultIncrease = 1.1f;
        public int dmgFlatIncrease = 10;

                public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); private set { } }
        public override void PreUpdate()
        {
            if (Pet.skinColorChanged == false)
            {
                Pet.skin = Player.skinColor;
                Pet.skinColorChanged = true;
            }
            if (Pet.PetInUse(ItemID.LizardEgg) == false)
            {
                Player.skinColor = Pet.skin;
                Pet.skinColorChanged = false;
            }
            if (Pet.PetInUse(ItemID.LizardEgg))
            {
                Player.skinColor = Color.YellowGreen;
                Pet.timerMax = transformCd;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg))
            {
                if (transformTimer <= 0)
                {
                    onLizardSteroidsOrNah = false;
                }

                if (onLizardSteroidsOrNah == true)
                {
                    transformTimer--;
                    Player.moveSpeed += 0.2f;
                    Player.statDefense += 5;
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
                }
                if (Pet.timer == 0)
                {
                    AdvancedPopupRequest popupMessage = new();
                    popupMessage.Text = "Lizard Cooldown Ready!";
                    popupMessage.DurationInFrames = 120;
                    popupMessage.Velocity = new Vector2(0, -7);
                    popupMessage.Color = Color.ForestGreen;
                    PopupText.NewText(popupMessage, Player.position);
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg) && Pet.timer <= 0 && Player.statLife < Player.statLifeMax2 * 0.55f)
            {
                steroidCount = maxSteroidCount;
                transformTimer = transformTime;
                onLizardSteroidsOrNah = true;
                AdvancedPopupRequest popupMessage = new();
                popupMessage.Text = "Lizard genetics activated";
                popupMessage.DurationInFrames = 120;
                popupMessage.Velocity = new Vector2(0, -7);
                popupMessage.Color = Color.ForestGreen;
                PopupText.NewText(popupMessage, Player.position);
                Pet.timer = Pet.timerMax;
            }
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg) && steroidCount > 0 && onLizardSteroidsOrNah == true)
            {
                modifiers.FinalDamage *= dmgMultIncrease;
                modifiers.FlatBonusDamage += dmgFlatIncrease;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg) && steroidCount > 0 && onLizardSteroidsOrNah == true && GlobalPet.LifestealCheck(target))
            {
                Pet.Lifesteal(damageDone, lizardLifesteal, Pet.Lifesteal(Player.statLifeMax2, lizardLifestealHealth, respectLifeStealCap: false, doLifesteal: false));
                steroidCount--;
            }
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg) && Pet.timer <= 0 && Player.statLife < Player.statLifeMax2 * 0.55f)
            {
                steroidCount = maxSteroidCount;
                transformTimer = transformTime;
                onLizardSteroidsOrNah = true;
                AdvancedPopupRequest popupMessage = new();
                popupMessage.Text = "Lizard genetics activated";
                popupMessage.DurationInFrames = 120;
                popupMessage.Velocity = new Vector2(0, -7);
                popupMessage.Color = Color.ForestGreen;
                PopupText.NewText(popupMessage, Player.position);
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class LizardEgg : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LizardEgg;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Lizard lizard = Main.LocalPlayer.GetModPlayer<Lizard>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LizardEgg")
                        .Replace("<transformTime>", Math.Round(lizard.transformTime / 60f, 2).ToString())
                        .Replace("<hitCount>", lizard.maxSteroidCount.ToString())
                        .Replace("<hitDmg>", lizard.dmgMultIncrease.ToString())
                        .Replace("<hitFlat>", lizard.dmgFlatIncrease.ToString())
                        .Replace("<lifesteal>", Math.Round(lizard.lizardLifesteal * 100, 2).ToString())
                        .Replace("<maxHpRecovery>", Math.Round(lizard.lizardLifestealHealth * 100, 2).ToString())
                        .Replace("<transformCooldown>", Math.Round(lizard.transformCd / 60f, 2).ToString())
                        ));
        }
    }
}
