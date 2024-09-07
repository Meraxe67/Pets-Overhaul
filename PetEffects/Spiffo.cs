using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Spiffo : PetEffect
    {
        public int ammoReserveChance = 20;
        public int zombieArmorPen = 6;
        public int penetrateChance = 75;

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SpiffoPlush) && NPCID.Sets.Zombies[target.type] && modifiers.DamageType.Type == DamageClass.Ranged.Type)
            {
                modifiers.ArmorPenetration += zombieArmorPen;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SpiffoPlush) && target.active == false && proj.CountsAsClass<RangedDamageClass>() && proj.penetrate >= 0)
            {
                proj.penetrate += GlobalPet.Randomizer(penetrateChance);
                if (proj.usesLocalNPCImmunity == false)
                {
                    proj.usesLocalNPCImmunity = true;
                    proj.localNPCHitCooldown = 10;
                }
            }
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return Pet.PetInUseWithSwapCd(ItemID.SpiffoPlush) && Main.rand.NextBool(ammoReserveChance, 100)
                ? false
                : base.CanConsumeAmmo(weapon, ammo);
        }
    }
    public sealed class SpiffoPlush : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SpiffoPlush;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Spiffo spiffo = Main.LocalPlayer.GetModPlayer<Spiffo>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SpiffoPlush")
                .Replace("<class>", PetTextsColors.ClassText(spiffo.PetClassPrimary, spiffo.PetClassSecondary))
                        .Replace("<ammoReserve>", spiffo.ammoReserveChance.ToString())
                        .Replace("<armorPen>", spiffo.zombieArmorPen.ToString())
                        .Replace("<penChance>", spiffo.penetrateChance.ToString())
                        ));
        }
    }
}
