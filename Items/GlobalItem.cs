using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    /// <summary>
    /// GlobalItem class that contains many useful booleans for mainly gathering purposes. This class is bread & butter of all Gathering Pets.
    /// </summary>
    public sealed class ItemPet : GlobalItem
    {
        public override bool InstancePerEntity => true;
        /// <summary>
        /// Includes tiles that are considered 'soil' tiles, except Dirt. Used by Dirtiest Block.
        /// </summary>
        public static bool[] commonTiles = TileID.Sets.Factory.CreateBoolSet(false, TileID.Mud, TileID.SnowBlock, TileID.Ash, TileID.ClayBlock, TileID.Marble, TileID.Granite, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone, TileID.Sand, TileID.Ebonsand, TileID.Crimsand, TileID.Pearlsand, TileID.CorruptSandstone, TileID.Sandstone, TileID.CrimsonSandstone, TileID.HallowSandstone, TileID.HardenedSand, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand, TileID.HallowHardenedSand, TileID.IceBlock, TileID.CorruptIce, TileID.FleshIce, TileID.HallowedIce, TileID.Stone, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone);
        /// <summary>
        /// Includes Gem tiles.
        /// </summary>
        public static bool[] gemTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.AmberStoneBlock, TileID.Diamond, TileID.ExposedGems, TileID.Crystals);
        /// <summary>
        /// Includes tiles that are extractable by an Extractinator and a few other stuff that aren't recognized as ores such as Obsidian and Luminite
        /// </summary>
        public static bool[] extractableAndOthers = TileID.Sets.Factory.CreateBoolSet(false, TileID.DesertFossil, TileID.Slush, TileID.Silt, TileID.Obsidian, TileID.LunarOre);
        /// <summary>
        /// Includes tiles that counts as trees.
        /// </summary>
        public static bool[] treeTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeAsh, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.Trees, TileID.TreeSapphire, TileID.TreeTopaz, TileID.MushroomTrees, TileID.PalmTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow, TileID.Bamboo, TileID.Cactus);
        /// <summary>
        /// Contains items dropped by gemstone trees. Current only use is Caveling Gardener.
        /// </summary>
        public static bool[] gemstoneTreeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.GemTreeAmberSeed, ItemID.GemTreeAmethystSeed, ItemID.GemTreeDiamondSeed, ItemID.GemTreeEmeraldSeed, ItemID.GemTreeRubySeed, ItemID.GemTreeSapphireSeed, ItemID.GemTreeTopazSeed, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond, ItemID.StoneBlock);
        /// <summary>
        /// Contains items dropped by trees. Current only use is Blue Chicken.
        /// </summary>
        public static bool[] treeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Acorn, ItemID.BambooBlock, ItemID.Cactus, ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.PalmWood, ItemID.Ebonwood, ItemID.Shadewood, ItemID.RichMahogany, ItemID.Pearlwood, ItemID.SpookyWood);
        /// <summary>
        /// Contains items dropped by trees. Current only use is Blue Chicken.
        /// </summary>
        public static bool[] seaPlantItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemID.LightningWhelkShell, ItemID.TulipShell, ItemID.JunoniaShell);
        /// <summary>
        /// Adds coordinates in this to PlayerPlacedBlockList if a tile has been replaced and item is obtained through that way. In GlobalTile, Add to this in the CanReplace() hook.
        /// </summary>
        public static List<Point16> updateReplacedTile = new();

        //Checks to determine which Pet should benefit
        public bool itemFromNpc = false;
        public bool herbBoost = false;
        public bool oreBoost = false;
        public bool commonBlock = false;
        public bool blockNotByPlayer = false;
        public bool pickedUpBefore = false;
        public bool itemFromBag = false;
        public bool itemFromBoss = false;
        public bool harvestingDrop = false;
        public bool miningDrop = false;
        public bool fishingDrop = false;
        public bool globalDrop = false;
        public bool fortuneHarvestingDrop = false;
        public bool fortuneMiningDrop = false;
        public bool fortuneFishingDrop = false;

        /// <summary>
        /// 1000 is 10 exp.
        /// </summary>
        public const int MinimumExpForRarePlant = 1000;

        public override void UpdateInventory(Item item, Player player)
        {
            if (pickedUpBefore == false)
            {
                pickedUpBefore = true;
            }
        }
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (WorldGen.generatingWorld)
            {
                return;
            }

            if (source is EntitySource_Pet petSource)
            {
                globalDrop = petSource.ContextType == EntitySourcePetIDs.GlobalItem;

                harvestingDrop = petSource.ContextType == EntitySourcePetIDs.HarvestingItem;

                miningDrop = petSource.ContextType == EntitySourcePetIDs.MiningItem;

                fishingDrop = petSource.ContextType == EntitySourcePetIDs.FishingItem;

                fortuneHarvestingDrop = petSource.ContextType == EntitySourcePetIDs.HarvestingFortuneItem;

                fortuneMiningDrop = petSource.ContextType == EntitySourcePetIDs.MiningFortuneItem;

                fortuneFishingDrop = petSource.ContextType == EntitySourcePetIDs.FishingFortuneItem;
            }
            else if (source is EntitySource_TileBreak || source is EntitySource_ShakeTree)
            {
                herbBoost = Junimo.HarvestingXpPerGathered.Exists(x => x.plantList.Contains(item.type));

                if (source is EntitySource_TileBreak brokenTile)
                {
                    ushort tileType = Main.tile[brokenTile.TileCoords].TileType;

                    if (PlayerPlacedBlockList.placedBlocksByPlayer.Remove(new Point16(brokenTile.TileCoords)) == false)
                    {
                        oreBoost = TileID.Sets.Ore[tileType] || gemTile[tileType] || extractableAndOthers[tileType] || item.type == ItemID.LifeCrystal;
                        commonBlock = TileID.Sets.Conversion.Moss[tileType] || commonTiles[tileType];
                        blockNotByPlayer = true;
                    }

                    if (TileID.Sets.CountsAsGemTree[tileType] == false && gemstoneTreeItem[item.type] || treeTile[tileType] == false && treeItem[item.type] || blockNotByPlayer == false && seaPlantItem[item.type])
                    {
                        herbBoost = false;
                    }

                    if (updateReplacedTile.Count > 0)
                    {
                        PlayerPlacedBlockList.placedBlocksByPlayer.AddRange(updateReplacedTile);
                    }
                }
            }
            else if (source is EntitySource_Loot lootSource && lootSource.Entity is NPC npc)
            {
                if (npc.boss == true || NpcPet.nonBossTrueBosses.Contains(npc.type))
                {
                    itemFromBoss = true;
                }
                else
                {
                    itemFromNpc = true;
                }
            }
            else if (source is EntitySource_ItemOpen)
            {
                itemFromBag = true;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            BitsByte sources1 = new(blockNotByPlayer, pickedUpBefore, itemFromNpc, itemFromBoss, itemFromBag, herbBoost, oreBoost, commonBlock);
            BitsByte sources2 = new(globalDrop, harvestingDrop, miningDrop, fishingDrop, fortuneHarvestingDrop, fortuneMiningDrop, fortuneFishingDrop);
            writer.Write(sources1);
            writer.Write(sources2);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            BitsByte sources1 = reader.ReadByte();
            sources1.Retrieve(ref blockNotByPlayer, ref pickedUpBefore, ref itemFromNpc, ref itemFromBoss, ref itemFromBag, ref herbBoost, ref oreBoost, ref commonBlock);
            BitsByte sources2 = reader.ReadByte();
            sources2.Retrieve(ref globalDrop, ref harvestingDrop, ref miningDrop, ref fishingDrop, ref fortuneHarvestingDrop, ref fortuneMiningDrop, ref fortuneFishingDrop);
        }
    }
}
