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
    public sealed class BabyHornet : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public int nectarCooldown = 360;
        public int beeDmg = 6;
        public float beeKb = 0.1f;
        public float dmgReduction = 0.03f;
        public int defReduction = 3;
        public int maxMinion = 1;
        public int critReduction = 3;
        public float moveSpdIncr = 0.03f;
        public float healthRecovery = 0.05f;
        public int beeChance = 7;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.Nectar))
            {
                Pet.SetPetAbilityTimer(nectarCooldown);
            }
        }
        public override void PreUpdateBuffs()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Nectar))
            {

                Player.buffImmune[BuffID.Poisoned] = false;
                Player.buffImmune[BuffID.Venom] = false;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Nectar))
            {

                if (Player.HasBuff(BuffID.Poisoned) == true || Player.HasBuff(BuffID.Venom) == true)
                {
                    if (Pet.timer <= 0)
                    {
                        Pet.PetRecovery(Player.statLifeMax2, healthRecovery, isLifesteal: false);
                        if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                        {
                            SoundEngine.PlaySound(SoundID.Item97 with { Pitch = 0.4f, MaxInstances = 10 });
                        }

                        Pet.timer = Pet.timerMax;
                    }
                    Player.ClearBuff(BuffID.Poisoned);
                    Player.ClearBuff(BuffID.Venom);
                }
                Player.statDefense -= defReduction;
                Player.GetDamage<GenericDamageClass>() -= dmgReduction;
                Player.GetCritChance<GenericDamageClass>() -= critReduction;
                Player.moveSpeed += moveSpdIncr;
                Player.maxMinions += maxMinion;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {

            int summonMult = 1;
            if (hit.DamageType == DamageClass.Summon || hit.DamageType == DamageClass.SummonMeleeSpeed || hit.DamageType == DamageClass.MagicSummonHybrid)
            {
                summonMult = 2;
            }

            if (Pet.PetInUseWithSwapCd(ItemID.Nectar))
            {
                for (int i = 0; i < GlobalPet.Randomizer(beeChance * summonMult); i++)
                {
                    if (Player.strongBees == true && Main.rand.NextBool(1, 3))
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2Circular(10f, 10f), ProjectileID.GiantBee, beeDmg * 2, beeKb * 2, Player.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2Circular(10f, 10f), ProjectileID.Bee, beeDmg, beeKb, Player.whoAmI);
                    }
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int summonMult = 1;
            if (hit.DamageType == DamageClass.Summon || hit.DamageType == DamageClass.SummonMeleeSpeed || hit.DamageType == DamageClass.MagicSummonHybrid)
            {
                summonMult = 2;
            }

            if (Pet.PetInUseWithSwapCd(ItemID.Nectar) && proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj == false)
            {
                for (int i = 0; i < GlobalPet.Randomizer(beeChance * summonMult); i++)
                {
                    if (Player.strongBees == true && Main.rand.NextBool(1, 3))
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2Circular(10f, 10f), ProjectileID.GiantBee, beeDmg * 2, beeKb * 2, Player.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2Circular(10f, 10f), ProjectileID.Bee, beeDmg, beeKb, Player.whoAmI);
                    }
                }
            }
        }
    }
    public sealed class Nectar : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Nectar;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyHornet babyHornet = Main.LocalPlayer.GetModPlayer<BabyHornet>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Nectar")
                .Replace("<class>", PetColors.ClassText(babyHornet.PetClassPrimary, babyHornet.PetClassSecondary))
                .Replace("<antidotePercent>", Math.Round(babyHornet.healthRecovery * 100, 2).ToString())
                .Replace("<antidoteCd>", Math.Round(babyHornet.nectarCooldown / 60f, 2).ToString())
                .Replace("<moveSpd>", Math.Round(babyHornet.moveSpdIncr * 100, 2).ToString())
                .Replace("<def>", babyHornet.defReduction.ToString())
                .Replace("<dmgCrit>", Math.Round(babyHornet.dmgReduction * 100, 2).ToString())
                .Replace("<maxMinion>", babyHornet.maxMinion.ToString())
                .Replace("<regularChance>", babyHornet.beeChance.ToString())
                .Replace("<summonChance>", (babyHornet.beeChance * 2).ToString())
                .Replace("<beeDmg>", babyHornet.beeDmg.ToString())
                .Replace("<beeKb>", babyHornet.beeKb.ToString())
            ));
        }
    }
}
