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
    public sealed class FennecFox : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float sizeDecrease = 0.85f;
        public float speedIncrease = 0.15f;
        public float meleeSpdIncrease = 0.18f;
        public float meleeDmg = 0.05f;
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ExoticEasternChewToy))
            {
                scale *= sizeDecrease;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ExoticEasternChewToy))
            {
                if (Player.HeldItem.CountsAsClass<MeleeDamageClass>())
                {
                    Player.moveSpeed += speedIncrease;
                    Player.GetAttackSpeed<MeleeDamageClass>() += meleeSpdIncrease;
                }
                Player.GetDamage<MeleeDamageClass>() += meleeDmg;
            }
        }
    }
    public sealed class ExoticEasternChewToy : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ExoticEasternChewToy;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            FennecFox fennecFox = Main.LocalPlayer.GetModPlayer<FennecFox>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ExoticEasternChewToy")
                        .Replace("<meleeSpd>", Math.Round(fennecFox.meleeSpdIncrease * 100, 5).ToString())
                        .Replace("<moveSpd>", Math.Round(fennecFox.speedIncrease * 100, 5).ToString())
                        .Replace("<sizeNerf>", fennecFox.sizeDecrease.ToString())
                        .Replace("<dmg>", Math.Round(fennecFox.meleeDmg * 100, 5).ToString())
                        ));
        }
    }
}
