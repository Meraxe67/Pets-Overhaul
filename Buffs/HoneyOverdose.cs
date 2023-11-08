using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace PetsOverhaul.Buffs
{
    public sealed class HoneyOverdose : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = 0;
            tip = Lang.GetBuffDescription(ModContent.BuffType<HoneyOverdose>())
                .Replace("<AbilityHaste>", Math.Round(Main.LocalPlayer.GetModPlayer<HoneyBee>().currentAbilityHasteBonus * 100,2).ToString());
        }
    }
}
