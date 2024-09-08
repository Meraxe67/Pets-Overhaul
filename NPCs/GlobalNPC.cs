using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.NPCs
{
    public sealed class NpcPet : GlobalNPC
    {
        int test = 0;
        /// <summary>
        /// Slow thats applied to an NPC.
        /// </summary>
        /// <param name="slowAmount">% of slow to be applied to the NPC. Negative values will speed the enemy up, which cannot go below -0.9f.</param>
        /// <param name="slowTime">Time for slow to be applied in frames.</param>
        /// <param name="slowId">Slows with the ID of -1 (or lower) are independent. If another slow with ID higher than 0 meets itself, it will replace the 'worse slow' of the same ID. Same slow ID cannot exist more than once.</param>
        public struct PetSlow(float slowAmount, int slowTime, int slowId = PetSlowIDs.IndependentSlow)
        {
            public float SlowAmount = slowAmount;
            public int SlowTime = slowTime;
            public int SlowId = slowId;
        }
        public List<PetSlow> SlowList = new();
        /// <summary>
        /// If you need to find out how much current cumulative slow amount is, use this.
        /// </summary>
        public float CurrentSlowAmount { get; internal set; }
        public bool seaCreature;
        public int playerThatFishedUp;
        public int maulCounter;
        public int curseCounter;
        /// <summary>
        /// Contains all Vanilla bosses that does not return npc.boss = true
        /// </summary>
        public static bool[] nonBossTrueBosses = NPCID.Sets.Factory.CreateBoolSet(false, NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex, NPCID.TorchGod, NPCID.Retinazer, NPCID.Spazmatism);
        public Vector2 FlyingVelo { get; internal set; }
        public float GroundVelo { get; internal set; }
        public bool VeloChangedFlying { get; internal set; }
        public bool VeloChangedFlying2 { get; internal set; }
        public bool VeloChangedGround { get; internal set; }
        public bool VeloChangedGround2 { get; internal set; }

        public override bool InstancePerEntity => true;
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.EyeofCthulhu && MasteryShardCheck.masteryShardObtained1 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillEoC(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.WallofFlesh && MasteryShardCheck.masteryShardObtained2 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillWoF(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.Golem && MasteryShardCheck.masteryShardObtained3 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillGolem(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.SkeletronHead && MasteryShardCheck.masteryShardObtained4 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillSkeletron(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                if (MasteryShardCheck.masteryShardObtained5 == false)
                {
                    npcLoot.Add(ItemDropRule.ByCondition(new FirstKillMoonLord(), ModContent.ItemType<MasteryShard>()));
                }

                if (Main.expertMode == false && Main.masterMode == false)
                {
                    npcLoot.Add(ItemDropRule.Common(ItemID.SuspiciousLookingTentacle));
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                MasteryShardCheck.masteryShardObtained1 = true;
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                MasteryShardCheck.masteryShardObtained2 = true;
            }
            if (npc.type == NPCID.Golem)
            {
                MasteryShardCheck.masteryShardObtained3 = true;
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                MasteryShardCheck.masteryShardObtained4 = true;
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                MasteryShardCheck.masteryShardObtained5 = true;
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_FishedOut fisherman && fisherman.Fisher is Player player)
            {
                playerThatFishedUp = player.whoAmI;
                seaCreature = true;
            }
            else if (npc.type == NPCID.DukeFishron && source is EntitySource_BossSpawn fisher && fisher.Target is Player player2) //improve for bosses
            {
                playerThatFishedUp = player2.whoAmI;
                seaCreature = true;
            }
            else
            {
                seaCreature = false;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.active)
            {
                if (VeloChangedGround == true)
                {
                    npc.velocity.X = GroundVelo;
                    VeloChangedGround2 = true;
                }
                else
                {
                    VeloChangedGround2 = false;
                }

                if (VeloChangedGround2 == false)
                {
                    GroundVelo = npc.velocity.X;
                }

                if (VeloChangedFlying == true)
                {
                    npc.velocity = FlyingVelo;
                    VeloChangedFlying2 = true;
                }
                else
                {
                    VeloChangedFlying2 = false;
                }

                if (VeloChangedFlying2 == false)
                {
                    FlyingVelo = npc.velocity;
                }
            }
            return base.PreAI(npc);
        }
        public override void PostAI(NPC npc)
        {
            if (npc.active)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    CurrentSlowAmount = 0;

                    if (SlowList.Count > 0)
                    {
                        SlowList.ForEach(x => CurrentSlowAmount += x.SlowAmount);

                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                            packet.Write((byte)MessageType.PetSlow);
                            packet.Write((byte)npc.whoAmI);
                            packet.Write(CurrentSlowAmount);
                            packet.Send();
                        }

                        for (int i = 0; i < SlowList.Count; i++) //Since Structs in Lists acts as Readonly, we re-assign the values to the index to decrement the timer.
                        {
                            PetSlow slow = SlowList[i];
                            slow.SlowTime--;
                            SlowList[i] = slow;
                        }
                        int indexToRemove = SlowList.FindIndex(x => x.SlowTime <= 0);
                        if (indexToRemove != -1)
                        {
                            SlowList.RemoveAt(indexToRemove);
                        }
                    }
                }
                if (CurrentSlowAmount > 0)
                {
                    Slow(npc, CurrentSlowAmount);
                }
            }
        }
        /// <summary>
        /// Does not slow an npc's vertical speed if they are affected by gravity, but does so if they arent. Due to the formula, you may use a positive number for slowAmount freely and as much as you want, it almost will never completely stop an enemy. Negative values however, easily can get out of hand and cause unwanted effects. Due to that, a cap of -0.9f exists for negative values, which 10x's the speed.
        /// </summary>
        private void Slow(NPC npc, float slow)
        {
            if (slow < -0.9f)
            {
                slow = -0.9f;
            }

            FlyingVelo = npc.velocity;
            GroundVelo = npc.velocity.X;
            if (npc.noGravity == false)
            {
                npc.velocity.X *= 1 / (1 + slow);
                VeloChangedGround = true;
            }
            else
            {
                VeloChangedGround = false;
            }

            if (npc.noGravity)
            {
                npc.velocity *= 1 / (1 + slow);
                VeloChangedFlying = true;
            }   
            else
            {
                VeloChangedFlying = false;
            }
        }
        /// <summary>
        /// Use this to add Slow to an NPC. Also checks if the NPC is a boss or a friendly npc or not.
        /// </summary>
        static public void AddSlow(PetSlow petSlow, NPC npc)
        {
            if (npc.active && (npc.townNPC == false || npc.isLikeATownNPC == false || npc.friendly == false) && npc.boss == false && nonBossTrueBosses[npc.type] == false && npc.TryGetGlobalNPC<NpcPet>(out NpcPet npcPet))
            {
                if (petSlow.SlowId <= -1)
                {
                    npcPet.SlowList.Add(petSlow);
                    return;
                }
                int indexToReplace;
                if (npcPet.SlowList.Exists(x => x.SlowId == petSlow.SlowId && x.SlowAmount < petSlow.SlowAmount))
                {
                    indexToReplace = npcPet.SlowList.FindIndex(x => x.SlowId == petSlow.SlowId && x.SlowAmount < petSlow.SlowAmount);
                    npcPet.SlowList[indexToReplace] = petSlow;
                }
                else if (npcPet.SlowList.Exists(x => x.SlowId == petSlow.SlowId && x.SlowAmount == petSlow.SlowAmount && x.SlowTime < petSlow.SlowTime))
                {
                    indexToReplace = npcPet.SlowList.FindIndex(x => x.SlowId == petSlow.SlowId && x.SlowAmount == petSlow.SlowAmount && x.SlowTime < petSlow.SlowTime);
                    npcPet.SlowList[indexToReplace] = petSlow;
                }
                else if (npcPet.SlowList.Exists(x => x.SlowId == petSlow.SlowId) == false)
                {
                    npcPet.SlowList.Add(petSlow);
                }
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<Mauled>()))
            {
                for (int i = 0; i < maulCounter; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 3f), 75, default, Main.rand.NextFloat(0.5f, 0.8f));
                    dust.velocity *= 0.8f;
                }
            }
            if (npc.HasBuff(ModContent.BuffType<QueensDamnation>()))
            {
                if (curseCounter >= 20)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Wraith, Main.rand.NextFloat(0f, 0.6f), Main.rand.NextFloat(-1f, 2f), 75, default, Main.rand.NextFloat(0.7f, 1f));
                    dust.velocity *= 0.2f;
                }
                else
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Pixie, Main.rand.NextFloat(0f, 0.6f), Main.rand.NextFloat(-1f, 2f), 150, default, Main.rand.NextFloat(0.5f, 0.7f));
                    dust.velocity *= 0.2f;
                }
            }
        }
    }
    /// <summary>
    /// Class that contains PetSlowID's, where same slow ID does not overlap with itself, and a slow with greater slow & better remaining time will override the obsolete one.
    /// </summary>
    public class PetSlowIDs //
    {
        /// <summary>
        /// Slows with ID lower than 0 won't be overriden by itself by any means and can have multiples of the same ID this way.
        /// </summary>
        public const int IndependentSlow = -1;
        public const int Grinch = 0;
        public const int Snowman = 1;
        public const int QueenSlime = 2;
        public const int Deerclops = 3;
        public const int IceQueen = 4;
        public const int VoltBunny = 5;
        public const int PhantasmalIce = 6;
    }
}
