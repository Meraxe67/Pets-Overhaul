using MonoMod.RuntimeDetour;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul
{
    public class PetsOverhaul : Mod
    {
        public static Action<Item, Player> OnPickupActions;
        private delegate bool orig_ItemLoaderOnPickup(Item item, Player player);
        private delegate bool hook_ItemLoaderOnPickup(orig_ItemLoaderOnPickup orig, Item item, Player player);
        private static readonly MethodInfo OnPickupInfo = typeof(ItemLoader).GetMethod("OnPickup");
        private readonly List<Hook> hooks = new();
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
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            switch (msgType)
            {
                case MessageType.ShieldFullAbsorb:
                    int damage = reader.ReadInt32();
                    GlobalPet.HandleShieldBlockMessage(reader, whoAmI, damage);
                    break;
                case MessageType.SeaCreatureOnKill:
                    int npcId = reader.ReadInt32();
                    Junimo.RunSeaCreatureOnKill(reader, whoAmI, npcId);
                    break;
                case MessageType.HoneyBeeHeal:
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
                case MessageType.BlockPlace:
                    int xPlace = reader.ReadInt32();
                    int yPlace = reader.ReadInt32();
                    PlayerPlacedBlockList.placedBlocksByPlayer.Add(new Point16(xPlace, yPlace));
                    break;
                case MessageType.BlockReplace:
                    int xReplace = reader.ReadInt32();
                    int yReplace = reader.ReadInt32();
                    ItemPet.updateReplacedTile.Add(new Point16(xReplace, yReplace));
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(msgType));
            }
        }
    }
}