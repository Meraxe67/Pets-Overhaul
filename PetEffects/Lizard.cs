using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Lizard : PetEffect
    {
        public float percentHpDmg = 0.1f;
        public int buffDurations = 120;
        private int buffTimer = 0;
        public float tailAcc = 1f;
        public float tailSpd = 0.5f;
        public int tailAggro = 10000;
        public int tailWait = 300;
        public float percentHpRecover = 0.1f;
        public float tailCdRefund = 0.5f;
        public float tailMaxHp = 0.5f; //Uses Player's maximum health.
        public int tailCooldown = 3600;

        public float kbResist = 0.6f;
        public float moveSpd = 0.7f;
        public int defense = 10;
        public float jumpMult = 0.5f;

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override void PreUpdate()
        {
            if (Pet.skinColorChanged == false)
            {
                Pet.skin = Player.skinColor;
                Pet.skinColorChanged = true;
            }
            if (Pet.PetInUse(ItemID.LizardEgg) == false || Pet.PetInUse(ItemID.LizardEgg) && Player.statLife > Player.statLifeMax2 * 0.55f)
            {
                Player.skinColor = Pet.skin;
                Pet.skinColorChanged = false;
            }
            if (Pet.PetInUse(ItemID.LizardEgg))
            {
                buffTimer--;
                if (buffTimer <= 0)
                    buffTimer = 0;
                Pet.SetPetAbilityTimer(tailCooldown);
                if (Player.statLife < Player.statLifeMax2 * 0.55f)
                {
                    Player.skinColor = Color.YellowGreen;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(ItemID.LizardEgg))
            {
                int dmg = (int)(Player.statLifeMax2 * percentHpDmg);
                if (Player.statLife < dmg)
                {
                    Player.statLife = dmg + 1;
                }
                Player.Hurt(new Player.HurtInfo() with { Damage = dmg, Dodgeable = false, Knockback = 0, DamageSource = PlayerDeathReason.ByCustomReason("If you're seeing this death message, report it through our discord or steam page.") });
                NPC.NewNPC(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetNPC), (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<LizardTail>(), ai0: Player.statLifeMax2 * tailMaxHp, ai1: (int)(tailWait * (1 / (1 + Pet.abilityHaste))), ai2: Pet.timerMax / 2);
                Pet.timer = Pet.timerMax;
                buffTimer = buffDurations;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg))
            {
                if (buffTimer > 0)
                {
                    Player.moveSpeed += tailSpd;
                    Player.SetImmuneTimeForAllTypes(1);
                    Player.aggro -= tailAggro;
                }
                if (Player.statLife < Player.statLifeMax2 * 0.55f)
                {
                    Player.moveSpeed += moveSpd;
                    Player.statDefense += defense;
                    Player.noKnockback = true;
                    Player.wingTimeMax = (int)(jumpMult * Player.wingTimeMax);
                    Player.jumpHeight = (int)(jumpMult * Player.jumpHeight);
                }
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg) && buffTimer > 0)
            {
                Player.runAcceleration *= tailAcc + 1f;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg))
            {
                modifiers.Knockback *= 1f - kbResist;
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
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Lizard lizard = Main.LocalPlayer.GetModPlayer<Lizard>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LizardEgg")
                    .Replace("<class>", PetTextsColors.ClassText(lizard.PetClassPrimary, lizard.PetClassSecondary))
                    .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                    .Replace("<tailDmgTaken>", Math.Round(lizard.percentHpDmg * 100, 2).ToString())
                    .Replace("<tailAcc>", Math.Round(lizard.tailAcc * 100, 2).ToString())
                    .Replace("<tailSpd>", Math.Round(lizard.tailSpd * 100, 2).ToString())
                    .Replace("<tailAggro>", lizard.tailAggro.ToString())
                    .Replace("<buffDuration>", Math.Round(lizard.buffDurations / 60f, 2).ToString())
                    .Replace("<tailWait>", Math.Round(lizard.tailWait / 60f, 2).ToString())
                    .Replace("<tailRecover>", Math.Round(lizard.percentHpRecover * 100, 2).ToString())
                    .Replace("<cdRefund>", Math.Round(lizard.tailCdRefund * 100, 2).ToString())
                    .Replace("<tailMaxHp>", Math.Round(lizard.tailMaxHp * 100, 2).ToString())
                    .Replace("<tailCooldown>", Math.Round(lizard.tailCooldown / 60f, 2).ToString())
                    .Replace("<kbResist>", Math.Round(lizard.kbResist * 100, 2).ToString())
                    .Replace("<def>", lizard.defense.ToString())
                    .Replace("<moveSpd>", Math.Round(lizard.moveSpd * 100, 2).ToString())
                    .Replace("<jumpPenalty>", Math.Round(lizard.jumpMult * 100, 2).ToString())
                ));
        }
    }
}
