using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    public class PlayerPlacedBlockList : ModSystem
    {
        internal static List<Vector2> placedBlocksByPlayer = new List<Vector2>();
        public override void SetStaticDefaults() //DÜNYALAR ARASI DEĞERLER AYNI KALIYOR VE UPDATE'LENDİĞİNDE VS. RESETLENİYOR
        {
            placedBlocksByPlayer.Capacity = Main.maxTilesX * Main.maxTilesY;
        }
        public override void OnWorldLoad()
        {
            placedBlocksByPlayer.Clear();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("placedBlocksbyPlayer", placedBlocksByPlayer);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            placedBlocksByPlayer = tag.Get<List<Vector2>>("placedBlocksbyPlayer");
        }
    }
}
