using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace PetsOverhaul.PetEffects
{
    public sealed class PhantasmalDragon : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public int phantasmDragonCooldown = 120;
        public int iceBase = 400;
        public int lightningBase = 300;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.LunaticCultistPetItem))
            {
                Pet.SetPetAbilityTimer(phantasmDragonCooldown);
            }
        }
        public int Damage(int dmg)
        {
            return Main.DamageVar(Player.GetTotalDamage<GenericDamageClass>().ApplyTo(dmg), Player.luck);
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.LunaticCultistPetItem) && Pet.timer <= 0 && Keybinds.UsePetAbility.JustPressed)
            {
                Pet.timer = Pet.timerMax;
                switch (Main.rand.Next(2))
                {
                    case 0: //Ice
                        Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                        int damage = Main.DamageVar(Player.GetTotalDamage<GenericDamageClass>().ApplyTo(iceBase), Player.luck); //ai 88
                        if (velocity.X >= 0)
                            velocity.X = Math.Clamp(velocity.X, 3f, 5f);
                        else if (velocity.X <= 0)
                            velocity.X = Math.Clamp(velocity.X, -5f, -3f);
                        if (velocity.Y >= 0)
                            velocity.Y = Math.Clamp(velocity.Y, 3f, 5f);
                        else if (velocity.Y <= 0)
                            velocity.Y = Math.Clamp(velocity.Y, -5f, -3f);
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile, "Phantasmal"), Main.MouseWorld, velocity, ProjectileID.CultistBossIceMist, Damage(iceBase), 0, Player.whoAmI, 0f, 1f)
                            .netUpdate = true;
                        break;
                    case 1: //Lightning
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile, "Phantasmal"), Main.MouseWorld, Vector2.Zero, ProjectileID.CultistBossLightningOrb, Damage(lightningBase), 0, Player.whoAmI, 0f)
                            .netUpdate = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public sealed class LunaticCultistFriendlyProjectiles : GlobalProjectile //NOTE: Could not run the projectiles on Server properly, other players currently cannot see the projectiles, but they work fine in Multiplayer.
    {
        public override bool InstancePerEntity => true;
        public bool fromPhantasmalPet = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if ((source is EntitySource_Pet dragonProj && dragonProj.ContextType == EntitySourcePetIDs.PetProjectile && dragonProj.Context == "Phantasmal") ||
                (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.TryGetGlobalProjectile(out LunaticCultistFriendlyProjectiles result) && result.fromPhantasmalPet))
            {
                fromPhantasmalPet = true;
            }
        }
        public override void PostAI(Projectile projectile)
        {
            if (fromPhantasmalPet)
            {
                if (Main.netMode != NetmodeID.SinglePlayer && projectile.type == ProjectileID.CultistBossIceMist && projectile.ai[1] == 1f && projectile.ai[0] % 30f == 0f)
                {
                    Vector2 vector = projectile.rotation.ToRotationVector2();
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector.X, vector.Y, 464, projectile.damage, projectile.knockBack, projectile.owner);
                }
                projectile.friendly = true;
                projectile.hostile = false;
                projectile.netUpdate = true;
                if (projectile.type == ProjectileID.CultistBossLightningOrbArc || projectile.type == ProjectileID.CultistBossLightningOrb)
                {
                    projectile.penetrate = -1;
                }
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            if (fromPhantasmalPet) //Lightning is copy paste of Vanilla code with a few changes
            {
                Vector2 val4;
                if (projectile.type == ProjectileID.CultistBossLightningOrb)
                {
                    if (projectile.localAI[1] == 0f)
                    {
                        SoundEngine.PlaySound(in SoundID.Item121, projectile.position);
                        projectile.localAI[1] = 1f;
                    }
                    if (projectile.ai[0] < 180f)
                    {
                        projectile.alpha -= 5;
                        if (projectile.alpha < 0)
                        {
                            projectile.alpha = 0;
                        }
                    }
                    else
                    {
                        projectile.alpha += 5;
                        if (projectile.alpha > 255)
                        {
                            projectile.alpha = 255;
                            projectile.Kill();
                            return false;
                        }
                    }
                    projectile.ai[0]++;
                    if (projectile.ai[0] % 30f == 0f && projectile.ai[0] < 180f)
                    {
                        int[] array3 = new int[5];
                        Vector2[] array4 = (Vector2[])(object)new Vector2[5];
                        int num808 = 0;
                        float num809 = 2000f;
                        for (int num811 = 0; num811 < 255; num811++)
                        {
                            if (!Main.player[num811].active || Main.player[num811].dead)
                            {
                                continue;
                            }
                            Vector2 center6 = Main.MouseWorld; //Changed Main.player[num811].Center to Main.MouseWorld
                            float num812 = Vector2.Distance(center6, projectile.Center);
                            if (num812 < num809 && Collision.CanHit(projectile.Center, 1, 1, center6, 1, 1))
                            {
                                array3[num808] = num811;
                                array4[num808] = center6;
                                int num388 = num808 + 1;
                                num808 = num388;
                                if (num388 >= array4.Length)
                                {
                                    break;
                                }
                            }
                        }
                        for (int num813 = 0; num813 < num808; num813++)
                        {
                            Vector2 vector162 = array4[num813] - projectile.Center;
                            float ai = Main.rand.Next(100);
                            Vector2 vector163 = Vector2.Normalize(vector162.RotatedByRandom(0.7853981852531433)) * 7f;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector163.X, vector163.Y, ProjectileID.CultistBossLightningOrbArc, projectile.damage, 0f, projectile.owner, vector162.ToRotation(), ai); //Changed ID for readability and the WhoAmI to .owner
                        }
                    }
                    Lighting.AddLight(projectile.Center, 0.4f, 0.85f, 0.9f);
                    if (++projectile.frameCounter >= 4)
                    {
                        projectile.frameCounter = 0;
                        if (++projectile.frame >= Main.projFrames[projectile.type])
                        {
                            projectile.frame = 0;
                        }
                    }
                    if (projectile.alpha >= 150 || !(projectile.ai[0] < 180f))
                    {
                        return false;
                    }
                    for (int num814 = 0; num814 < 1; num814++)
                    {
                        float num815 = (float)Main.rand.NextDouble() * 1f - 0.5f;
                        if (num815 < -0.5f)
                        {
                            num815 = -0.5f;
                        }
                        if (num815 > 0.5f)
                        {
                            num815 = 0.5f;
                        }
                        Vector2 spinningpoint56 = new Vector2((float)(-projectile.width) * 0.2f * projectile.scale, 0f);
                        double radians43 = num815 * ((float)Math.PI * 2f);
                        val4 = default(Vector2);
                        Vector2 spinningpoint57 = Utils.RotatedBy(spinningpoint56, radians43, val4);
                        double radians44 = projectile.velocity.ToRotation();
                        val4 = default(Vector2);
                        Vector2 vector164 = spinningpoint57.RotatedBy(radians44, val4);
                        int num816 = Dust.NewDust(projectile.Center - Vector2.One * 5f, 10, 10, 226, (0f - projectile.velocity.X) / 3f, (0f - projectile.velocity.Y) / 3f, 150, Color.Transparent, 0.7f);
                        Main.dust[num816].position = projectile.Center + vector164;
                        Main.dust[num816].velocity = Vector2.Normalize(Main.dust[num816].position - projectile.Center) * 2f;
                        Main.dust[num816].noGravity = true;
                    }
                    for (int num817 = 0; num817 < 1; num817++)
                    {
                        float num818 = (float)Main.rand.NextDouble() * 1f - 0.5f;
                        if (num818 < -0.5f)
                        {
                            num818 = -0.5f;
                        }
                        if (num818 > 0.5f)
                        {
                            num818 = 0.5f;
                        }
                        Vector2 spinningpoint58 = new Vector2((float)(-projectile.width) * 0.6f * projectile.scale, 0f);
                        double radians45 = num818 * ((float)Math.PI * 2f);
                        val4 = default(Vector2);
                        Vector2 spinningpoint59 = Utils.RotatedBy(spinningpoint58, radians45, val4);
                        double radians46 = projectile.velocity.ToRotation();
                        val4 = default(Vector2);
                        Vector2 vector165 = spinningpoint59.RotatedBy(radians46, val4);
                        int num819 = Dust.NewDust(projectile.Center - Vector2.One * 5f, 10, 10, 226, (0f - projectile.velocity.X) / 3f, (0f - projectile.velocity.Y) / 3f, 150, Color.Transparent, 0.7f);
                        Main.dust[num819].velocity = Vector2.Zero;
                        Main.dust[num819].position = projectile.Center + vector165;
                        Main.dust[num819].noGravity = true;
                    }
                    return false;
                }
                else if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    projectile.frameCounter++;
                    Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
                    if (projectile.velocity == Vector2.Zero)
                    {
                        if (projectile.frameCounter >= projectile.extraUpdates * 2)
                        {
                            projectile.frameCounter = 0;
                            bool flag29 = true;
                            for (int num820 = 1; num820 < projectile.oldPos.Length; num820++)
                            {
                                if (projectile.oldPos[num820] != projectile.oldPos[0])
                                {
                                    flag29 = false;
                                }
                            }
                            if (flag29)
                            {
                                projectile.Kill();
                                return false;
                            }
                        }
                        if (Main.rand.Next(projectile.extraUpdates) == 0)
                        {
                            Vector2 vector166 = default(Vector2);
                            for (int num822 = 0; num822 < 2; num822++)
                            {
                                float num823 = projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                                float num824 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                                vector166 = new Vector2((float)Math.Cos(num823) * num824, (float)Math.Sin(num823) * num824);
                                int num825 = Dust.NewDust(projectile.Center, 0, 0, 226, vector166.X, vector166.Y);
                                Main.dust[num825].noGravity = true;
                                Main.dust[num825].scale = 1.2f;
                            }
                            if (Main.rand.Next(5) == 0)
                            {
                                Vector2 spinningpoint60 = projectile.velocity;
                                val4 = default(Vector2);
                                Vector2 vector168 = spinningpoint60.RotatedBy(1.5707963705062866, val4) * ((float)Main.rand.NextDouble() - 0.5f) * (float)projectile.width;
                                int num826 = Dust.NewDust(projectile.Center + vector168 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                                Dust dust137 = Main.dust[num826];
                                Dust dust212 = dust137;
                                dust212.velocity *= 0.5f;
                                Main.dust[num826].velocity.Y = 0f - Math.Abs(Main.dust[num826].velocity.Y);
                            }
                        }
                    }
                    else
                    {
                        if (projectile.frameCounter < projectile.extraUpdates * 2)
                        {
                            return false;
                        }
                        projectile.frameCounter = 0;
                        float num827 = projectile.velocity.Length();
                        UnifiedRandom unifiedRandom = new UnifiedRandom((int)projectile.ai[1]);
                        int num828 = 0;
                        Vector2 spinningpoint7 = -Vector2.UnitY;
                        while (true)
                        {
                            int num829 = unifiedRandom.Next();
                            projectile.ai[1] = num829;
                            num829 %= 100;
                            float f = (float)num829 / 100f * ((float)Math.PI * 2f);
                            Vector2 vector169 = f.ToRotationVector2();
                            if (vector169.Y > 0f)
                            {
                                vector169.Y *= -1f;
                            }
                            bool flag30 = false;
                            if (vector169.Y > -0.02f)
                            {
                                flag30 = true;
                            }
                            if (vector169.X * (float)(projectile.extraUpdates + 1) * 2f * num827 + projectile.localAI[0] > 40f)
                            {
                                flag30 = true;
                            }
                            if (vector169.X * (float)(projectile.extraUpdates + 1) * 2f * num827 + projectile.localAI[0] < -40f)
                            {
                                flag30 = true;
                            }
                            if (flag30)
                            {
                                if (num828++ >= 100)
                                {
                                    projectile.velocity = Vector2.Zero;
                                    projectile.localAI[1] = 1f;
                                    break;
                                }
                                continue;
                            }
                            spinningpoint7 = vector169;
                            break;
                        }
                        if (projectile.velocity != Vector2.Zero)
                        {
                            projectile.localAI[0] += spinningpoint7.X * (float)(projectile.extraUpdates + 1) * 2f * num827;
                            Vector2 spinningpoint61 = spinningpoint7;
                            double radians47 = projectile.ai[0] + (float)Math.PI / 2f;
                            val4 = default;
                            projectile.velocity = spinningpoint61.RotatedBy(radians47, val4) * num827;
                            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                        }
                    }
                    return false;
                }
            }
            return true;
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
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PhantasmalDragon phantasmalDragon = Main.LocalPlayer.GetModPlayer<PhantasmalDragon>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.LunaticCultistPetItem")
                .Replace("<class>", PetTextsColors.ClassText(phantasmalDragon.PetClassPrimary, phantasmalDragon.PetClassSecondary))
                       .Replace("<cooldown>", Math.Round(phantasmalDragon.phantasmDragonCooldown / 60f, 2).ToString())
                       ));
        }
    }
}
