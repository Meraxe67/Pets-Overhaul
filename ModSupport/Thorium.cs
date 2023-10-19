using System;
using System.Collections.Generic;
using System.Linq;

using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;

using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.ModSupport
{
    public class ThoriumSupport
    {
        public string InternalModName = "ThoriumMod";
        public string[] InternalModdedItemNames = new string[]
        {
            "AbyssalWhistle", // Abyssal Bunny
            "AlienResearchNotes", // Research Probe
            "AncientCheeseBlock",// Ancient Rat
            "AncientDrachma", // Curious Coinling
            "AromaticBiscuit", // Skunk
            "BalloonBall", // Tanuki Girl
            "BioPod", // Bio-Feeder
            "BlisterSack", // Flying Blister
            "BloodSausage", // Lazy Bat
            "ChaoticMarble", // Chaotic Pet
            "CloudyChewToy", // Wyvern Pup
            "DelectableNut", // Lil' Mog
            "DiverPlushie", // Princess Jellyfish
            "DoomSayersPenny", // Mini Primordials
            "EnergizedQuadCube", // Energized Quad-Cube
            "ExoticMynaEgg", // Exotic Myna
            "Experiment3", // Experiment #3
            "FishEgg", // Clownfish
            "ForgottenLetter", // Lost Snowy Owl
            "FragmentedRune", // Ferret
            "FreshPickle", // Normal Dog
            "FrozenBalloon", // Frozen Balloon
            "GlassShard", // Glass Shard
            "GreenFirefly", // Amnesiac
            "GuildsStaff", // Lil' Necromancer
            "ModelGun", // Lyrist
            "MoleCrate", // Skull Cat
            "PearTreeSapling", // Partridge
            "PinkSlimeEgg", // Pink Slime
            "PurifiersRing", // Purifier's Ring
            "RottenMeat", // Fly
            "SimpleBroom", // Lil' Maid
            "StormCloud", // Storm Cloud
            "SubterraneanBulb", // Subterranean Angler
            "SuspiciousMoisturizerBottle", // Living Hand
            "SweetBeet", // Beet Cookie
            "SwordOfDestiny", // Sword Of Destiny
            "TortleScute", // Tortle Sage
            "WhisperingShell", // Creature
        };
        //If these arent defined, they will be skipped

        public List<(int, int[])> MiningXpPerModdedBlock;
        public List<(int, int[])> FishingXpPerModdedFish;
        public List<(int, int[])> FishingXpPerModdedKill;
        public List<(int, int[])> HarvestingXpPerModdedPlant;
        public Mod ModInstance;
        public Dictionary<string, int> InternalNameToModdedItemId = new Dictionary<string, int> { };
        public Dictionary<string, ModItem> InternalNameToModdedItemInstance = new Dictionary<string, ModItem> { };
        public void InitializeMod()
        {
            if (!ModLoader.TryGetMod(InternalModName, out ModInstance)) return;
            MergePetItems();
            MergeJunimoExp();
        }

        public void MergePetItems()
        {
            if (InternalModdedItemNames == null) return;
            foreach (string internalName in InternalModdedItemNames)
            {
                ModItem item;
                ModInstance.TryFind(internalName, out item);
                //Console.WriteLine($"IN: {internalName}\n Type: {item.Type}");

                ModContent.GetInstance<PetRegistry>().TerrariaPetItemIds.TryAdd(internalName, item.Type);
                InternalNameToModdedItemId.TryAdd(internalName, item.Type);
            };
        }

        public void MergeJunimoExp()
        {
            if (MiningXpPerModdedBlock != null) Main.LocalPlayer.GetModPlayer<Junimo>().MiningXpPerBlock.AddRange(MiningXpPerModdedBlock);
            if (HarvestingXpPerModdedPlant != null) Main.LocalPlayer.GetModPlayer<Junimo>().HarvestingXpPerGathered.AddRange(HarvestingXpPerModdedPlant);
            if (FishingXpPerModdedFish != null) Main.LocalPlayer.GetModPlayer<Junimo>().FishingXpPerCaught.AddRange(FishingXpPerModdedFish);
            if (FishingXpPerModdedKill != null) Main.LocalPlayer.GetModPlayer<Junimo>().FishingXpPerKill.AddRange(FishingXpPerModdedKill);
        }

        public bool IsModLoaded()
        {
            return ModInstance != null;
        }
        public bool GetModInstance(out Mod instance)
        {
            if (!IsModLoaded())
            {
                instance = null;
                return false;
            }
            instance = ModInstance;
            return true;
        }

        public bool GetItemInstance(string InternalName, out ModItem item)
        {
            if (!InternalNameToModdedItemId.ContainsKey(InternalName))
            {
                item = null;
                return false;
            }

            item = InternalNameToModdedItemInstance[InternalName];
            return true;
        }
    }
}