using PetsOverhaul.PetEffects;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public sealed class EocPetEnrage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.miscEquips[0].type != ItemID.EyeOfCthulhuPetItem)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            SuspiciousEye sus = Main.LocalPlayer.GetModPlayer<SuspiciousEye>();
            rare = 0;
            tip = Lang.GetBuffDescription(ModContent.BuffType<EocPetEnrage>())
                .Replace("<rage>", sus.ragePoints.ToString())
                .Replace("<dmg>", Math.Round(sus.dmgMult * sus.ragePoints, 2).ToString())
                .Replace("<speed>", Math.Round(sus.spdMult * sus.ragePoints, 2).ToString())
                .Replace("<crit>", Math.Round(sus.critMult * sus.ragePoints, 2).ToString());
        }
    }
}
