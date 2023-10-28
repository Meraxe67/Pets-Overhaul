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
    public sealed class CompanionCube : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float manaToHealth = 0.11f;
        /// <summary>
        /// This is base amount for mana to health recovery before the Potion Sickness reductions.
        /// </summary>
        public float manaToHealthUpdate = 0.11f;
        public float healthToMana = 0.2f;
        public float manaPotionNerf = 0.17f;
        public float manaToHealthNerf = 0.03f;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.CompanionCube))
            {
                manaToHealth = manaToHealthUpdate;
            }
        }
        public override void PostUpdateBuffs()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.CompanionCube) && Player.manaSick)
            {
                if (Player.manaSickReduction > Player.manaSickLessDmg)
                {
                    manaToHealth -= manaToHealthNerf * 2;
                }
                else
                {
                    manaToHealth -= manaToHealthNerf;
                }
            }
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {


            if (Pet.PetInUseWithSwapCd(ItemID.CompanionCube) && Player.manaSick)
            {
                if (Player.manaSickReduction > Player.manaSickLessDmg)
                {
                    healValue -= (int)(healValue * manaPotionNerf * 2);
                }
                else
                {
                    healValue -= (int)(healValue * manaPotionNerf);
                }
            }
        }
        public override void OnConsumeMana(Item item, int manaConsumed)
        {

            if (Pet.PetInUseWithSwapCd(ItemID.CompanionCube))
            {
                Pet.Lifesteal(manaConsumed, manaToHealth, respectLifeStealCap: false);
            }
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.CompanionCube))
            {
                Pet.Lifesteal(info.Damage, healthToMana, manaSteal: true, respectLifeStealCap: false);
            }
        }
    }
    public sealed class CompanionCubeItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CompanionCube;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            CompanionCube companionCube = Main.LocalPlayer.GetModPlayer<CompanionCube>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.CompanionCube")
                        .Replace("<manaToHealth>", Math.Round(companionCube.manaToHealth * 100, 2).ToString())
                        .Replace("<healthToMana>", Math.Round(companionCube.healthToMana * 100, 2).ToString())
                        .Replace("<manaPotionNerf>", Math.Round(companionCube.manaPotionNerf * 100, 2).ToString())
                        .Replace("<manaToHealthNerf>", Math.Round(companionCube.manaToHealthNerf * 100, 2).ToString())
                        .Replace("<halfOfSickness>", Math.Round(Player.manaSickLessDmg * 100, 2).ToString())
                        ));
        }
    }
}
