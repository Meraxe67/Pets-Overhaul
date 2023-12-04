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
    public sealed class CarrotBunny : ModPlayer
    {
                public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); private set { } }
        private int bunnyTimer = 0;
        public int bunnyStack = 0;
        public float jumpPerStk = 0.02f;
        public float spdPerStk = 0.005f;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Carrot))
            {
                bunnyTimer--;
                if (Player.jump < Player.jumpHeight / 2 && Player.jump != 0 && Pet.jumpRegistered == false)
                {
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.Item56 with { Volume = 0.5f, Pitch = 0.2f, PitchVariance = 0.3f }, Player.position);
                    }

                    bunnyStack++;
                    bunnyTimer = 240;
                    Pet.jumpRegistered = true;
                }
                if (bunnyStack > 10)
                {
                    bunnyStack = 10;
                }
                if (bunnyTimer <= 0)
                {
                    bunnyStack = 0;
                }
                Player.jumpSpeedBoost += bunnyStack * jumpPerStk;
                Player.moveSpeed += bunnyStack * spdPerStk;
            }
        }
    }
    public sealed class Carrot : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Carrot;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            CarrotBunny carrotBunny = Main.LocalPlayer.GetModPlayer<CarrotBunny>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Carrot")
                .Replace("<moveSpeed>", Math.Round(carrotBunny.spdPerStk * 100, 2).ToString())
                .Replace("<jumpSpeed>", Math.Round(carrotBunny.jumpPerStk * 100, 2).ToString())
            ));
        }
    }
}
