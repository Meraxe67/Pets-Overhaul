using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyRedPanda : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Harvesting;
        public int aggroReduce = 500;
        public float regularAtkSpd = 0.06f;
        public float jungleBonusSpd = 0.04f;
        public int bambooChance = 50;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BambooLeaf))
            {
                Player.aggro -= aggroReduce;
                Player.GetAttackSpeed<GenericDamageClass>() += regularAtkSpd;
                if (Player.ZoneJungle)
                {
                    Player.GetAttackSpeed<GenericDamageClass>() += jungleBonusSpd;
                }
            }
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            BabyRedPanda panda = player.GetModPlayer<BabyRedPanda>();
            if (PickerPet.PickupChecks(item, ItemID.BambooLeaf, out ItemPet _) && item.type == ItemID.BambooBlock)
            {
                for (int i = 0; i < ItemPet.Randomizer(panda.bambooChance * item.stack); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), item.type, 1);
                }
            }
        }
    }
    public sealed class BambooLeaf : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BambooLeaf;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyRedPanda babyRedPanda = Main.LocalPlayer.GetModPlayer<BabyRedPanda>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BambooLeaf")
                .Replace("<class>", PetColors.ClassText(babyRedPanda.PetClassPrimary, babyRedPanda.PetClassSecondary))
                .Replace("<atkSpd>", Math.Round(babyRedPanda.regularAtkSpd * 100, 2).ToString())
                .Replace("<jungleAtkSpd>", Math.Round(babyRedPanda.jungleBonusSpd * 100, 2).ToString())
                .Replace("<aggro>", babyRedPanda.aggroReduce.ToString())
                .Replace("<bambooChance>", babyRedPanda.bambooChance.ToString())
            ));
        }
    }
}
