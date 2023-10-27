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
    public sealed class VoltBunny : ModPlayer
    {
        public float movespdFlat = 0.05f;
        public float movespdMult = 1.1f;
        public float movespdToDmg = 0.2f;
        public float lightningRod = 0.1f;
        private int lightningRodTime = 0;
        public int lightningRodMax = 300;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LightningCarrot))
            {
                Player.moveSpeed += movespdFlat;
                Player.moveSpeed *= movespdMult;
                Player.GetDamage<GenericDamageClass>() += (Player.moveSpeed - 1f) * movespdToDmg;
                Player.buffImmune[BuffID.Electrified] = false;
                if (lightningRodTime > 0)
                {
                    lightningRodTime--;
                    Player.GetDamage<GenericDamageClass>() += lightningRod; //Ability'i Lightning Rod' dan Static'e çevir
                }
                if (Player.HasBuff(BuffID.Electrified))
                {
                    AdvancedPopupRequest popupMessage = new();
                    popupMessage.Text = "Lightning Rod makes the user immune to Electrified!";
                    popupMessage.DurationInFrames = 120;
                    popupMessage.Velocity = new Vector2(0, -7);
                    popupMessage.Color = Color.LightGoldenrodYellow;
                    PopupText.NewText(popupMessage, Player.position);
                    Player.ClearBuff(BuffID.Electrified);
                    lightningRodTime = lightningRodMax;
                }
            }
        }
    }
    public sealed class LightningCarrot : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LightningCarrot;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            VoltBunny voltBunny = Main.LocalPlayer.GetModPlayer<VoltBunny>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LightningCarrot")
                       .Replace("<flatSpd>", Math.Round(voltBunny.movespdFlat * 100, 5).ToString())
                       .Replace("<multSpd>", voltBunny.movespdMult.ToString())
                       .Replace("<spdToDmg>", Math.Round(voltBunny.movespdToDmg * 100, 5).ToString())
                       .Replace("<electricRod>", Math.Round(voltBunny.lightningRod * 100, 5).ToString())
                       .Replace("<electricRodDuration>", Math.Round(voltBunny.lightningRodMax / 60f, 5).ToString())
                       ));
        }
    }
}
