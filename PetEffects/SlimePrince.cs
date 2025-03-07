﻿using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SlimePrince : PetEffect //Pet will be reworked post 3.0 update
    {
        public override int PetItemID => ItemID.KingSlimePetItem;
        public float wetSpeed = 0.10f;
        public float wetDmg = 0.08f;
        public float wetDef = 0.08f;
        public float slimyKb = 1.5f;
        public float slimyJump = 2f;
        public float wetDealtLower = 0.93f;
        public float wetRecievedHigher = 1.08f;
        public float bonusKb = 1.5f;
        public float healthDmg = 0.015f;
        public int burnCap = 50;
        public Player slimeDual;
        public Player slimePrince;

        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public void PutOutTheBurn()
        {
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (GlobalPet.BurnDebuffs.Contains(Player.buffType[i]))
                {
                    Player.buffTime[i]--;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (GlobalPet.KingSlimePetActive(out slimePrince) && Player.HasBuff(BuffID.Wet))
            {
                Player.moveSpeed += slimePrince.GetModPlayer<SlimePrince>().wetSpeed;
                Player.GetDamage<GenericDamageClass>() += slimePrince.GetModPlayer<SlimePrince>().wetDmg;
                Player.statDefense *= slimePrince.GetModPlayer<SlimePrince>().wetDef + 1;
                PutOutTheBurn();
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && Player.HasBuff(BuffID.Wet))
            {
                Player.moveSpeed += slimeDual.GetModPlayer<DualSlime>().wetSpeed;
                Player.GetDamage<GenericDamageClass>() += slimeDual.GetModPlayer<DualSlime>().wetDmg;
                Player.statDefense *= slimeDual.GetModPlayer<DualSlime>().wetDef + 1;
                PutOutTheBurn();
            }
            if (GlobalPet.KingSlimePetActive(out slimePrince) && Player.HasBuff(BuffID.Slimed))
            {
                Player.GetKnockback<GenericDamageClass>() *= slimePrince.GetModPlayer<SlimePrince>().slimyKb;
                Player.jumpSpeedBoost += slimePrince.GetModPlayer<SlimePrince>().slimyJump;
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && Player.HasBuff(BuffID.Slimed))
            {
                Player.GetKnockback<GenericDamageClass>() *= slimeDual.GetModPlayer<DualSlime>().slimyKb;
                Player.jumpSpeedBoost += slimeDual.GetModPlayer<DualSlime>().slimyJump;
            }
        }
    }
    public sealed class SlimePrinceandDualNpc : GlobalNPC
    {
        public Player slimeDual;
        public Player slimePrince;
        public override bool InstancePerEntity => true;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            void BurnAmp(Player slime)
            {
                for (int i = 0; i < NPC.maxBuffs; i++)
                {
                    if (GlobalPet.BurnDebuffs.Contains(npc.buffType[i]))
                    {
                        if (npc.lifeMax * slime.GetModPlayer<SlimePrince>().healthDmg > slime.GetModPlayer<SlimePrince>().burnCap)
                        {
                            npc.lifeRegen -= slime.GetModPlayer<SlimePrince>().burnCap;
                        }
                        else
                        {
                            npc.lifeRegen -= (int)(npc.lifeMax * slime.GetModPlayer<SlimePrince>().healthDmg);
                        }
                    }
                }
            }
            if (GlobalPet.KingSlimePetActive(out slimePrince) && npc.HasBuff(BuffID.Slimed))
            {
                BurnAmp(slimePrince);
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && npc.HasBuff(BuffID.Slimed))
            {
                BurnAmp(slimeDual);
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (GlobalPet.KingSlimePetActive(out slimePrince) && npc.HasBuff(BuffID.Wet))
            {
                modifiers.FinalDamage *= slimePrince.GetModPlayer<SlimePrince>().wetDealtLower;
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && npc.HasBuff(BuffID.Wet))
            {
                modifiers.FinalDamage *= slimeDual.GetModPlayer<DualSlime>().wetDealtLower;
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (GlobalPet.KingSlimePetActive(out slimePrince) && npc.HasBuff(BuffID.Wet))
            {
                modifiers.FinalDamage *= slimePrince.GetModPlayer<SlimePrince>().wetRecievedHigher;
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && npc.HasBuff(BuffID.Wet))
            {
                modifiers.FinalDamage *= slimeDual.GetModPlayer<DualSlime>().wetRecievedHigher;
            }
            if (GlobalPet.KingSlimePetActive(out slimePrince) && npc.HasBuff(BuffID.Slimed))
            {
                modifiers.Knockback *= slimePrince.GetModPlayer<SlimePrince>().bonusKb;
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && npc.HasBuff(BuffID.Slimed))
            {
                modifiers.Knockback *= slimeDual.GetModPlayer<DualSlime>().bonusKb;
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (GlobalPet.KingSlimePetActive(out slimePrince) && npc.HasBuff(BuffID.Wet))
            {
                modifiers.FinalDamage *= slimePrince.GetModPlayer<SlimePrince>().wetRecievedHigher;
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && npc.HasBuff(BuffID.Wet))
            {
                modifiers.FinalDamage *= slimeDual.GetModPlayer<DualSlime>().wetRecievedHigher;
            }
            if (GlobalPet.KingSlimePetActive(out slimePrince) && npc.HasBuff(BuffID.Slimed))
            {
                modifiers.Knockback *= slimePrince.GetModPlayer<SlimePrince>().bonusKb;
            }
            else if (GlobalPet.DualSlimePetActive(out slimeDual) && npc.HasBuff(BuffID.Slimed))
            {
                modifiers.Knockback *= slimeDual.GetModPlayer<DualSlime>().bonusKb;
            }
        }
    }
    public sealed class PrinceSlimeandDualSlimeEnemyProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool fromNpcAndWet = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is NPC npc && npc.HasBuff(BuffID.Wet))
            {
                fromNpcAndWet = true;
            }
            else
            {
                fromNpcAndWet = false;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (fromNpcAndWet == true)
            {
                if (GlobalPet.KingSlimePetActive(out Player prince))
                {
                    modifiers.FinalDamage *= prince.GetModPlayer<SlimePrince>().wetDealtLower;
                }
                else if (GlobalPet.DualSlimePetActive(out Player dual))
                {
                    modifiers.FinalDamage *= dual.GetModPlayer<DualSlime>().wetDealtLower;
                }
            }
        }
    }
    public sealed class KingSlimePetItem : PetTooltip
    {
        public override PetEffect PetsEffect => slimePrince;
        public static SlimePrince slimePrince
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SlimePrince pet))
                    return pet;
                else
                    return ModContent.GetInstance<SlimePrince>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.KingSlimePetItem")
                        .Replace("<burnHp>", Math.Round(slimePrince.healthDmg * 100, 2).ToString())
                        .Replace("<burnCap>", slimePrince.burnCap.ToString())
                        .Replace("<extraKb>", slimePrince.bonusKb.ToString())
                        .Replace("<jumpSpd>", Math.Round(slimePrince.slimyJump * 100, 2).ToString())
                        .Replace("<kbBoost>", slimePrince.slimyKb.ToString())
                        .Replace("<enemyDmgRecieve>", slimePrince.wetRecievedHigher.ToString())
                        .Replace("<enemyDmgDeal>", slimePrince.wetDealtLower.ToString())
                        .Replace("<dmg>", Math.Round(slimePrince.wetDmg * 100, 2).ToString())
                        .Replace("<def>", Math.Round(slimePrince.wetDef * 100, 2).ToString())
                        .Replace("<moveSpd>", Math.Round(slimePrince.wetSpeed * 100, 2).ToString());
    }
}
