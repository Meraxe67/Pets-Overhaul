using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PetsOverhaul.Config
{
    public class Personalization : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [LabelKey("$Mods.PetsOverhaul.Config.NoticeLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.NoticeTooltip")]
        [DefaultValue(false)]
        public bool DisableNotice;
        [LabelKey("$Mods.PetsOverhaul.Config.TooltipShiftToggleLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.TooltipShiftToggleTooltip")]
        [DefaultValue(false)]
        public bool TooltipsEnabledWithShift;
        [LabelKey("$Mods.PetsOverhaul.Config.HurtSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.HurtSoundTooltip")]
        [DefaultValue(false)]
        public bool HurtSoundDisabled;
        [LabelKey("$Mods.PetsOverhaul.Config.DeathSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.DeathSoundTooltip")]
        [DefaultValue(false)]
        public bool DeathSoundDisabled;
        [LabelKey("$Mods.PetsOverhaul.Config.PassiveSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.PassiveSoundTooltip")]
        [DefaultValue(false)]
        public bool PassiveSoundDisabled;
        [LabelKey("$Mods.PetsOverhaul.Config.AbilitySoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.AbilitySoundTooltip")]
        [DefaultValue(false)]
        public bool AbilitySoundDisabled;
        [LabelKey("$Mods.PetsOverhaul.Config.LowCooldownSoundLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.LowCooldownSoundTooltip")]
        [DefaultValue(true)]
        public bool LowCooldownSoundDisabled;
        [Slider()]
        [Range(0, 20)]
        [LabelKey("$Mods.PetsOverhaul.Config.MoreDifficultLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.MoreDifficultTooltip")]
        [DefaultValue(0)]
        public int DifficultAmount;
        [LabelKey("$Mods.PetsOverhaul.Config.SwapCooldownLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.SwapCooldownTooltip")]
        [DefaultValue(false)]
        public bool SwapCooldown;
        [OptionStrings(["On the Player, Icon on the left", "On the Player, Icon on the right", "Next to the Healthbar, Icon on the left", "Next to the Healthbar, Icon on the right"])]
        [DefaultValue("Next to the Healthbar, Icon on the right")]
        [LabelKey("$Mods.PetsOverhaul.Config.ShieldLocationLabel")]
        [TooltipKey("$Mods.PetsOverhaul.Config.ShieldLocationTooltip")]
        public string ShieldLocation;
    }
}
