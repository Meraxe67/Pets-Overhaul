using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Parrot : PetEffect
    {
        public int projChance = 22;
        public int meleeChance = 28;
        public float projDamage = 0.75f;
        public float meleeDamage = 0.75f;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ParrotCracker))
            {
                for (int i = 0; i < GlobalPet.Randomizer(meleeChance); i++)
                {
                    target.SimpleStrikeNPC((int)(hit.SourceDamage * meleeDamage), hit.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance(hit.DamageType), 100), 100), 0, hit.DamageType, true, Player.luck);
                    PlayParrotSound();
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ParrotCracker) && (proj.minion || proj.sentry || proj.usesOwnerMeleeHitCD || proj.ownerHitCheck || proj.type == ProjectileID.TrueNightsEdge))
            {
                for (int i = 0; i < GlobalPet.Randomizer(meleeChance); i++)
                {
                    target.SimpleStrikeNPC((int)(hit.SourceDamage * projDamage), hit.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance(hit.DamageType), 100), 100), 0, hit.DamageType, true, Player.luck);
                    PlayParrotSound();
                }
            }
        }

        public void PlayParrotSound()
        {
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
            {
                SoundStyle style = default;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        style = SoundID.Zombie78;
                        break;
                    case 1:
                        style = SoundID.Cockatiel;
                        break;
                    case 2:
                        style = SoundID.Macaw;
                        break;
                }
                SoundEngine.PlaySound(in style with
                {
                    PitchVariance = 2f,
                    MaxInstances = 1,
                    SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                    Volume = 0.2f
                }, Player.Center);
            }
        }
    }
    public sealed class ParrotExtraProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.player[projectile.owner].TryGetModPlayer(out Parrot parrot) && parrot.Pet.PetInUseWithSwapCd(ItemID.ParrotCracker) && projectile.usesOwnerMeleeHitCD == false && projectile.ownerHitCheck == false && projectile.damage > 0 && projectile.type != ProjectileID.TrueNightsEdge && projectile.minion == false && projectile.sentry == false) //We check if Projectile is a 'melee' projectile, or a direct 'melee hit' of a Minion/Sentry. True Nights Edge does not use usesOwnerMeleeHitCD or ownerHitCheck, reason for its exception.
            {
                DamageClass damageType = DamageClass.Default;
                if (source is EntitySource_ItemUse entity1 && entity1.Item is not null)
                {
                    damageType = entity1.Item.DamageType;
                }
                else if (source is EntitySource_Parent parent && parent.Entity is Projectile proj1 && (proj1.minion || proj1.sentry))
                {
                    damageType = proj1.DamageType;
                }

                if (source is EntitySource_ItemUse || source is EntitySource_Parent { Entity: Projectile entity } && (entity.minion || entity.sentry))
                {
                    for (int i = 0; i < GlobalPet.Randomizer(parrot.projChance); i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), projectile.Center, projectile.velocity.RotateRandom(0.5f), projectile.type, (int)(projectile.damage * parrot.projDamage), projectile.knockBack, projectile.owner);
                        petProjectile.DamageType = damageType;
                        parrot.PlayParrotSound();
                    }
                }
            }
        }
    }
    public sealed class ParrotCracker : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ParrotCracker;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            Parrot parrot = Main.LocalPlayer.GetModPlayer<Parrot>();
            tooltips.Add(new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ParrotCracker")
                .Replace("<class>", PetTextsColors.ClassText(parrot.PetClassPrimary, parrot.PetClassSecondary))
                .Replace("<projChance>", parrot.projChance.ToString())
                .Replace("<projDamage>", parrot.projDamage.ToString())
                .Replace("<meleeChance>", parrot.meleeChance.ToString())
                .Replace("<meleeDamage>", parrot.meleeDamage.ToString())
                ));
        }
    }
}
