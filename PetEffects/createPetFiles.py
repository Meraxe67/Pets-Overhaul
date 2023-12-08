import os
import re

string = """using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using PetsOverhaul.Config;
using Terraria.GameInput;
using PetsOverhaul.ModSupport;
using System;

namespace PetsOverhaul.PetEffects.ModName
{
    sealed public class PetName : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            
        }
    }
    sealed public class PetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if(ModManager.ModName == null) return false;
            if(ModManager.ModName.InternalNameToModdedItemId == null) return false;
            if(!ModManager.ModName.InternalNameToModdedItemId.ContainsKey("PetItemKey")) return false;

            return entity.type == ModManager.ModName.InternalNameToModdedItemId["PetItemKey"];
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current) return;
            PetName PetNameUnder = Main.LocalPlayer.GetModPlayer<PetName>();
            tooltips.Add(new(Mod, "Tooltip0", "Pet Overhaul effects coming soon!"/*Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.PetItem")*/
                
            ));
        }
    }
}
"""


modName = input("Mod?: ")
inputType = input("Type of input? (1/2/3): ")
if(not os.path.exists(modName)):
    os.mkdir(modName)
string = string.replace("ModName", modName)

listOfPets = input("List?:").replace("\"", "").replace("\n", "").split(',')
if inputType == "1":
    print("single line")
    listOfPets.sort()
    for x in listOfPets:
        f = open(modName + "/" + x + ".cs", "wt")
        f.write(string.replace("PetItem", x))
        f.close()

    print("Done. Made {} files.".format(len(listOfPets)))
elif inputType == "2":
    print("multiline")
    def recurse():
        i = input("next").replace("\"", "").replace(",", "").replace(" ", "")
        f = open(modName + "/" + i + ".cs", "wt")
        f.write(string.replace("PetItem", i))
        f.close()
        print("Done. Made {} file.".format(i))
        if f:
            recurse()
    recurse()
elif inputType == "3":
    print("multiline2")
    def recurse():
        i = re.sub(r"[^a-z,A-Z]+","",input("next")).split(",")
        f = open(modName + "/" + i[1] + ".cs", "wt")
        s = string.replace("PetItemKey", i[0])
        if i[0] == i[1]: 
            i[0] = i[0]+"Item"
        f.write(s.replace("PetItem", i[0]).replace("PetNameUnder", i[1][0].lower() + i[1][1:]).replace("PetName", i[1]))
        f.close()
        print("Done. Made {} file.".format(i[1]))
        if f:
            recurse()
    recurse()