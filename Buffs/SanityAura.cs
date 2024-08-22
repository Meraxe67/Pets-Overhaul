using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    /// <summary>
    /// This buff doesn't do anything, its only for awareness for recipient players, so that they know they have the buff.
    /// </summary>
    public class SanityAura : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = Lang.GetBuffDescription(ModContent.BuffType<SanityAura>());
            rare = 0;
        }
    }
}
