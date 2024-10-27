using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.Player;

namespace PetsOverhaul.PetEffects
{
    public sealed class SkeletronJr : PetEffect
    {
        public List<(int, int)> skeletronTakenDamage = new();
        private int timer = 0;
        public float enemyDamageIncrease = 1.2f;
        public float playerDamageTakenSpeed = 4f;
        public float playerTakenMult = 1.02f;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void PreUpdate()
        {
            timer++;
            if (timer > 10000)
            {
                timer = 10000;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (skeletronTakenDamage.Count > 0 && timer >= 60)
            {
                int totalDmg = 0;
                skeletronTakenDamage.ForEach(x => totalDmg += (int)Math.Ceiling(x.Item2 / playerDamageTakenSpeed));
                Player.statLife -= totalDmg;
                CombatText.NewText(Player.getRect(), CombatText.DamagedHostile, totalDmg);
                if (Player.statLife <= 0)
                {
                    string reason;
                    switch (Main.rand.Next(20))
                    {
                        case 0:
                            reason = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronDeath3");
                            break;
                        case 1:
                            reason = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronDeath4");
                            break;
                        default:
                            if (Main.rand.NextBool())
                            {
                                reason = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronDeath1");
                            }
                            else
                            {
                                reason = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronDeath2");
                            }
                            reason = reason.Replace("<name>", Player.name);
                            break;
                    }
                    Player.KillMe(PlayerDeathReason.ByCustomReason(reason), 1, 0);
                }

                for (int i = 0; i < skeletronTakenDamage.Count; i++)
                {
                    (int, int) point = skeletronTakenDamage[i];
                    point.Item1 -= (int)Math.Ceiling(point.Item2 / playerDamageTakenSpeed);
                    skeletronTakenDamage[i] = point;
                }
                skeletronTakenDamage.RemoveAll(x => x.Item1 <= 0);

                timer = 0;
            }
        }
        public override bool ConsumableDodge(HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                PlayerLoader.OnHurt(Player, info);
                PlayerLoader.PostHurt(Player, info);
                skeletronTakenDamage.Add((info.Damage, info.Damage));
                if (info.Damage <= 1)
                {
                    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 40 : 20);
                }
                else
                {
                    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 80 : 40);
                }
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void UpdateDead()
        {
            skeletronTakenDamage.Clear();
        }
    }
    public sealed class SkeletronJrEnemy : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<(int, int)> skeletronDealtDamage = new();
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (skeletronDealtDamage.Count > 0)
            {
                int totalDmg = 0;
                skeletronDealtDamage.ForEach(x => totalDmg += x.Item1);
                npc.lifeRegen -= totalDmg / 2;
                damage = totalDmg / 5;
            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                npc.life += damageDone;
                skeletronDealtDamage.Add(((int)(damageDone * player.GetModPlayer<SkeletronJr>().enemyDamageIncrease), 240));
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.SkeletronPetItem))
            {
                npc.life += damageDone;
                skeletronDealtDamage.Add(((int)(damageDone * Main.player[projectile.owner].GetModPlayer<SkeletronJr>().enemyDamageIncrease), 240));
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (skeletronDealtDamage.Count > 0)
            {
                for (int i = 0; i < skeletronDealtDamage.Count; i++) //List'lerde struct'lar bir nevi readonly olarak çalıştığından, değeri alıp tekrar atıyoruz
                {
                    (int, int) point = skeletronDealtDamage[i];
                    point.Item2--;
                    skeletronDealtDamage[i] = point;
                }
                skeletronDealtDamage.RemoveAll(x => x.Item2 <= 0);
            }

            return base.PreAI(npc);
        }
        public override void OnKill(NPC npc)
        {
            skeletronDealtDamage.Clear();
        }
    }
    public sealed class SkeletronPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SkeletronPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            SkeletronJr skeletronJr = Main.LocalPlayer.GetModPlayer<SkeletronJr>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronPetItem")
                .Replace("<class>", PetTextsColors.ClassText(skeletronJr.PetClassPrimary, skeletronJr.PetClassSecondary))
                        .Replace("<recievedMult>", skeletronJr.playerTakenMult.ToString())
                        .Replace("<recievedHowLong>", skeletronJr.playerDamageTakenSpeed.ToString())
                        .Replace("<dealtMult>", skeletronJr.enemyDamageIncrease.ToString())
                        ));
        }
    }
}
