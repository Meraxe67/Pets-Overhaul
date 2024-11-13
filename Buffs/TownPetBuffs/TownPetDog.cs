using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetDog : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Dog dog = Main.LocalPlayer.GetModPlayer<Dog>();
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCID.TownDog && Main.LocalPlayer.Distance(npc.Center) < dog.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetDog>()).Replace("<Name>", npc.FullName);
                    break;
                }
                else
                {
                    buffName = "Dog Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetDog>())
                .Replace("<DogFish>", dog.dogFish.ToString())
                .Replace("<DogFishExp>", dog.DefaultFishFort.ToString());
            rare = 0;
        }
    }
}
