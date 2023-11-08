using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
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
            tag.Add("placedBlocksbyPlayer", placedBlocksByPlayer);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("placedBlocksbyPlayer"))
            {
                object MyKeyData = tag["placedBlocksbyPlayer"];

                if (MyKeyData is List<Vector2> oldList)
                {
                    placedBlocksByPlayer = oldList.ConvertAll(x => x.ToPoint16());
                }

                if (MyKeyData is List<Point16> correct)
                {
                    placedBlocksByPlayer = correct;
                }
            }
        }
    }
}
