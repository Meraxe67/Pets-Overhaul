using PetsOverhaul.ModSupport;
using System.IO;
using Terraria.ModLoader;
using PetsOverhaul.Systems;

namespace PetsOverhaul
{
    public class PetsOverhaul : Mod
    {
        public enum MessageType : byte
        {
            shieldFullAbsorb
        }
        public override void PostSetupContent()
        {
            ModManager.LoadMods();
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            int damage = reader.ReadInt32();
            switch (msgType)
            {
                case MessageType.shieldFullAbsorb:
                    GlobalPet.HandleShieldBlockMessage(reader, whoAmI,damage);
                    break;
            }
        }
    }
}