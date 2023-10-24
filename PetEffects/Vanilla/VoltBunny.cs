using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;

using PetsOverhaul.Config;
using Terraria.GameInput;

namespace PetsOverhaul.PetEffects.Vanilla
{
    sealed public class VoltBunny : ModPlayer
    {
        public float movespdFlat = 0.05f;
        public float movespdMult = 1.1f;
        public float movespdToDmg = 0.2f;
        public float lightningRod = 0.1f;
        private int lightningRodTime = 0;
        public int lightningRodMax = 300;
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
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
    sealed public class LightningCarrot : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.LightningCarrot;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            VoltBunny voltBunny = Main.LocalPlayer.GetModPlayer<VoltBunny>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LightningCarrot")
                       .Replace("<flatSpd>", (voltBunny.movespdFlat * 100).ToString())
                       .Replace("<multSpd>", voltBunny.movespdMult.ToString())
                       .Replace("<spdToDmg>", (voltBunny.movespdToDmg * 100).ToString())
                       .Replace("<electricRod>", (voltBunny.lightningRod * 100).ToString())
                       .Replace("<electricRodDuration>", (voltBunny.lightningRodMax / 60f).ToString())
                       ));
        }
    }
}
