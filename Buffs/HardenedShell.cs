using PetsOverhaul.PetEffects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    /// <summary>
    /// This buff is for display & awareness only, has nothing to do with the effects itselves.
    /// </summary>
    public sealed class HardenedShell : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.miscEquips[0].type != ItemID.Seaweed || player.GetModPlayer<Turtle>().currentStacks <= 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = 0;
            tip = Lang.GetBuffDescription(ModContent.BuffType<HardenedShell>())
                .Replace("<hit>", Main.LocalPlayer.GetModPlayer<Turtle>().currentStacks.ToString());
        }
    }
}
