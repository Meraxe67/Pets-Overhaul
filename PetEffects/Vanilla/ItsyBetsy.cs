using PetsOverhaul.Buffs;
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
    public sealed class ItsyBetsy : ModPlayer
    {
        public int debuffTime = 1200;
        public int maxStacks = 20;
        public float defReduction = 0.02f;
        public float missingHpRecover = 0.004f;

                public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); private set { } }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.GetGlobalNPC<NpcPet>().curseCounter > maxStacks)
            {
                target.GetGlobalNPC<NpcPet>().curseCounter = maxStacks;
            }

            if (target.HasBuff(ModContent.BuffType<QueensDamnation>()))
            {
                modifiers.Defense *= 1f - defReduction * target.GetGlobalNPC<NpcPet>().curseCounter;
            }
            else
            {
                target.GetGlobalNPC<NpcPet>().curseCounter = 0;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.active == false && GlobalPet.LifestealCheck(target) && target.GetGlobalNPC<NpcPet>().curseCounter > 0)
            {
                int mult = 1;
                if (target.GetGlobalNPC<NpcPet>().curseCounter >= maxStacks)
                {
                    mult = 2;
                }

                Pet.Lifesteal(Player.statLifeMax2 - Player.statLife, missingHpRecover * target.GetGlobalNPC<NpcPet>().curseCounter * mult);
            }
            if (Pet.PetInUseWithSwapCd(ItemID.DD2BetsyPetItem) && hit.DamageType == DamageClass.Ranged)
            {
                target.AddBuff(ModContent.BuffType<QueensDamnation>(), debuffTime);
                target.GetGlobalNPC<NpcPet>().curseCounter++;
            }

        }
    }
    public sealed class DD2BetsyPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DD2BetsyPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            ItsyBetsy itsyBetsy = Main.LocalPlayer.GetModPlayer<ItsyBetsy>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DD2BetsyPetItem")
                        .Replace("<debuffTime>", Math.Round(itsyBetsy.debuffTime / 60f, 2).ToString())
                        .Replace("<defDecrease>", Math.Round(itsyBetsy.defReduction * 100, 2).ToString())
                        .Replace("<maxStack>", itsyBetsy.maxStacks.ToString())
                        .Replace("<missingHpSteal>", Math.Round(itsyBetsy.missingHpRecover * 100, 2).ToString())
                        ));
        }
    }
}
