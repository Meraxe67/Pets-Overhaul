using Microsoft.Xna.Framework;
using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Commands
{
    public class ModContentCommand : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type
            => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command
            => "modContent";

        // A short description of this command
        public override string Description
            => "";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                string returnVal = "";
                foreach (KeyValuePair<string, int> entry in PetRegistry.PetNamesAndItems)
                {
                    string key = entry.Key;
                    int value = entry.Value;
                    returnVal += $"Name: {key} Id: {value}\n";

                };
                caller.Reply(returnVal);
            }
            else
            {
                if (!int.TryParse(args[0], out int type))
                {
                    if (PetRegistry.PetNamesAndItems.ContainsKey(args[0]))
                    {
                        caller.Reply($"Name: {args[0]} Id: {PetRegistry.PetNamesAndItems[args[0]]}\n");
                    }
                    else
                    {
                        caller.Reply("Nonexistant");
                    }
                }
                else
                {
                    throw new UsageException("The given argument must be a string");
                }
            }
        }
    }
    public class PlayerPlacedBlockListCommand : ModCommand
    {
        public override string Command => "PlayerPlacedBlockAmount";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "(DEBUG COMMAND)Displays current amount of blocks that are placed by Player(s).";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply(PlayerPlacedBlockList.placedBlocksByPlayer.Count.ToString());
        }
    }
    public class PetsOverhaulCommands : ModCommand
    {
        public override string Command => "pet";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "Pets Overhaul commands";
        public override string Usage => "/pet <option>\n"
            + "Only use /pet to see options.";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    caller.Reply("[c/90C2AA:List of Pets Overhaul commands:]\n" +
                    "/pet fortune - Displays your current fortune stats and explains their functionality.\n" +
                    "/pet vanity - Explains how to use a 'vanity' Pet.\n" +
                    "/pet junimoscoreboard - Displays Top 5 Online Players with highest exp counts for all 3 skills of Junimo. Only returns your EXP & level values if you're in singleplayer.\n" +
                    "/pet faq - Displays frequently asked questions regarding Pets Overhaul.\n");
                    break;
                case 1:
                    switch (args[0].ToLower())
                    {
                        case "fortune":
                            GlobalPet Pet = caller.Player.GetModPlayer<GlobalPet>();
                            caller.Reply("All fortune stats increase amount of items obtained in said category of items. (Ex. Ores = Mining Fortune)" +
                                "\nThey work with 100% effectiveness for items gained by Pets of same Class, 50% for items gained through non-pet means." +
                                "\nFor example: If you have 100 Harvesting Fortune, gathering 3 Dayblooms by hand and +2 with Magical Pumpkin Seed (Squashling Pet)" +
                                "\nWill result in 1.5 (item gained through non-pet means, 50% effective) + 2 (item gained through Pet perk, 100% effective) more Daybloom." +
                                "\n[c/90C2AA:Global Fortune] - While working with 100% effectiveness on non-classified Pet Items," +
                                "\nworks with 50% effectiveness with everything else. Your Current Global Fortune: " + Pet.globalFortune +
                                "\n[c/96A8B0:Mining Fortune] - Ores, gems etc. Your Current Mining Fortune: " + Pet.miningFortune +
                                "\n[c/0382E9:Fishing Fortune] - Fishes, crates etc. Your Current Fishing Fortune: " + Pet.fishingFortune +
                                "\n[c/CDFF00:Harvesting Fortune] - Herbs, plants, trees etc. Your Current Harvesting Fortune: " + Pet.harvestingFortune);
                            break;
                        case "vanity":
                            caller.Reply("Step 1: Equip the pet you don't want to be visible, but want its effects active in your Pet Slot." +
                                "\nStep 2: Disable the currently equipped Pet's visibility from little eye next to the Pet Slot." +
                                "\nStep 3: 'Use' the Pet you want to be visible via the Pet Item from your inventory. Done!");
                            break;
                        case "junimoscoreboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                Junimo junimoLvls = caller.Player.GetModPlayer<Junimo>();
                                caller.Reply($"[c/96A8B0:Your Junimo Mining Level: {junimoLvls.junimoMiningLevel} Your Junimo Mining EXP: {junimoLvls.junimoMiningExp}]");
                                caller.Reply($"[c/0382E9:Your Junimo Fishing Level: {junimoLvls.junimoFishingLevel} Your Junimo Fishing EXP: {junimoLvls.junimoFishingExp}]");
                                caller.Reply($"[c/CDFF00:Your Junimo Harvesting Level: {junimoLvls.junimoHarvestingLevel} Your Junimo Harvesting EXP: {junimoLvls.junimoHarvestingExp}]");
                            }
                            else
                            {
                                caller.Reply("[c/96A8B0:Top 5 Players with highest Junimo Mining EXP:]");
                                caller.Reply("[c/0382E9:Top 5 Players with highest Junimo Fishing EXP:]");
                                caller.Reply("[c/CDFF00:Top 5 Players with highest Junimo Harvesting EXP:]");
                            }
                            break;
                        case "faq":
                            caller.Reply("Q: Will there be crossmod content?" +
                                "\nA: Yes. Calamity will be priority, afterwards, possibly Thorium." +
                                "\nQ: Any way of increasing rolls of Light Pets?" +
                                "\nA: Currently, no. There may be a feature added in future, so its advised to keep your max rolls." +
                                "\nQ: How do I get X Pet?" +
                                "\nA: Refer to Wikis to figure how to obtain a certain Pet, vanilla Wiki can be a good helper with this." +
                                "\nQ: Why am I randomly getting fishes, crates etc. in my inventory?" +
                                "\nA: 99% due to your autofishing mod catching fishes procs your Fishing Fortune. (refer to /pet fortune for more info)" +
                                "\nQ: Why is my Pet effects not working?" +
                                "\nA: Make sure the Pet effects you want to take in place are in the Pet slot, which locates in 'Equipment' section, on top of your Helmet slot.");
                            break;
                        default:
                            caller.Reply("Given argument was invalid, here is list of Pets Overhaul commands:", Color.Red);
                            caller.Reply("/pet fortune - Displays your current fortune stats and explains their functionality.\n" +
                    "/pet vanity - Explains how to use a 'vanity' Pet.\n" +
                    "/pet junimoscoreboard - Displays Top 5 Online Players with highest exp counts for all 3 skills of Junimo. Only returns your EXP & level values if you're in singleplayer.\n" +
                    "/pet faq - Displays frequently asked questions regarding Pets Overhaul.\n");
                            break;
                    }
                    break;
                default:
                    caller.Reply("Only one argument was expected, make sure you're not using spaces for an argument. List of Pets Overhaul commands:", Color.Red);
                    caller.Reply("/pet fortune - Displays your current fortune stats and explains their functionality.\n" +
                    "/pet vanity - Explains how to use a 'vanity' Pet.\n" +
                    "/pet junimoscoreboard - Displays Top 5 Online Players with highest exp counts for all 3 skills of Junimo. Only returns your EXP & level values if you're in singleplayer.\n" +
                    "/pet faq - Displays frequently asked questions regarding Pets Overhaul.\n");
                    break;
            }
        }
    }
}
