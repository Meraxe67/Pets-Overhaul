using MonoMod.RuntimeDetour;
using PetsOverhaul.ModSupport;
using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
        public static Action<Item, Player> OnPickupActions;
        private delegate bool orig_ItemLoaderOnPickup(Item item, Player player);
        private delegate bool hook_ItemLoaderOnPickup(orig_ItemLoaderOnPickup orig, Item item, Player player);
        private static readonly MethodInfo OnPickupInfo = typeof(ItemLoader).GetMethod("OnPickup");
        List<Hook> hooks = new();
        public override void Load()
        {
            hooks.Add(new(OnPickupInfo, ItemLoaderOnPickupDetour));
            foreach (Hook hook in hooks)
            {
                hook.Apply();
            }

        }
        private bool ItemLoaderOnPickupDetour(orig_ItemLoaderOnPickup orig, Item item, Player player)
        {
            OnPickupActions?.Invoke(item, player);

            return orig(item, player);
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
                    GlobalPet.HandleShieldBlockMessage(reader, whoAmI, damage);
                    break;
                case MessageType.seaCreatureOnKill:
                    int npcId = reader.ReadInt32();
                    Junimo.RunSeaCreatureOnKill(reader, whoAmI, npcId);
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
                    HoneyBee.HealByHoneyBee(bottledHoney, honeyBeeWhoAmI, false);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(msgType));
            }
        }
    }
}