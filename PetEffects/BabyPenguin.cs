using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyPenguin : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Fishing;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        internal int penguinOldChilledTime = 0;
        public int snowFish = 25;
        public int oceanFish = 15;
        public int regularFish = 5;
        public float chillingMultiplier = 0.45f;
        public int snowFishChance = 100;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(ItemID.Fish))
            {
                if (Player.ZoneSnow)
                {
                    Player.fishingSkill += snowFish;
                    Player.accFlipper = true;
                }
                else if (Player.ZoneBeach)
                {
                    Player.fishingSkill += oceanFish;
                    Player.accFlipper = true;
                }
                else
                {
                    Player.fishingSkill += regularFish;
                }
            }

            if (Pet.PetInUseWithSwapCd(ItemID.Fish) && Player.HasBuff(BuffID.Chilled))
            {
                if (Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] > penguinOldChilledTime)
                {
                    Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] -= (int)(Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)] * chillingMultiplier);
                }
                penguinOldChilledTime = Player.buffTime[Player.FindBuffIndex(BuffID.Chilled)];
            }


        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (Pet.PetInUse(ItemID.Fish) && (fish.type == ItemID.FrostMinnow || fish.type == ItemID.AtlanticCod || fish.type == ItemID.FrostDaggerfish || fish.type == ItemID.FrozenCrate || fish.type == ItemID.FrozenCrateHard))
            {
                for (int i = 0; i < GlobalPet.Randomizer(snowFishChance * fish.stack); i++)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.FishingItem), fish.type, 1);
                }
            }
        }
    }
    public sealed class Fish : PetTooltip
    {
        public override PetEffect PetsEffect => babyPenguin;
        public static BabyPenguin babyPenguin
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabyPenguin pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabyPenguin>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Fish")
                .Replace("<fp>", babyPenguin.regularFish.ToString())
                .Replace("<oceanFp>", babyPenguin.oceanFish.ToString())
                .Replace("<snowFp>", babyPenguin.snowFish.ToString())
                .Replace("<catchChance>", babyPenguin.snowFishChance.ToString())
                .Replace("<chilledMult>", babyPenguin.chillingMultiplier.ToString());
    }
}
