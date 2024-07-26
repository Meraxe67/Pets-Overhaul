using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class PhantasmalDragon : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public int phantasmDragonCooldown = 120;
        public int iceFlat = 150;
        public float iceMult = 0.2f;
        public int IceDamage(int damageDone)
        {
            int dmg = (int)(damageDone * iceMult) + iceFlat;
            return dmg;
        }
        public int fireFlat = 250;
        public float fireMult = 0.22f;
        public int FireDamage(int damageDone)
        {
            int dmg = (int)(damageDone * fireMult) + fireFlat;
            return dmg;
        }
        public int lightFlat = 100;
        public float lightMult = 0.13f;
        public int LightDamage(int damageDone)
        {
            int dmg = (int)(damageDone * lightMult) + lightFlat;
            return dmg;
        }
        public int icePierce = 25;
        public int lightPierce = 10;
        public float iceSlowAmount = 2.5f;
        public int iceSlowTime = 600;
        public float nerfOnNonPrimary = 0.33f;
        internal int iceIndex = -1;
        public override void PreUpdate()
        {
            if (iceIndex >= 0 && Main.projectile[iceIndex].active == false)
            {
                iceIndex = -1;
            }
            if (Pet.PetInUse(ItemID.LunaticCultistPetItem))
            {
                Pet.timerMax = phantasmDragonCooldown;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LunaticCultistPetItem) && Pet.timer <= 0 && proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj == false)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        int ice = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), new Vector2(target.Center.X, target.Center.Y), Main.rand.NextVector2Circular(5f, 5f), ProjectileID.CultistBossIceMist, IceDamage(damageDone), hit.Knockback, Player.whoAmI);
                        Main.projectile[ice].friendly = true;
                        Main.projectile[ice].hostile = false;
                        Main.projectile[ice].tileCollide = false;
                        Main.projectile[ice].netImportant = true;
                        Main.projectile[ice].penetrate = icePierce;
                        target.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.PhantasmalIce, iceSlowAmount, iceSlowTime, target);
                        iceIndex = ice;
                        break;
                    case 1:
                        for (int i = 0; i < 4; i++)
                        {
                            if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit55 with { PitchVariance = 0.5f, MaxInstances = 10, Volume = 0.2f }, Player.position);
                            }

                            Dust.NewDustDirect(new Vector2(Player.Center.X + Main.rand.NextFloat(-25f, 25f), Player.Center.Y - 400f), 25, 25, DustID.FlameBurst, 0, 0, 25);
                            int fire = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), new Vector2(target.Center.X + Main.rand.NextFloat(-25f, 25f), target.Center.Y - 400f), new Vector2(Main.rand.NextFloat(5f, -5f), 7f), ProjectileID.CultistBossFireBall, FireDamage(damageDone), hit.Knockback, Player.whoAmI);
                            Main.projectile[fire].friendly = true;
                            Main.projectile[fire].hostile = false;
                            Main.projectile[fire].tileCollide = true;
                            Main.projectile[fire].netImportant = true;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < 5; i++)
                        {
                            if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                            {
                                SoundEngine.PlaySound(SoundID.Zombie90 with { PitchVariance = 0.5f, MaxInstances = 10, Volume = 0.3f }, Player.position);
                            }

                            int light = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), new Vector2(target.Center.X - Main.rand.Next(-300, 300), target.Center.Y - Main.rand.Next(-300, 300)), Vector2.Zero, ProjectileID.HallowBossSplitShotCore, LightDamage(damageDone), hit.Knockback, Player.whoAmI);
                            Main.projectile[light].friendly = true;
                            Main.projectile[light].hostile = false;
                            Main.projectile[light].tileCollide = false;
                            Main.projectile[light].netImportant = true;
                            Main.projectile[light].penetrate = lightPierce;
                        }
                        break;

                }
                Pet.timer = Pet.timerMax;
            }
            if (Pet.PetInUseWithSwapCd(ItemID.LunaticCultistPetItem) && proj.whoAmI == iceIndex && proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj)
            {
                target.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.PhantasmalIce, iceSlowAmount * nerfOnNonPrimary, iceSlowTime, target);
            }

        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LunaticCultistPetItem) && Pet.timer <= 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        int ice = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), new Vector2(target.Center.X, target.Center.Y), Main.rand.NextVector2Circular(5f, 5f), ProjectileID.CultistBossIceMist, IceDamage(damageDone), hit.Knockback, Player.whoAmI);
                        Main.projectile[ice].friendly = true;
                        Main.projectile[ice].hostile = false;
                        Main.projectile[ice].tileCollide = false;
                        Main.projectile[ice].netImportant = true;
                        Main.projectile[ice].penetrate = icePierce;
                        break;
                    case 1:
                        for (int i = 0; i < 4; i++)
                        {
                            if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit55 with { PitchVariance = 0.5f, MaxInstances = 10, Volume = 0.2f }, Player.position);
                            }

                            Dust.NewDustDirect(new Vector2(Player.Center.X + Main.rand.NextFloat(-25f, 25f), Player.Center.Y - 400f), 25, 25, DustID.FlameBurst, 0, 0, 25);
                            int fire = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), new Vector2(target.Center.X + Main.rand.NextFloat(-25f, 25f), target.Center.Y - 400f), new Vector2(Main.rand.NextFloat(5f, -5f), 7f), ProjectileID.CultistBossFireBall, FireDamage(damageDone), hit.Knockback, Player.whoAmI);
                            Main.projectile[fire].friendly = true;
                            Main.projectile[fire].hostile = false;
                            Main.projectile[fire].tileCollide = true;
                            Main.projectile[fire].netImportant = true;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < 5; i++)
                        {
                            if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                            {
                                SoundEngine.PlaySound(SoundID.Zombie90 with { PitchVariance = 0.5f, MaxInstances = 10, Volume = 0.3f }, Player.position);
                            }

                            int light = Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.petProjectile), new Vector2(target.Center.X - Main.rand.Next(-300, 300), target.Center.Y - Main.rand.Next(-300, 300)), Vector2.Zero, ProjectileID.HallowBossSplitShotCore, LightDamage(damageDone), hit.Knockback, Player.whoAmI);
                            Main.projectile[light].friendly = true;
                            Main.projectile[light].hostile = false;
                            Main.projectile[light].tileCollide = false;
                            Main.projectile[light].netImportant = true;
                            Main.projectile[light].penetrate = lightPierce;
                        }
                        break;

                }
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class LunaticCultistPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.LunaticCultistPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PhantasmalDragon phantasmalDragon = Main.LocalPlayer.GetModPlayer<PhantasmalDragon>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LunaticCultistPetItem")
                .Replace("<class>", PetColors.ClassText(phantasmalDragon.PetClassPrimary, phantasmalDragon.PetClassSecondary))
                       .Replace("<cooldown>", Math.Round(phantasmalDragon.phantasmDragonCooldown / 60f, 2).ToString())
                       .Replace("<icePierce>", phantasmalDragon.icePierce.ToString())
                       .Replace("<icePercent>", Math.Round(phantasmalDragon.iceMult * 100, 2).ToString())
                       .Replace("<iceFlat>", phantasmalDragon.iceFlat.ToString())
                       .Replace("<iceSlow>", Math.Round(phantasmalDragon.iceSlowAmount * phantasmalDragon.nerfOnNonPrimary * 100, 2).ToString())
                       .Replace("<iceMainSlow>", Math.Round(phantasmalDragon.iceSlowAmount * 100, 2).ToString())
                       .Replace("<iceSlowTime>", Math.Round(phantasmalDragon.iceSlowTime / 60f, 2).ToString())
                       .Replace("<lightPercent>", Math.Round(phantasmalDragon.lightMult * 100, 2).ToString())
                       .Replace("<lightFlat>", phantasmalDragon.lightFlat.ToString())
                       .Replace("<lightPierce>", phantasmalDragon.lightPierce.ToString())
                       .Replace("<firePercent>", Math.Round(phantasmalDragon.fireMult * 100, 2).ToString())
                       .Replace("<fireFlat>", phantasmalDragon.fireFlat.ToString())
                       ));
        }
    }
}
