using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameInput;
using PetsOverhaul.Config;

using PetsOverhaul.Config;
using Terraria.GameInput;

namespace PetsOverhaul.PetEffects.Vanilla
{
    sealed public class SugarGlider : ModPlayer
    {
        public float speedMult = 1.1f;
        public float accMult = 1.2f;
        public float accSpeedRaise = 0.1f;
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EucaluptusSap))
            {
                if (Player.equippedWings == null)
                {
                    Player.wings = 1;
                    Player.wingsLogic = 1;
                    Player.wingTimeMax = 1;
                }
            }
        }
    }
    sealed public class EucaluptusSap : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.EucaluptusSap;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            SugarGlider sugarGlider = Main.LocalPlayer.GetModPlayer<SugarGlider>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EucaluptusSap")
                        .Replace("<speed>", sugarGlider.speedMult.ToString())
                        .Replace("<acceleration>", sugarGlider.accMult.ToString())
                        .Replace("<flatIncrease>", (sugarGlider.accSpeedRaise * 100).ToString())
                        ));
        }
    }
    sealed public class SugarGliderWing : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            if (player.TryGetModPlayer(out SugarGlider sugarGlider) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.EucaluptusSap))
            {
                speed *= sugarGlider.speedMult;
                speed += sugarGlider.accSpeedRaise;
                acceleration *= sugarGlider.accMult;
                acceleration += sugarGlider.accSpeedRaise;
            }
        }
    }
}
