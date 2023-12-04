using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Parrot : ModPlayer
    {
        public int projChance = 22;
        public int meleeChance = 28;
        public float projDamage = 0.75f;
        public float meleeDamage = 0.75f;
        public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); private set { } }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ParrotCracker))
            {
                for (int i = 0; i < ItemPet.Randomizer(meleeChance); i++)
                {
                    target.StrikeNPC(hit with { Damage = (int)(damageDone * meleeDamage) });
                    PlayParrotSound();
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.ParrotCracker) && (proj.minion || proj.sentry))
            {
                for (int i = 0; i < ItemPet.Randomizer(meleeChance); i++)
                {
                    target.StrikeNPC(hit with { Damage = (int)(damageDone * meleeDamage) });
                    PlayParrotSound();
                }
            }
        }

        public void PlayParrotSound()
        {
            if (!ModContent.GetInstance<Personalization>().AbilitySoundDisabled)
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
                SoundEngine.PlaySound(in style, Player.position);
            }
        }
    }
    public sealed class ParrotExtraProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.damage > 0 && ((!projectile.minion || !projectile.sentry) && source is EntitySource_ItemUse || source is EntitySource_Parent { Entity: Projectile entity } && (entity.minion || entity.sentry)) && Main.player[projectile.owner].TryGetModPlayer(out Parrot parrot) && parrot.Pet.PetInUseWithSwapCd(ItemID.ParrotCracker))
            {
                for (int i = 0; i < ItemPet.Randomizer(parrot.projChance); i++)
                {
                    Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), projectile.position, projectile.velocity * Main.rand.NextFloat(0.9f, 1.1f), projectile.type, (int)(projectile.damage * parrot.projDamage), projectile.knockBack, projectile.owner);
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
            if (!ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift || PlayerInput.Triggers.Current.KeyStatus["Down"])
            {
                Parrot modPlayer = Main.LocalPlayer.GetModPlayer<Parrot>();
                tooltips.Add(new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ParrotCracker")
                    .Replace("<projChance>", modPlayer.projChance.ToString())
                    .Replace("<projDamage>", modPlayer.projDamage.ToString())
                    .Replace("<meleeChance>", modPlayer.meleeChance.ToString())
                    .Replace("<meleeDamage>", modPlayer.meleeDamage.ToString())));
            }
        }
    }
}
