using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class ItsyBetsy : PetEffect
    {
        public int debuffTime = 720;
        public int maxStacks = 20;
        public float defReduction = 0.02f;
        public float missingHpRecover = 0.007f;
        public float maxStackBonusRecover = 0.5f;

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
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
                Pet.PetRecovery(Player.statLifeMax2 - Player.statLife, missingHpRecover * target.GetGlobalNPC<NpcPet>().curseCounter * (1f + (target.GetGlobalNPC<NpcPet>().curseCounter >= maxStacks ? maxStackBonusRecover : 0)));
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
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            ItsyBetsy itsyBetsy = Main.LocalPlayer.GetModPlayer<ItsyBetsy>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DD2BetsyPetItem")
                .Replace("<class>", PetTextsColors.ClassText(itsyBetsy.PetClassPrimary, itsyBetsy.PetClassSecondary))
                        .Replace("<debuffTime>", Math.Round(itsyBetsy.debuffTime / 60f, 2).ToString())
                        .Replace("<defDecrease>", Math.Round(itsyBetsy.defReduction * 100, 2).ToString())
                        .Replace("<maxStack>", itsyBetsy.maxStacks.ToString())
                        .Replace("<missingHpSteal>", Math.Round(itsyBetsy.missingHpRecover * 100, 2).ToString())
                        .Replace("<maxStackIncr>", Math.Round(itsyBetsy.maxStackBonusRecover * 100, 2).ToString())
                        ));
        }
    }
}
