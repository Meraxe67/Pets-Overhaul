using Terraria.ModLoader.Config;

namespace PetsOverhaul.Systems
{
    public enum MessageType : byte
    {
        ShieldFullAbsorb,
        SeaCreatureOnKill,
        HoneyBeeHeal,
        BlockPlace,
        BlockReplace,
        PetSlow,
    }
    public enum EntitySourcePetIDs
    {
        GlobalItem,
        HarvestingItem,
        MiningItem,
        FishingItem,
        HarvestingFortuneItem,
        MiningFortuneItem,
        FishingFortuneItem,
        PetProjectile,
        PetNPC,
        PetMisc,
    }
    public enum PetClasses
    {
        None,
        Melee,
        Ranged,
        Magic,
        Summoner,
        Utility,
        Mobility,
        Harvesting,
        Mining,
        Fishing,
        Offensive,
        Defensive,
        Supportive,
    }
    public enum ShieldPosition
    {
        [LabelKey("$Mods.PetsOverhaul.Config.PlayerLeft")]
        PlayerLeft,
        [LabelKey("$Mods.PetsOverhaul.Config.PlayerRight")]
        PlayerRight,
        [LabelKey("$Mods.PetsOverhaul.Config.HealthBarLeft")]
        HealthBarLeft,
        [LabelKey("$Mods.PetsOverhaul.Config.HealthBarRight")]
        HealthBarRight,
    }
}
