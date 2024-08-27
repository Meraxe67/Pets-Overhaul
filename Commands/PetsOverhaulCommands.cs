using Microsoft.Xna.Framework;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Commands
{
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
        public struct TopPlayer
        {
            public string PlayerName { get; set; }
            public int PlayerLevel { get; set; }
            public int PlayerExp { get; set; }
        }
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
                    caller.Reply("[c/90C2AA:List of Pets Overhaul commands:] (commands are not case sensitive)\n" +
                    "/pet fortune, /pet fortunestat, /pet fortunestats - Displays your current fortune stats and explains their functionality.\n" +
                    "/pet vanity, /pet vanitypet - Explains how to use a 'vanity' Pet.\n" +
                    "/pet junimo, /pet junimoscoreboard, /pet junimoleaderboard - Displays Top 3 Online Players with highest exp counts for all 3 skills of Junimo. Only returns your EXP & level values if you're in singleplayer.\n" +
                    "/pet {mining|fishing|harvesting}scoreboard, /pet {mining|fishing|harvesting}leaderboard - Displays limitless amount of Online Players with highest chosen skill's exp counts from Junimo's 3 skills.\n" +
                    "/pet faq, /pet question - Displays frequently asked questions regarding Pets Overhaul.\n", Color.Gray);
                    break;
                case 1:
                    switch (args[0].ToLower())
                    {
                        case "fortune" or "fortunestat" or "fortunestats":
                            GlobalPet Pet = caller.Player.GetModPlayer<GlobalPet>();
                            caller.Reply("All fortune stats increase amount of items obtained in said category of items. (Ex. Ores = Mining Fortune)" +
                                "\nThey work with 100% effectiveness for items gained by Pets of same Class, 50% for items gained through non-pet means." +
                                "\nFor example: If you have 100 Harvesting Fortune, gathering 3 Dayblooms by hand and +2 with Magical Pumpkin Seed (Squashling Pet)" +
                                "\nWill result in 1.5 (item gained through non-pet means, 50% effective) + 2 (item gained through Pet perk, 100% effective) more Daybloom.", Color.Gray);
                            caller.Reply("\n[c/90C2AA:Global Fortune] - While working with 100% effectiveness on non-classified Pet Items," +
                            "\nworks with 50% effectiveness with everything else. Your Current Global Fortune: " + Pet.globalFortune +
                            "\n[c/96A8B0:Mining Fortune] - Ores, gems etc. Your Current Mining Fortune: " + Pet.miningFortune +
                            "\n[c/0382E9:Fishing Fortune] - Fishes, crates etc. Your Current Fishing Fortune: " + Pet.fishingFortune +
                            "\n[c/CDFF00:Harvesting Fortune] - Herbs, plants, trees etc. Your Current Harvesting Fortune: " + Pet.harvestingFortune);
                            break;
                        case "vanity" or "vanitypet":
                            caller.Reply("Step 1: Equip the pet you don't want to be visible, but want its effects active in your Pet Slot." +
                                "\nStep 2: Disable the currently equipped Pet's visibility from little eye next to the Pet Slot." +
                                "\nStep 3: 'Use' the Pet you want to be visible via the Pet Item from your inventory. Done!");
                            break;
                        case "junimo" or "junimoscoreboard" or "junimoleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                Junimo junimoLvls = caller.Player.GetModPlayer<Junimo>();
                                caller.Reply($"[c/96A8B0:Your Junimo Mining Level: {junimoLvls.junimoMiningLevel} Your Junimo Mining EXP: {junimoLvls.junimoMiningExp}]");
                                caller.Reply($"[c/0382E9:Your Junimo Fishing Level: {junimoLvls.junimoFishingLevel} Your Junimo Fishing EXP: {junimoLvls.junimoFishingExp}]");
                                caller.Reply($"[c/CDFF00:Your Junimo Harvesting Level: {junimoLvls.junimoHarvestingLevel} Your Junimo Harvesting EXP: {junimoLvls.junimoHarvestingExp}]");
                            }
                            else
                            {
                                List<TopPlayer> topMining = new();
                                List<TopPlayer> topFishing = new();
                                List<TopPlayer> topHarvesting = new();
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topMining.Add(new TopPlayer() with { PlayerExp = juni.junimoMiningExp, PlayerLevel = juni.junimoMiningLevel, PlayerName = player.name });
                                    topFishing.Add(new TopPlayer() with { PlayerExp = juni.junimoFishingExp, PlayerLevel = juni.junimoFishingLevel, PlayerName = player.name });
                                    topHarvesting.Add(new TopPlayer() with { PlayerExp = juni.junimoHarvestingExp, PlayerLevel = juni.junimoHarvestingLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;
                                caller.Reply($"\n[c/96A8B0:Top {topMining.Count} Player" + (topMining.Count > 1 ? "s" : "") + " with highest Junimo Mining EXP:]");
                                for (int i = topMining.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topMining.Find(x => x.PlayerExp == topMining.Max(x => x.PlayerExp));
                                    caller.Reply(topPlayer.PlayerName + "'s Mining Level: " + topPlayer.PlayerLevel + " Mining Exp: " + topPlayer.PlayerExp, displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topMining.Remove(topPlayer);
                                }
                                displayCounter = 0;
                                caller.Reply($"\n[c/0382E9:Top {topFishing.Count} Player" + (topFishing.Count > 1 ? "s" : "") + " with highest Junimo Fishing EXP:]");
                                for (int i = topFishing.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topFishing.Find(x => x.PlayerExp == topFishing.Max(x => x.PlayerExp));
                                    caller.Reply(topPlayer.PlayerName + "'s Fishing Level: " + topPlayer.PlayerLevel + " Fishing Exp: " + topPlayer.PlayerExp, displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                }
                                displayCounter = 0;
                                caller.Reply($"\n[c/CDFF00:Top {topHarvesting.Count} Player" + (topHarvesting.Count > 1 ? "s" : "") + " with highest Junimo Harvesting EXP:]");
                                for (int i = topHarvesting.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topHarvesting.Find(x => x.PlayerExp == topHarvesting.Max(x => x.PlayerExp));
                                    caller.Reply(topPlayer.PlayerName + "'s Harvesting Level: " + topPlayer.PlayerLevel + " Harvesting Exp: " + topPlayer.PlayerExp, displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topHarvesting.Remove(topPlayer);
                                }
                            }
                            break;
                        case "miningscoreboard" or "miningleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply("Please use this command on Multiplayer! If you want to see your Junimo Skills, use /pet junimoscoreboard or /pet junimoleaderboard");
                            }
                            else
                            {
                                List<TopPlayer> topMining = new();
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topMining.Add(new TopPlayer() with { PlayerExp = juni.junimoMiningExp, PlayerLevel = juni.junimoMiningLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;
                                caller.Reply($"\n[c/96A8B0:Top {topMining.Count} Player" + (topMining.Count > 1 ? "s" : "") + " with highest Junimo Mining EXP:]");
                                for (int i = topMining.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topMining.Find(x => x.PlayerExp == topMining.Max(x => x.PlayerExp));
                                    caller.Reply(topPlayer.PlayerName + "'s Mining Level: " + topPlayer.PlayerLevel + " Mining Exp: " + topPlayer.PlayerExp, displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topMining.Remove(topPlayer);
                                }
                            }
                            break;
                        case "fishingscoreboard" or "fishingleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply("Please use this command on Multiplayer! If you want to see your Junimo Skills, use /pet junimoscoreboard or /pet junimoleaderboard");
                            }
                            else
                            {
                                List<TopPlayer> topFishing = new();
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topFishing.Add(new TopPlayer() with { PlayerExp = juni.junimoFishingExp, PlayerLevel = juni.junimoFishingLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;
                                caller.Reply($"\n[c/0382E9:Top {topFishing.Count} Player" + (topFishing.Count > 1 ? "s" : "") + " with highest Junimo Fishing EXP:]");
                                for (int i = topFishing.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topFishing.Find(x => x.PlayerExp == topFishing.Max(x => x.PlayerExp));
                                    caller.Reply(topPlayer.PlayerName + "'s Fishing Level: " + topPlayer.PlayerLevel + " Fishing Exp: " + topPlayer.PlayerExp, displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                }
                            }
                            break;
                        case "harvestingscoreboard" or "harvestingleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply("Please use this command on Multiplayer! If you want to see your Junimo Skills, use /pet junimo, /pet junimoscoreboard or /pet junimoleaderboard");
                            }
                            else
                            {
                                List<TopPlayer> topHarvesting = new();
                                foreach (var player in Main.ActivePlayers)
                                {
                                    Junimo juni = player.GetModPlayer<Junimo>();
                                    topHarvesting.Add(new TopPlayer() with { PlayerExp = juni.junimoHarvestingExp, PlayerLevel = juni.junimoHarvestingLevel, PlayerName = player.name });
                                }
                                int displayCounter = 0;
                                caller.Reply($"\n[c/CDFF00:Top {topHarvesting.Count} Player" + (topHarvesting.Count > 1 ? "s" : "") + " with highest Junimo Harvesting EXP:]");
                                for (int i = topHarvesting.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topHarvesting.Find(x => x.PlayerExp == topHarvesting.Max(x => x.PlayerExp));
                                    caller.Reply(topPlayer.PlayerName + "'s Harvesting Level: " + topPlayer.PlayerLevel + " Harvesting Exp: " + topPlayer.PlayerExp, displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topHarvesting.Remove(topPlayer);
                                }
                            }
                            break;
                        case "faq" or "question":
                            caller.Reply("Q: Will there be crossmod content?" +
                                "\nA: Yes. Calamity will be priority, afterwards, possibly Thorium." +
                                "\nQ: Any way of increasing rolls of Light Pets?" +
                                "\nA: Currently, no. There may be a feature added in future, so its advised to keep your max rolls." +
                                "\nQ: How do I get X Pet?" +
                                "\nA: Refer to Wikis to figure how to obtain a certain Pet, vanilla Wiki can be a good helper with this." +
                                "\nQ: Why am I randomly getting fishes, crates etc. in my inventory?" +
                                "\nA: 99% due to your autofishing mod catching fishes procs your Fishing Fortune. (refer to /pet fortune for more info)" +
                                "\nQ: Why is my Pet effects not working? & How do Pet Effects become activated?" +
                                "\nA: Make sure the Pet effects you want to take in place are in the Pet slot, which locates in 'Equipment' section, on top of your Helmet slot.");
                            break;
                        default:
                            caller.Reply("Given argument was invalid, here is list of Pets Overhaul commands: (commands are not case sensitive)", Color.Red);
                            caller.Reply("/pet fortune, /pet fortunestat, /pet fortunestats - Displays your current fortune stats and explains their functionality.\n" +
                    "/pet vanity, /pet vanitypet - Explains how to use a 'vanity' Pet.\n" +
                    "/pet junimo, /pet junimoscoreboard, /pet junimoleaderboard - Displays Top 3 Online Players with highest exp counts for all 3 skills of Junimo. Only returns your EXP & level values if you're in singleplayer.\n" +
                    "/pet {mining|fishing|harvesting}scoreboard, /pet {mining|fishing|harvesting}leaderboard - Displays limitless amount of Online Players with highest chosen skill's exp counts from Junimo's 3 skills.\n" +
                    "/pet faq, /pet question - Displays frequently asked questions regarding Pets Overhaul.\n", Color.Gray);
                            break;

                    }
                    break;
                default:
                    caller.Reply("Only one argument was expected, make sure you're not using spaces for an argument. List of Pets Overhaul commands: (commands are not case sensitive)", Color.Red);
                    caller.Reply("/pet fortune, /pet fortunestat, /pet fortunestats - Displays your current fortune stats and explains their functionality.\n" +
                    "/pet vanity, /pet vanitypet - Explains how to use a 'vanity' Pet.\n" +
                    "/pet junimo, /pet junimoscoreboard, /pet junimoleaderboard - Displays Top 3 Online Players with highest exp counts for all 3 skills of Junimo. Only returns your EXP & level values if you're in singleplayer.\n" +
                    "/pet {mining|fishing|harvesting}scoreboard, /pet {mining|fishing|harvesting}leaderboard - Displays limitless amount of Online Players with highest chosen skill's exp counts from Junimo's 3 skills.\n" +
                    "/pet faq, /pet question - Displays frequently asked questions regarding Pets Overhaul.\n", Color.Gray);
                    break;
            }
        }
    }
}
