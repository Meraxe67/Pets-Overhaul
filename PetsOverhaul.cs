using PetsOverhaul.ModSupport;
using System.IO;
using Terraria.ModLoader;
using PetsOverhaul.Systems;
using PetsOverhaul.PetEffects.Vanilla;
using System;

namespace PetsOverhaul
{
    public class PetsOverhaul : Mod
    {
        public enum MessageType : byte
        {
            shieldFullAbsorb,
            seaCreatureOnKill,
        }
        public override void PostSetupContent()
        {
            ModManager.LoadMods();
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            switch (msgType)
            {
                case MessageType.shieldFullAbsorb:
                    int damage = reader.ReadInt32();
                    GlobalPet.HandleShieldBlockMessage(reader, whoAmI,damage);
                    break;
                case MessageType.seaCreatureOnKill:
                    int npcId = reader.ReadInt32();
                    Junimo.RunSeaCreatureOnKill(reader,whoAmI,npcId);
                    break;
                    default: throw new ArgumentOutOfRangeException(nameof(msgType));
            }
        }
    }
}