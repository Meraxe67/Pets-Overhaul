using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabySnowman : PetEffect
    {
        public override int PetItemID => ItemID.ToySled;
        public int frostburnTime = 300;
        public float snowmanSlow = 0.3f;
        public int slowTime = 180;
        public int frostMult = 3;
        public int FrostArmorMult => Player.frostBurn ? frostMult : 1;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ToySled))
            {
                target.AddBuff(BuffID.Frostburn2, frostburnTime * FrostArmorMult);
                NpcPet.AddSlow(new NpcPet.PetSlow(snowmanSlow * FrostArmorMult, slowTime * FrostArmorMult, PetSlowIDs.Snowman), target);
            }
        }
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ToySled))
            {
                if (item.CountsAsClass(DamageClass.Melee) && !item.noMelee && !item.noUseGraphic && Main.rand.Next(2) == 0)
                {
                    if (Player.frostBurn) 
                    {
                        return;
                    }
                    int num19 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 135, Player.velocity.X * 0.2f + (float)(Player.direction * 3), Player.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                    Main.dust[num19].noGravity = true;
                    Main.dust[num19].velocity *= 0.7f;
                    Main.dust[num19].velocity.Y -= 0.5f;
                }
            }
        }
        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (projectile.active)
            {
                if (Pet.PetInUseWithSwapCd(ItemID.ToySled))
                {
                    if ((projectile.CountsAsClass(DamageClass.Melee) || projectile.CountsAsClass(DamageClass.Ranged)) && Player.frostBurn)
                    {
                        return;
                    }
                    if (projectile.friendly && !projectile.hostile && Main.rand.Next(2 * (1 + projectile.extraUpdates)) == 0 && projectile.damage > 0)
                    {
                        int num = Dust.NewDust(boxPosition, boxWidth, boxHeight, 135, projectile.velocity.X * 0.2f + (float)(projectile.direction * 3), projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].velocity *= 0.7f;
                        Main.dust[num].velocity.Y -= 0.5f;
                    }
                }
            }
        }
    }
    public sealed class ToySled : PetTooltip
    {
        public override PetEffect PetsEffect => babySnowman;
        public static BabySnowman babySnowman
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BabySnowman pet))
                    return pet;
                else
                    return ModContent.GetInstance<BabySnowman>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ToySled")
                .Replace("<frostburnTime>", Math.Round(babySnowman.frostburnTime / 60f * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<slowAmount>", Math.Round(babySnowman.snowmanSlow * 100 * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<slowTime>", Math.Round(babySnowman.slowTime / 60f * babySnowman.FrostArmorMult, 2).ToString())
                .Replace("<frostMult>", babySnowman.frostMult.ToString());
    }
}
