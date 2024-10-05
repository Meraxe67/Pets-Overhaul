using PetsOverhaul.Config;
using PetsOverhaul.Systems;
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
                    target.StrikeNPC(hit with { Damage = (int)(damageDone * meleeDamage) });
                    PlayParrotSound();
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ParrotCracker) && (proj.minion || proj.sentry || proj.usesOwnerMeleeHitCD))
            {
                for (int i = 0; i < GlobalPet.Randomizer(meleeChance); i++)
                {
                    target.StrikeNPC(hit with { Damage = (int)(damageDone * meleeDamage) });
                    PlayParrotSound();
                }
            }
        }

        public void PlayParrotSound()
        {
            if (!ModContent.GetInstance<Personalization>().AbilitySoundEnabled)
            {
                SoundStyle style = default;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        style = SoundID.Zombie78 with
                        {
                            PitchVariance = 1f,
                            MaxInstances = 1,
                            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                            Volume = 0.25f
                        };
                        break;
                    case 1:
                        style = SoundID.Cockatiel with
                        {
                            PitchVariance = 1f,
                            MaxInstances = 1,
                            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                            Volume = 0.25f
                        };
                        break;
                    case 2:
                        style = SoundID.Macaw with
                        {
                            PitchVariance = 1f,
                            MaxInstances = 1,
                            SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest,
                            Volume = 0.25f
                        };
                        break;
                }
                SoundEngine.PlaySound(in style, Player.Center);
            }
        }   
    }
    public sealed class ParrotExtraProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.usesOwnerMeleeHitCD == false && projectile.damage > 0 && ((!projectile.minion || !projectile.sentry) && source is EntitySource_ItemUse || source is EntitySource_Parent { Entity: Projectile entity } && (entity.minion || entity.sentry)) && Main.player[projectile.owner].TryGetModPlayer(out Parrot parrot) && parrot.Pet.PetInUseWithSwapCd(ItemID.ParrotCracker))
            {
                for (int i = 0; i < GlobalPet.Randomizer(parrot.projChance); i++)
                {
                    Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), projectile.Center, projectile.velocity * Main.rand.NextFloat(0.8f, 1.2f), projectile.type, (int)(projectile.damage * parrot.projDamage), projectile.knockBack, projectile.owner);
                    parrot.PlayParrotSound();
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
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
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
