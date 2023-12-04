using PetsOverhaul.Config;
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
    public sealed class EyeballSpring : ModPlayer
    {
                public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public float acceleration = 0.15f;
        public float jumpBoost = 5f;
        public float ascentPenaltyMult = 0.6f;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EyeSpring))
            {
                if (Player.jump > 0 && Pet.jumpRegistered == false)
                {
                    if (ModContent.GetInstance<Personalization>().HurtSoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.Item56 with { Volume = 0.5f, Pitch = -0.3f, PitchVariance = 0.1f }, Player.position);
                    }

                    Pet.jumpRegistered = true;
                }
                Player.runAcceleration += acceleration;
                Player.jumpSpeedBoost += jumpBoost;
            }
        }
    }
    public sealed class EyeSpring : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EyeSpring;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            EyeballSpring eyeballSpring = Main.LocalPlayer.GetModPlayer<EyeballSpring>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EyeSpring")
                        .Replace("<jumpBoost>", Math.Round(eyeballSpring.jumpBoost * 100, 2).ToString())
                        .Replace("<acceleration>", Math.Round(eyeballSpring.acceleration * 100, 2).ToString())
                        .Replace("<ascNerf>", eyeballSpring.ascentPenaltyMult.ToString())
                        ));
        }
    }
    public sealed class EyeballSpringWing : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (player.TryGetModPlayer(out EyeballSpring eyeballs) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.EyeSpring))
            {
                maxAscentMultiplier *= eyeballs.ascentPenaltyMult;
            }

        }
    }
}
