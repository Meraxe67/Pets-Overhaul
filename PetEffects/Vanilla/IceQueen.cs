using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
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
    public sealed class IceQueen : ModPlayer
    {
        public int cooldown = 10800;
        private bool frozenTomb = false;
        private int iceQueenFrame = 0;
        public int queenRange = 480;
        public float slowAmount = 10f;
        public int freezeDamage = 200;
        public int immuneTime = 150;
        public int tombTime = 300;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.IceQueenPetItem))
            {
                Pet.timerMax = cooldown;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.IceQueenPetItem))
            {
                if (frozenTomb == true)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && Player.Distance(npc.Center) < queenRange)
                        {
                            npc.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.IceQueen, slowAmount, 2);
                        }
                    }
                    for (int i = 0; i < 20 + iceQueenFrame / 15; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Player.position + Main.rand.NextVector2Circular(queenRange, queenRange), DustID.SnowflakeIce, Vector2.Zero, newColor: Color.Aquamarine);
                        dust.noGravity = true;
                    }
                    if (iceQueenFrame % 30 == 0 && ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        if (Main.rand.NextBool())
                        {
                            SoundEngine.PlaySound(SoundID.Item48 with { PitchVariance = 0.3f, Volume = 0.8f }, Player.position + Main.rand.NextVector2Circular(queenRange, queenRange));
                        }
                        else
                        {
                            SoundEngine.PlaySound(SoundID.Item49 with { PitchVariance = 0.3f, Volume = 0.8f }, Player.position + Main.rand.NextVector2Circular(queenRange, queenRange));
                        }
                    }
                    iceQueenFrame++;
                    Player.buffImmune[BuffID.Frozen] = false;
                    Player.AddBuff(BuffID.Frozen, 1);
                    Player.SetImmuneTimeForAllTypes(1);
                    if (iceQueenFrame % 3 == 0)
                    {
                        Player.statLife++;
                    }
                }
                if (iceQueenFrame >= tombTime && frozenTomb == true)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && Player.Distance(npc.Center) < queenRange && Pet.LifestealCheck(npc))
                        {
                            int crit = (int)Player.GetTotalCritChance<GenericDamageClass>();
                            if (crit > 100)
                            {
                                crit = 100;
                            }

                            npc.SimpleStrikeNPC(freezeDamage, 1, Main.rand.NextBool(crit, 100), 0, DamageClass.Generic);
                        }
                    }
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.Shatter with { PitchVariance = 0.2f }, Player.position);
                    }
                        Player.HealEffect(100);
                    Player.immune = false;
                    Player.SetImmuneTimeForAllTypes(immuneTime);
                    frozenTomb = false;
                    iceQueenFrame = 0;
                }
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            if (Pet.PetInUseWithSwapCd(ItemID.IceQueenPetItem) && Pet.timer <= 0)
            {

                if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                {
                    SoundEngine.PlaySound(SoundID.Item30 with { PitchVariance = 0.5f, MaxInstances = 5, Pitch = -0.5f }, Player.position);
                }

                frozenTomb = true;
                Player.statLife = 1;
                Pet.timer = Pet.timerMax;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public sealed class IceQueenPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.IceQueenPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            IceQueen iceQueen = Main.LocalPlayer.GetModPlayer<IceQueen>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.IceQueenPetItem")
                        .Replace("<frozenTombTime>", Math.Round(iceQueen.tombTime / 60f, 2).ToString())
                        .Replace("<range>", Math.Round(iceQueen.queenRange / 16f, 2).ToString())
                        .Replace("<slowAmount>", Math.Round(iceQueen.slowAmount * 100, 2).ToString())
                        .Replace("<healthRecovery>", (iceQueen.tombTime / 3).ToString())
                        .Replace("<baseDmg>", iceQueen.freezeDamage.ToString())
                        .Replace("<postTombImmunity>", Math.Round(iceQueen.immuneTime / 60f, 2).ToString())
                        .Replace("<tombCooldown>", Math.Round(iceQueen.cooldown / 3600f, 2).ToString())
                        ));
        }
    }
}
