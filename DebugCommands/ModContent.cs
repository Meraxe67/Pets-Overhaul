using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace PetsOverhaul.DebugCommands
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
                foreach (KeyValuePair<string, int> entry in ModContent.GetInstance<PetRegistry>().TerrariaPetItemIds)
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
                    if (ModContent.GetInstance<PetRegistry>().TerrariaPetItemIds.ContainsKey(args[0]))
                    {
                        caller.Reply($"Name: {args[0]} Id: {ModContent.GetInstance<PetRegistry>().TerrariaPetItemIds[args[0]]}\n");
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
}
