using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    public class PlayerPlacedBlockList : ModSystem
    {
        public static List<Point16> placedBlocksByPlayer = new();
        public override void OnWorldLoad()
        {
            placedBlocksByPlayer.Clear();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("placedBlocksByPlayer", placedBlocksByPlayer);
            if (tag.ContainsKey("placedBlocksbyPlayer"))
            {
                tag.Remove("placedBlocksbyPlayer");
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("placedBlocksByPlayer", out List<Point16> listOfPlacedBlocks))
                placedBlocksByPlayer = listOfPlacedBlocks;
        }
    }
}
