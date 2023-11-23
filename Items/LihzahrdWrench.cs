using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class LihzahrdWrench : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Master;
            Item.value = 0;
            Item.master = true;
            Item.width = 38;
            Item.height = 38;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.DarkOrange.ToVector3() * 0.3f);
        }
    }
}
