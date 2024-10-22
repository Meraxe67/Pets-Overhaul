using PetsOverhaul.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class TilePlacement : GlobalTile
    {
        public static void AddToList(int i, int j)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.BlockPlace);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Point16(i, j));
            }
        }
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            AddToList(i, j);
        }
        public static void ReplacedBlockToList(int i, int j)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.BlockReplace);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                ItemPet.updateReplacedTile.Add(new Point16(i, j));
            }
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            ReplacedBlockToList(i,j);

            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
}