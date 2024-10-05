using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PetsOverhaul.Systems;
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
        [DefaultValue(true)]
        public bool EnableNotice { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.ModNoticeLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ModNoticeTooltip")]
        [DefaultValue(true)]
        public bool EnableModNotice { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DisableTooltipToggleLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DisableTooltipToggleTooltip")]
        [DefaultValue(false)]
        public bool EnableTooltipToggle { get; set; }

        [DefaultValue(ShieldPosition.HealthBarRight)]
        [LabelKey("$Mods.PetsOverhaul.Config.ShieldLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ShieldLocationTooltip")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShieldPosition ShieldLocation { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DisableAbilityUnusedLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DisableAbilityUnusedTooltip")]
        [DefaultValue(false)]
        public bool AbilityDisplayUnused { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilityDisplayInfoLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilityDisplayInfoTooltip")]
        [DefaultValue(true)]
        public bool AbilityDisplayInfo { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilityDisplayDisableLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilityDisplayDisableTooltip")]
        [DefaultValue(true)]
        public bool ShowAbilityDisplay { get; set; }
        #endregion
        //Sounds
        #region
        [Header("$Mods.PetsOverhaul.Config.HeaderSound")]

        [LabelKey("$Mods.PetsOverhaul.Config.HurtSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.HurtSoundTooltip")]
        [DefaultValue(true)]
        public bool HurtSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.DeathSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DeathSoundTooltip")]
        [DefaultValue(true)]
        public bool DeathSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PassiveSoundTooltip")]
        [DefaultValue(true)]
        public bool PassiveSoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.AbilitySoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilitySoundTooltip")]
        [DefaultValue(true)]
        public bool AbilitySoundEnabled { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownSoundTooltip")]
        [DefaultValue(false)]
        public bool LowCooldownSoundEnabled { get; set; }

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
        [DefaultValue(true)]
        public bool SwapCooldown { get; set; }

        [LabelKey("$Mods.PetsOverhaul.Config.PhantasmalDragonShootLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PhantasmalDragonShootTooltip")]
        [DefaultValue(false)]
        public bool PhantasmalDragonVolleyFromMouth { get; set; }
        #endregion

    }
}
