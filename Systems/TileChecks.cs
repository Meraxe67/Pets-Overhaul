using Microsoft.Xna.Framework;
using PetsOverhaul.PetEffects;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
namespace PetsOverhaul.Systems
{
    public class TileChecks : GlobalTile
    {
        public static bool[] commonTiles = TileID.Sets.Factory.CreateBoolSet(false, TileID.Mud, TileID.SnowBlock, TileID.Ash, TileID.ClayBlock, TileID.Marble, TileID.Granite, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone, TileID.Sand, TileID.Ebonsand, TileID.Crimsand, TileID.Pearlsand, TileID.CorruptSandstone, TileID.Sandstone, TileID.CrimsonSandstone, TileID.HallowSandstone, TileID.HardenedSand, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand, TileID.HallowHardenedSand, TileID.IceBlock, TileID.CorruptIce, TileID.FleshIce, TileID.HallowedIce);
        /// <summary>
        /// Includes tiles that are choppable by using an Axe or Chainsaws.
        /// </summary>
        public static bool[] choppableTiles = TileID.Sets.Factory.CreateBoolSet(false, TileID.Trees, TileID.Cactus, TileID.MushroomTrees, TileID.PalmTree, TileID.TreeAsh, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow, TileID.Bamboo, TileID.TreeAmber,TileID.TreeAmethyst,TileID.TreeDiamond,TileID.TreeEmerald,TileID.TreeRuby,TileID.TreeTopaz);
        /// <summary>
        /// Includes Gem tiles.
        /// </summary>
        public static bool[] gemTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.AmberStoneBlock, TileID.Diamond, TileID.ExposedGems);
        /// <summary>
        /// Includes tiles that are extractable by an Extractinator and a few other stuff that aren't recognized as ores such as Obsidian and Luminite
        /// </summary>
        public static bool[] extractableAndOthers = TileID.Sets.Factory.CreateBoolSet(false, TileID.DesertFossil, TileID.Slush, TileID.Silt, TileID.Obsidian, TileID.LunarOre, TileID.DesertFossil);
        /// <summary>
        /// Includes plants that grows rarely 
        /// </summary>
        public static bool[] rarePlantTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.DyePlants, TileID.LifeFruit);
        /// <summary>
        /// Includes plants affected by harvesting bonuses
        /// </summary>
        public static bool[] commonPlantTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.BloomingHerbs, TileID.MatureHerbs, TileID.Pumpkins, TileID.MushroomPlants);
        /// <summary>
        /// Includes plant tiles that have different variations in tile spritesheet
        /// </summary>
        public static List<(ushort tileType, int spriteFrame)> plantTilesWithFrames = new List<(ushort, int)> {
            {(TileID.JunglePlants,8)},
            {(TileID.Plants,8)},
            {(TileID.CorruptPlants,8)},
            {(TileID.CrimsonPlants,15)}
        };
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Vector2(i, j));
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            ItemPet.updateReplacedTile.Add(new Vector2(i, j));
            return true;
        }
    }
}