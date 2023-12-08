using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Spiffo : ModPlayer
    {
        public int ammoReserveChance = 10;
        public int zombieArmorPen = 4;
        public int penetrateChance = 50;

        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
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
                proj.penetrate += ItemPet.Randomizer(penetrateChance);
                proj.usesLocalNPCImmunity = true;
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Spiffo spiffo = Main.LocalPlayer.GetModPlayer<Spiffo>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SpiffoPlush")
                        .Replace("<ammoReserve>", spiffo.ammoReserveChance.ToString())
                        .Replace("<armorPen>", spiffo.zombieArmorPen.ToString())
                        .Replace("<penChance>", spiffo.penetrateChance.ToString())
                        ));
        }
    }
}
