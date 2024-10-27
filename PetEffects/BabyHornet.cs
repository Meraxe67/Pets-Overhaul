using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyHornet : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public int beeCooldown = 90;
        public int beeDmg = 6;
        public float beeKb = 0.9f;
        public float dmgReduction = 0.04f;
        public int defReduction = 4;
        public int maxMinion = 1;
        public int critReduction = 4;
        public float moveSpdIncr = 0.04f;
        public int beeChance = 7;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.Nectar))
            {
                Pet.SetPetAbilityTimer(beeCooldown);
            }
        }
        public override void PreUpdateBuffs()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Nectar))
            {
                Player.buffImmune[BuffID.Poisoned] = true;
                Player.buffImmune[BuffID.Venom] = true;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Nectar))
            {
                Player.statDefense -= defReduction;
                Player.GetDamage<GenericDamageClass>() -= dmgReduction;
                Player.GetCritChance<GenericDamageClass>() -= critReduction;
                Player.moveSpeed += moveSpdIncr;
                Player.maxMinions += maxMinion;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Nectar) && Pet.timer <= 0)
            {
                int summonMult = 1;
                if (hit.DamageType == DamageClass.Summon || hit.DamageType == DamageClass.SummonMeleeSpeed || hit.DamageType == DamageClass.MagicSummonHybrid)
                {
                    summonMult = 2;
                }
                for (int i = 0; i < GlobalPet.Randomizer(beeChance * summonMult); i++)
                {
                    if (Player.strongBees == true && Main.rand.NextBool(1, 3))
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2CircularEdge(7f, 7f), ProjectileID.GiantBee, beeDmg * 2, beeKb * 2, Player.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2CircularEdge(7f, 7f), ProjectileID.Bee, beeDmg, beeKb, Player.whoAmI);
                    }
                    Pet.timer = Pet.timerMax;
                }

            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.timer <= 0 && Pet.PetInUseWithSwapCd(ItemID.Nectar) && proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj == false)
            {
                int summonMult = 1;
                if (hit.DamageType == DamageClass.Summon || hit.DamageType == DamageClass.SummonMeleeSpeed || hit.DamageType == DamageClass.MagicSummonHybrid)
                {
                    summonMult = 2;
                }
                for (int i = 0; i < GlobalPet.Randomizer(beeChance * summonMult); i++)
                {
                    if (Player.strongBees == true && Main.rand.NextBool(1, 3))
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2CircularEdge(7f, 7f), ProjectileID.GiantBee, beeDmg * 2, beeKb * 2, Player.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Main.rand.NextVector2CircularEdge(7f, 7f), ProjectileID.Bee, beeDmg, beeKb, Player.whoAmI);
                    }
                    Pet.timer = Pet.timerMax;
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
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyHornet babyHornet = Main.LocalPlayer.GetModPlayer<BabyHornet>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Nectar")
                .Replace("<class>", PetTextsColors.ClassText(babyHornet.PetClassPrimary, babyHornet.PetClassSecondary))
                .Replace("<moveSpd>", Math.Round(babyHornet.moveSpdIncr * 100, 2).ToString())
                .Replace("<def>", babyHornet.defReduction.ToString())
                .Replace("<dmgCrit>", Math.Round(babyHornet.dmgReduction * 100, 2).ToString())
                .Replace("<maxMinion>", babyHornet.maxMinion.ToString())
                .Replace("<regularChance>", babyHornet.beeChance.ToString())
                .Replace("<summonChance>", (babyHornet.beeChance * 2).ToString())
                .Replace("<beeDmg>", babyHornet.beeDmg.ToString())
                .Replace("<beeKb>", babyHornet.beeKb.ToString())
                .Replace("<beeCd>", Math.Round(babyHornet.beeCooldown / 60f, 2).ToString())
            ));
        }
    }
}
