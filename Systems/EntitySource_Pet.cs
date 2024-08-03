using Terraria.DataStructures;

namespace PetsOverhaul.Systems
{
    public class EntitySource_Pet : IEntitySource
    {
        public string Context { get; set; }
        public EntitySourcePetIDs ContextType { get; set; }
    }
}
