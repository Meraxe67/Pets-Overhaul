using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    public class PetObtainedCondition : ModSystem
    {
        public static bool petIsObtained = false;
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("hasPet", petIsObtained);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("hasPet", out bool pet))
            {
                petIsObtained = pet;
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(petIsObtained);
        }
        public override void NetReceive(BinaryReader reader)
        {
            petIsObtained = reader.ReadBoolean();
        }
    }
}