using PetsOverhaul.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class TilePlacement : GlobalTile
    {
        public static bool RemoveFromList(int i, int j)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                packet.Write((byte)MessageType.BlockRemove);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
            else
            {
                GlobalPet.CoordsToRemove.Add(new Point16(i, j));
                return PlayerPlacedBlockList.placedBlocksByPlayer.Contains(new Point16(i, j)); //ItemPet checks for if it was removed, and OnSpawn is called on Server so this one works there.
            }
            return false;
        }
        public static void AddToList(int i, int j)
        {
            if (PlayerPlacedBlockList.placedBlocksByPlayer.Contains(new Point16(i, j)))
            {
                return;
            }

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
                GlobalPet.updateReplacedTile.Add(new Point16(i, j));
            }
        }
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            AddToList(i, j);
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            ReplacedBlockToList(i, j);

            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
    }
}