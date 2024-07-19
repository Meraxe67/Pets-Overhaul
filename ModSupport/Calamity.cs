using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace PetsOverhaul.ModSupport
{
    public class CalamitySupport
    {
        public string InternalModName = "CalamityMod";
        public string[] InternalModdedItemNames = new string[]
        {
            "ForgottenDragonEgg", //Akato
            "AstrophageItem", //Astrophage
            "BearsEye", //Bear
            "CharredRelic", //Brimling
            "CosmicPlushie", //Chibii Devourer
            "DaawnlightSpiritOrigin", //Daawnlight
            "TrashmanTrashcan", //DannyDevito
            "TheEtomer", //ElectricTroublemaker
            "AbyssShellFossil", //EscargidolonSnail
            "GeyserShell", //FlakHermit
            "FoxDrive", //FoxPet
            "PrimroseKeepsake", //FurtasticDuo
            "RomajedaOrchid", //Kendra
            "JoyfulHeart", //LadShark
            "Levi", //Levi
            "RottingEyeball", //MiniHiveMind
            "BloodyVein", //MiniPerforator
            "Pineapple", //Pineapple
            "PlagueCaller", //PlaguebringerBab
            "McNuggets", //SonOfYharon
            "BrimstoneJewel", //SupremeCalamitas
            "HermitsBoxofOneHundredMedicines", //ThirdSage
            "HowlsHeart", //TurnipHead
        };
        //If these arent defined, they will be skipped

        public List<(int, int[])> MiningXpPerModdedBlock;
        public List<(int, int[])> FishingXpPerModdedFish;
        public List<(int, int[])> FishingXpPerModdedKill;
        public List<(int, int[])> HarvestingXpPerModdedPlant;
        public Mod CalamityInstance;
        public Dictionary<string, int> InternalNameToModdedItemId = new Dictionary<string, int> { };
        public Dictionary<string, ModItem> InternalNameToModdedItemInstance = new Dictionary<string, ModItem> { };
        public void InitializeMod()
        {
            if (!ModLoader.TryGetMod(InternalModName, out CalamityInstance))
            {
                return;
            }

            MergePetItems();
            MergeJunimoExp();
        }

        public void MergePetItems()
        {
            if (InternalModdedItemNames == null)
            {
                return;
            }

            foreach (string internalName in InternalModdedItemNames)
            {
                CalamityInstance.TryFind(internalName, out ModItem item);
                if (item == null) 
                   continue;
                //Console.WriteLine($"IN: {internalName}\n Type: {item.Type}"); //debug
                PetRegistry.PetNamesAndItems.TryAdd(internalName, item.Type);
                InternalNameToModdedItemId.TryAdd(internalName, item.Type);
            };
        }

        public void MergeJunimoExp()
        {
            if (MiningXpPerModdedBlock != null)
            {
                Junimo.MiningXpPerBlock.AddRange(MiningXpPerModdedBlock);
            }

            if (HarvestingXpPerModdedPlant != null)
            {
                Junimo.HarvestingXpPerGathered.AddRange(HarvestingXpPerModdedPlant);
            }

            if (FishingXpPerModdedFish != null)
            {
                Junimo.FishingXpPerCaught.AddRange(FishingXpPerModdedFish);
            }

            if (FishingXpPerModdedKill != null)
            {
                Junimo.FishingXpPerKill.AddRange(FishingXpPerModdedKill);
            }
        }

        public bool IsModLoaded()
        {
            return CalamityInstance != null;
        }
        public bool GetModInstance(out Mod instance)
        {
            if (!IsModLoaded())
            {
                instance = null;
                return false;
            }
            instance = CalamityInstance;
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