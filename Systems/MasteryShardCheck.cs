using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    public class MasteryShardCheck : ModSystem
    {
        internal static bool masteryShardObtained1 = false;
        internal static bool masteryShardObtained2 = false;
        internal static bool masteryShardObtained3 = false;
        internal static bool masteryShardObtained4 = false;
        internal static bool masteryShardObtained5 = false;
        public override void OnWorldLoad()
        {
            masteryShardObtained1 = false;
            masteryShardObtained2 = false;
            masteryShardObtained3 = false;
            masteryShardObtained4 = false;
            masteryShardObtained5 = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("masteryshard1", masteryShardObtained1);
            tag.Add("masteryshard2", masteryShardObtained2);
            tag.Add("masteryshard3", masteryShardObtained3);
            tag.Add("masteryshard4", masteryShardObtained4);
            tag.Add("masteryshard5", masteryShardObtained5);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            masteryShardObtained1 = tag.GetBool("masteryshard1");
            masteryShardObtained2 = tag.GetBool("masteryshard2");
            masteryShardObtained3 = tag.GetBool("masteryshard3");
            masteryShardObtained4 = tag.GetBool("masteryshard4");
            masteryShardObtained5 = tag.GetBool("masteryshard5");
        }
    }
}
