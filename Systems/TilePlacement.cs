using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class TilePlacement : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Point16(i, j));
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            ItemPet.updateReplacedTile.Add(new Point16(i, j));
            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
}