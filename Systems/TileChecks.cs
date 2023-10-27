using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class TileChecks : GlobalTile
    {
        /// <summary>
        /// Includes tiles that are considered 'soil' tiles, except Dirt. Used by Dirtiest Block.
        /// </summary>
        public static bool[] commonTiles = TileID.Sets.Factory.CreateBoolSet(false, TileID.Mud, TileID.SnowBlock, TileID.Ash, TileID.ClayBlock, TileID.Marble, TileID.Granite, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone, TileID.Sand, TileID.Ebonsand, TileID.Crimsand, TileID.Pearlsand, TileID.CorruptSandstone, TileID.Sandstone, TileID.CrimsonSandstone, TileID.HallowSandstone, TileID.HardenedSand, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand, TileID.HallowHardenedSand, TileID.IceBlock, TileID.CorruptIce, TileID.FleshIce, TileID.HallowedIce);
        /// <summary>
        /// Includes Gem tiles.
        /// </summary>
        public static bool[] gemTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.AmberStoneBlock, TileID.Diamond, TileID.ExposedGems, TileID.Crystals);
        /// <summary>
        /// Includes tiles that are extractable by an Extractinator and a few other stuff that aren't recognized as ores such as Obsidian and Luminite
        /// </summary>
        public static bool[] extractableAndOthers = TileID.Sets.Factory.CreateBoolSet(false, TileID.DesertFossil, TileID.Slush, TileID.Silt, TileID.Obsidian, TileID.LunarOre, TileID.DesertFossil);
        /// <summary>
        /// Includes tree tiles. Is used by Blue Chicken.
        /// </summary>
        public static bool[] treeTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeAsh, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.Trees, TileID.TreeSapphire, TileID.TreeTopaz, TileID.MushroomTrees, TileID.PalmTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow);
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