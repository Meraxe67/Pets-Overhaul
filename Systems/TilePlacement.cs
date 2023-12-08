using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class TilePlacement : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)PetsOverhaul.MessageType.blockPlace);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Point16(i, j));
            }
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)PetsOverhaul.MessageType.blockReplace);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                ItemPet.updateReplacedTile.Add(new Point16(i, j));
            }

            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
}