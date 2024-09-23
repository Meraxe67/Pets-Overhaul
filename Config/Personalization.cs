using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace PetsOverhaul.Config
{
    public class Personalization : ModConfig
    { //Remember most are 'disablers', may be a bit confusing.
        public override LocalizedText DisplayName => Language.GetText("Mods.PetsOverhaul.Config.Personalization");
        public override ConfigScope Mode => ConfigScope.ClientSide;
        //Display
        #region
        [Header("$Mods.PetsOverhaul.Config.HeaderDisplay")]

        [LabelKey("$Mods.PetsOverhaul.Config.NoticeLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.NoticeTooltip")]
        [DefaultValue(false)]
        public bool DisableNotice { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.ModNoticeLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ModNoticeTooltip")]
        [DefaultValue(false)]
        public bool DisableModNotice { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DisableTooltipToggleLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DisableTooltipToggleTooltip")]
        [DefaultValue(true)]
        public bool DisableTooltipToggle { get; set; }

        [OptionStrings(["On the Player, Icon on the left", "On the Player, Icon on the right", "Next to the Healthbar, Icon on the left", "Next to the Healthbar, Icon on the right"])]
        [DefaultValue("Next to the Healthbar, Icon on the right")]
        [LabelKey("$Mods.PetsOverhaul.Config.ShieldLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ShieldLocationTooltip")]
        public string ShieldLocation { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DisableAbilityUnusedLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DisableAbilityUnusedTooltip")]
        [DefaultValue(true)]
        public bool AbilityDisplayUnused { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilityDisplayInfoLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilityDisplayInfoTooltip")]
        [DefaultValue(false)]
        public bool AbilityDisplayInfo { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilityDisplayDisableLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilityDisplayDisableTooltip")]
        [DefaultValue(false)]
        public bool AbilityDisplayDisable { get; set; }
        #endregion
        //Sounds
        #region
        [Header("$Mods.PetsOverhaul.Config.HeaderSound")]

        [LabelKey("$Mods.PetsOverhaul.Config.HurtSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.HurtSoundTooltip")]
        [DefaultValue(false)]
        public bool HurtSoundDisabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DeathSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DeathSoundTooltip")]
        [DefaultValue(false)]
        public bool DeathSoundDisabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PassiveSoundTooltip")]
        [DefaultValue(false)]
        public bool PassiveSoundDisabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilitySoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilitySoundTooltip")]
        [DefaultValue(false)]
        public bool AbilitySoundDisabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownSoundTooltip")]
        [DefaultValue(true)]
        public bool LowCooldownSoundDisabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownTresholdLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownTresholdTooltip")]
        [Range(30, 600)]
        [DefaultValue(150)]
        public int LowCooldownTreshold { get; set; }
        #endregion
        //Gameplay
        #region
        [Header("$Mods.PetsOverhaul.Config.HeaderGameplay")]

        [Slider()]
        [Range(0, 20)]
        [LabelKey("$Mods.PetsOverhaul.Config.MoreDifficultLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.MoreDifficultTooltip")]
        [DefaultValue(0)]
        [DrawTicks]
        public int DifficultAmount { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.SwapCooldownLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.SwapCooldownTooltip")]
        [DefaultValue(false)]
        public bool SwapCooldown { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.PhantasmalDragonShootLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PhantasmalDragonShootTooltip")]
        [DefaultValue(true)]
        public bool PhantasmalDragonVolleyFromMouth { get; set; }
        #endregion

    }
}
