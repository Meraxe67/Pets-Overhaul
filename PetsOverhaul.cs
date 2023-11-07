using PetsOverhaul.ModSupport;
using System.IO;
using Terraria.ModLoader;
using PetsOverhaul.Systems;
using PetsOverhaul.PetEffects.Vanilla;
using System;
using Terraria;
using Terraria.ID;

namespace PetsOverhaul
{
    public class PetsOverhaul : Mod
    {
        public enum MessageType : byte
        {
            shieldFullAbsorb,
            seaCreatureOnKill,
            honeyBeeHeal,
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
                case MessageType.honeyBeeHeal:
                    bool bottledHoney = reader.ReadBoolean();
                    int honeyBeeWhoAmI = reader.ReadByte();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)msgType);
                        packet.Write(bottledHoney);
                        packet.Write((byte)honeyBeeWhoAmI);
                        packet.Send(ignoreClient: honeyBeeWhoAmI);
                    }
                    HoneyBee.HealByHoneyBee(bottledHoney, honeyBeeWhoAmI,false);
                    break;




                    default: throw new ArgumentOutOfRangeException(nameof(msgType));
            }
        }
    }
}