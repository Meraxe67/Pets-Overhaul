using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class DynamiteKitten : PetEffect
    {
        public int cooldown = 120;
        public float damageMult = 0.6f;
        public float kbMult = 1.7f;
        public int armorPen = 15;
        public int explosionSize = 200;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BallOfFuseWire))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BallOfFuseWire) && Pet.timer <= 0)
            {
                Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Vector2.Zero, ModContent.ProjectileType<PetExplosion>(), (int)(damageDone * damageMult), hit.Knockback * kbMult, Player.whoAmI, explosionSize)
                    .OriginalArmorPenetration += armorPen;
                if (ModContent.GetInstance<Personalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(SoundID.Item14,target.Center);
                }
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class BallOfFuseWire : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BallOfFuseWire;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            DynamiteKitten dynamiteKitten = Main.LocalPlayer.GetModPlayer<DynamiteKitten>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BallOfFuseWire")
                .Replace("<class>", PetTextsColors.ClassText(dynamiteKitten.PetClassPrimary, dynamiteKitten.PetClassSecondary))
                        .Replace("<kb>", dynamiteKitten.kbMult.ToString())
                        .Replace("<dmg>", dynamiteKitten.damageMult.ToString())
                        .Replace("<armorPen>", dynamiteKitten.armorPen.ToString())
                        .Replace("<size>", dynamiteKitten.explosionSize.ToString())
                        ));
        }
    }
}
