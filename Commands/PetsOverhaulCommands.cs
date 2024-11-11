using Microsoft.Xna.Framework;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Commands
{
    public class PlayerPlacedBlockListCommand : ModCommand
    {
        public override string Command => "PlayerPlacedBlockAmount";
        public override CommandType Type => CommandType.World;
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
                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.Help"), Color.Gray);
                    break;
                case 1:
                    switch (args[0].ToLower())
                    {
                        case "fortune" or "fortunestat" or "fortunestats":
                            GlobalPet Pet = caller.Player.GetModPlayer<GlobalPet>();
                            caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.FortuneInfo"), Color.Gray);
                            caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.FortuneCurrent").Replace("<global>", Pet.globalFortune.ToString())
                                .Replace("<mining>", Pet.miningFortune.ToString()).Replace("<fishing>", Pet.fishingFortune.ToString()).Replace("<harvesting>", Pet.harvestingFortune.ToString()));
                            break;

                        case "vanity" or "vanitypet":
                            caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.VanityPet"));
                            break;

                        case "junimo" or "junimoscoreboard" or "junimoleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                Junimo junimoLvls = caller.Player.GetModPlayer<Junimo>();
                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.SinglePlayerJunimo").Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3())
                                    .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Mining)).Replace("<level>", junimoLvls.junimoMiningLevel.ToString()).Replace("<exp>", junimoLvls.junimoMiningExp.ToString()));

                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.SinglePlayerJunimo").Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3())
                                    .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Fishing)).Replace("<level>", junimoLvls.junimoFishingLevel.ToString()).Replace("<exp>", junimoLvls.junimoFishingExp.ToString()));

                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.SinglePlayerJunimo").Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3())
                                    .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Harvesting)).Replace("<level>", junimoLvls.junimoHarvestingLevel.ToString()).Replace("<exp>", junimoLvls.junimoHarvestingExp.ToString()));
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

                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.LeaderboardList")
                                    .Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()).Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Mining)));
                                for (int i = topMining.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topMining.Find(x => x.PlayerExp == topMining.Max(x => x.PlayerExp));
                                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Mining)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topMining.Remove(topPlayer);
                                }

                                displayCounter = 0;
                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.LeaderboardList")
                                    .Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()).Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Fishing)));
                                for (int i = topFishing.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topFishing.Find(x => x.PlayerExp == topFishing.Max(x => x.PlayerExp));
                                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Fishing)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                }

                                displayCounter = 0;
                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.LeaderboardList")
                                    .Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()).Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Harvesting)));
                                for (int i = topHarvesting.Count; i > 0 && displayCounter < 3; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topHarvesting.Find(x => x.PlayerExp == topHarvesting.Max(x => x.PlayerExp));
                                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Harvesting)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topHarvesting.Remove(topPlayer);
                                }
                            }
                            break;

                        case "miningscoreboard" or "miningleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.UseInMultiplayer"));
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

                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.LeaderboardList")
                                    .Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()).Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Mining)));
                                for (int i = topMining.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topMining.Find(x => x.PlayerExp == topMining.Max(x => x.PlayerExp));
                                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Mining)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topMining.Remove(topPlayer);
                                }
                            }
                            break;

                        case "fishingscoreboard" or "fishingleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.UseInMultiplayer"));
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

                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.LeaderboardList")
                                    .Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()).Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Fishing)));
                                for (int i = topFishing.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topFishing.Find(x => x.PlayerExp == topFishing.Max(x => x.PlayerExp));
                                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Fishing)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topFishing.Remove(topPlayer);
                                }
                            }
                            break;

                        case "harvestingscoreboard" or "harvestingleaderboard":
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.UseInMultiplayer"));
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

                                caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.LeaderboardList")
                                    .Replace("<color>", PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()).Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Harvesting)));
                                for (int i = topHarvesting.Count; i > 0 && displayCounter < Main.maxPlayers; i--, displayCounter++)
                                {
                                    TopPlayer topPlayer = topHarvesting.Find(x => x.PlayerExp == topHarvesting.Max(x => x.PlayerExp));
                                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.PlayerRankings").Replace("<player>", topPlayer.PlayerName)
                                        .Replace("<class>", PetTextsColors.PetClassLocalized(PetClasses.Harvesting)).Replace("<level>", topPlayer.PlayerLevel.ToString()).Replace("<exp>", topPlayer.PlayerExp.ToString()),
                                        displayCounter == 0 ? Color.Lavender : displayCounter == 1 ? PetTextsColors.HighQuality : displayCounter == 2 ? PetTextsColors.MidQuality : PetTextsColors.LowQuality);
                                    topHarvesting.Remove(topPlayer);
                                }
                            }
                            break;

                        case "faq" or "question":
                            caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.FAQ"));
                            break;

                        default:
                            caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.ArgumentInvalid"), Color.Red);
                            caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.Help"), Color.Gray);
                            break;

                    }
                    break;
                default:
                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.OneArgument"), Color.Red);
                    caller.Reply(Language.GetTextValue("Mods.PetsOverhaul.Commands.Help"), Color.Gray);
                    break;
            }
        }
    }
}
