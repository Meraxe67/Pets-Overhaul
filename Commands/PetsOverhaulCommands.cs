using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using System.Collections.Generic;
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
                    "/pet fortune - Displays your current fortune stats and explains their functionality\n" +
                    "/pet vanity - Explains how to use a 'vanity' Pet\n" +
                    "/pet junimoscoreboard - Displays Top 5 Players, with their exp counts for all 3 skills of Junimo\n" +
                    "/pet faq - Displays frequently asked questions regarding Pets Overhaul\n");
                    break;
                case 1:
                    switch (args[0].ToLower())
                    {
                        case "fortune":
                            caller.Reply("plchldr");
                            break;
                        case "vanity":
                            caller.Reply("plchldr");
                            break;
                        case "junimoscoreboard":
                            caller.Reply("plchldr");
                            break;
                        case "faq":
                            caller.Reply("plchldr");
                            break;
                        default:
                            caller.Reply("Given argument was invalid, here is list of Pets Overhaul commands:", Color.Red);
                            caller.Reply("/pet fortune - Displays your current fortune stats and explains their functionality\n" +
                        "/pet vanity - Explains how to use a 'vanity' Pet\n" +
                        "/pet junimoscoreboard - Displays Top 5 Players, with their exp counts for all 3 skills of Junimo\n" +
                        "/pet faq - Displays frequently asked questions regarding Pets Overhaul\n");
                            break;
                    }
                    break;
                default:
                    caller.Reply("Only one argument was expected, make sure you're not using spaces for an argument. List of Pets Overhaul commands:", Color.Red);
                    caller.Reply("/pet fortune - Displays your current fortune stats and explains their functionality\n" +
                    "/pet vanity - Explains how to use a 'vanity' Pet\n" +
                    "/pet junimoscoreboard - Displays Top 5 Players, with their exp counts for all 3 skills of Junimo\n" +
                    "/pet faq - Displays frequently asked questions regarding Pets Overhaul\n");
                    break;
            }
        }
    }
}
