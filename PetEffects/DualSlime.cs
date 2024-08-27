using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaul.Items;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class DualSlime : PetEffect
    {
        public bool swapTooltip = false;
        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        // King, in SlimePrince
        public float wetSpeed = 0.09f;
        public float wetDmg = 0.07f;
        public float wetDef = 0.07f;
        public float slimyKb = 1.45f;
        public float slimyJump = 1.8f;
        public float wetDealtLower = 0.94f;
        public float wetRecievedHigher = 1.07f;
        public float bonusKb = 1.45f;
        public float healthDmg = 0.012f;
        public int burnCap = 45;
        // Queen, in SlimePrincess
        public float slow = 0.59f;
        public float haste = 0.26f;
        public int shield = 6;
        public int shieldTime = 240;
        public float dmgBoost = 1.22f;
        public int baseCounterChnc = 90;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Keybinds.PetTooltipSwap != null && Keybinds.PetTooltipSwap.JustPressed)
            {
                swapTooltip = !swapTooltip;
            }
        }
    }

    public sealed class ResplendentDessert : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ResplendentDessert;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            DualSlime dualSlime = Main.LocalPlayer.GetModPlayer<DualSlime>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ResplendentDessert")
                .Replace("<approxWeak>", "10")
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.PetTooltipSwap))
                .Replace("<tooltip>", Language.GetTextValue($"Mods.PetsOverhaul.PetItemTooltips.{(dualSlime.swapTooltip ? "KingSlimePetItem" : "QueenSlimePetItem")}"))
                .Replace("<class>", PetTextsColors.ClassText(dualSlime.PetClassPrimary, dualSlime.swapTooltip ? PetClasses.None : PetClasses.Offensive))
                .Replace("<burnHp>", Math.Round(dualSlime.healthDmg * 100, 2).ToString())
                .Replace("<burnCap>", dualSlime.burnCap.ToString())
                .Replace("<extraKb>", dualSlime.bonusKb.ToString())
                .Replace("<jumpSpd>", Math.Round(dualSlime.slimyJump * 100, 2).ToString())
                .Replace("<kbBoost>", dualSlime.slimyKb.ToString())
                .Replace("<enemyDmgRecieve>", dualSlime.wetRecievedHigher.ToString())
                .Replace("<enemyDmgDeal>", dualSlime.wetDealtLower.ToString())
                .Replace("<dmg>", Math.Round(dualSlime.wetDmg * 100, 2).ToString())
                .Replace("<def>", Math.Round(dualSlime.wetDef * 100, 2).ToString())
                .Replace("<moveSpd>", Math.Round(dualSlime.wetSpeed * 100, 2).ToString())
                .Replace("<slow>", Math.Round(dualSlime.slow * 100, 2).ToString())
                .Replace("<haste>", Math.Round(dualSlime.haste * 100, 2).ToString())
                .Replace("<dmgBonus>", dualSlime.dmgBoost.ToString())
                .Replace("<shield>", dualSlime.shield.ToString())
                .Replace("<shieldTime>", Math.Round(dualSlime.shieldTime / 60f, 2).ToString())
                .Replace("<endless>", ModContent.ItemType<EndlessBalloonSack>().ToString())
            ));
        }
    }
}
