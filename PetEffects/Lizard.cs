using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
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
        public float tailAcc = 0.25f;
        public float tailSpd = 0.5f;
        public int tailAggro = -10000;
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
            if (Pet.PetInUse(ItemID.LizardEgg) == false)
            {
                Player.skinColor = Pet.skin;
                Pet.skinColorChanged = false;
            }
            else
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
            if (Pet.timer <= 0 && Pet.PetInUseWithSwapCd(ItemID.LizardEgg) && Keybinds.UsePetAbility.JustPressed)
            {

                NPC.NewNPC(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetNPC),(int)Player.position.X,(int)Player.position.Y,ModContent.NPCType<LizardTail>(),ai0:Player.statLifeMax2 * tailMaxHp, ai1:tailWait, ai2:Pet.timerMax/2);
                Pet.timer = Pet.timerMax;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg))
            {
                if (buffTimer>0)
                {
                    Player.moveSpeed += tailSpd;
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
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LizardEgg))
            {
                modifiers.Knockback *= 1f-kbResist;
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
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Lizard lizard = Main.LocalPlayer.GetModPlayer<Lizard>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LizardEgg")
                .Replace("<class>", PetColors.ClassText(lizard.PetClassPrimary, lizard.PetClassSecondary))
                .Replace("<keybind>", Keybinds.UsePetAbility.GetAssignedKeys(GlobalPet.PlayerInputMode).Count > 0 ? Keybinds.UsePetAbility.GetAssignedKeys(GlobalPet.PlayerInputMode)[0] : $"[c/{Colors.RarityTrash.Hex3()}:{Language.GetTextValue("Mods.PetsOverhaul.KeybindMissing")}]")
                        ));
        }
    }
}
