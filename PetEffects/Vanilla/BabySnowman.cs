﻿using PetsOverhaul.Config;
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
    public sealed class BabySnowman : ModPlayer
    {
        public int frostburnTime = 300;
        public float snowmanSlow = 0.2f;
        public int slowTime = 180;
        public int FrostArmorMult => Player.armor[0].type == ItemID.FrostHelmet && Player.armor[1].type == ItemID.FrostBreastplate && Player.armor[2].type == ItemID.FrostLeggings ? 3 : 1;

        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ToySled))
            {
                target.AddBuff(BuffID.Frostburn2, frostburnTime * FrostArmorMult);
                target.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.Snowman, snowmanSlow * FrostArmorMult, slowTime * FrostArmorMult, target);
            }
        }

    }
    public sealed class ToySled : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ToySled;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabySnowman babySnowman = Main.LocalPlayer.GetModPlayer<BabySnowman>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ToySled")
                .Replace("<frostburnTime>", Math.Round(babySnowman.frostburnTime / 60f * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<slowAmount>", Math.Round(babySnowman.snowmanSlow * 100 * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<slowTime>", Math.Round(babySnowman.slowTime / 60f * babySnowman.FrostArmorMult, 2).ToString())
            ));
        }
    }
}
