using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class PropellerGato : PetEffect
    {
        public int bonusCritChance = 15;
        public int turretIncrease = 1;

        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.DD2PetGato))
            {
                Player.maxTurrets++;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.DD2PetGato) && proj.GetGlobalProjectile<ProjectileSourceChecks>().isFromSentry)
            {
                if (proj.CritChance + bonusCritChance >= 100)
                {
                    modifiers.SetCrit();
                }
                else if (Main.rand.NextBool(proj.CritChance + bonusCritChance, 100))
                {
                    modifiers.SetCrit();
                }
                else
                {
                    modifiers.DisableCrit();
                }
            }
        }
    }
    public sealed class DD2PetGato : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DD2PetGato;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PropellerGato propellerGato = Main.LocalPlayer.GetModPlayer<PropellerGato>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DD2PetGato")
                .Replace("<class>", PetColors.ClassText(propellerGato.PetClassPrimary, propellerGato.PetClassSecondary))
                        .Replace("<crit>", propellerGato.bonusCritChance.ToString())
                        .Replace("<maxSentry>", propellerGato.turretIncrease.ToString())
                        ));
        }
    }
}
