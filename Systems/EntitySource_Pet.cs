using System;
using Terraria.DataStructures;

namespace PetsOverhaul.Systems
{
    public class EntitySource_Pet : IEntitySource
    {
        public enum TypeId
        {
            globalItem = 0, harvestingItem = 1, miningItem = 2, fishingItem = 3, harvestingFortuneItem = 4, miningFortuneItem = 5, fishingFortuneItem = 6, petProjectile = 7
        }
        public string Context { get; set; }
        public TypeId ContextType { get; set; }
    }
}
