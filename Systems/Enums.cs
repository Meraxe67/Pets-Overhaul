namespace PetsOverhaul.Systems
{
    public enum MessageType : byte
    {
        ShieldFullAbsorb,
        SeaCreatureOnKill,
        HoneyBeeHeal,
        BlockPlace,
        BlockReplace,
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
    public enum PetSlowIDs
    {
        Grinch,
        Snowman, 
        QueenSlime, 
        Deerclops, 
        IceQueen, 
        PikachuStatic, 
        PhantasmalIce, 
        Misc,
    }
}
