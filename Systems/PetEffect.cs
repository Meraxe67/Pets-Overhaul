using PetsOverhaul.Config;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Abstract class that contains what every Primary Pet Effect contains.
    /// </summary>
    public abstract class PetEffect : ModPlayer
    {
        /// <summary>
        /// Accesses the GlobalPet class, which has useful methods and fields for Pet implementation.
        /// </summary>
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        /// <summary>
        /// Primary Class of Pet that will appear on its tooltip with its color.
        /// </summary>
        public abstract PetClasses PetClassPrimary { get; }
        /// <summary>
        /// Secondary Class of Pet that will appear on its tooltip, which will mix its color with the Primary Classes color. Defaults to None.
        /// </summary>
        public virtual PetClasses PetClassSecondary => PetClasses.None;
    }
    //public abstract class PetTooltip : GlobalItem //Undone, but released due to an emergency patch
    //{
        
    //    public abstract int PetItemID { get; }
    //    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    //    {
    //        return entity.type == PetItemID;
    //    }
    //    public abstract string PetsTooltip { get; }
    //    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    //    {
    //        if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
    //        {
    //            return;
    //        }

    //        if (tooltips.Find(x => x.Name == "Tooltip0") != null)
    //            tooltips.Find(x => x.Name == "Tooltip0").Text += "\n" + PetsTooltip;

    //        ExtraModifyTooltips(item, tooltips);
    //    }
    //    public virtual void ExtraModifyTooltips(Item item, List<TooltipLine> tooltips)
    //    { }
    //}
}
