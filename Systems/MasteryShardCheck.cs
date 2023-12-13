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
            if (tag.TryGet("masteryshard1", out bool shard1))
            {
                masteryShardObtained1 = shard1;
            }

            if (tag.TryGet("masteryshard2", out bool shard2))
            {
                masteryShardObtained2 = shard2;
            }

            if (tag.TryGet("masteryshard3", out bool shard3))
            {
                masteryShardObtained3 = shard3;
            }

            if (tag.TryGet("masteryshard4", out bool shard4))
            {
                masteryShardObtained4 = shard4;
            }

            if (tag.TryGet("masteryshard5", out bool shard5))
            {
                masteryShardObtained5 = shard5;
            }
        }
    }
}
