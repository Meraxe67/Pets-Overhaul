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
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply(PlayerPlacedBlockList.placedBlocksByPlayer.Count.ToString());
        }
    }
}
