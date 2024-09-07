using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Turtle : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public float moveSpd = 0.12f;
        public float def = 0.11f;
        public float kbResist = 0.55f;

        private int timer = 0;
        internal int currentStacks = 0;
        public int shellHardenDuration = 900;
        public int shellHardenStacks = 6;
        public int shellHardenCd = 2100;
        public float dmgReduceShellHarden = 0.07f;
        public float dmgReflect = 1.1f;
        public float dmgReflectProjectile = 0.6f;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.Seaweed))
            {
                Pet.SetPetAbilityTimer(shellHardenCd);
                timer--;
                if (timer <= 0)
                {
                    currentStacks = 0;
                    timer = 0;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                Player.statDefense *= def + 1f;
                Player.moveSpeed -= moveSpd;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                if (currentStacks > 0)
                {
                    modifiers.Knockback *= 0;
                    modifiers.FinalDamage *= 1f - dmgReduceShellHarden;
                    return;
                }
                modifiers.Knockback *= 1f - kbResist;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed) && currentStacks > 0)
            {
                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    NPC.HitInfo hit = new NPC.HitInfo() with { Crit = false, DamageType = DamageClass.Generic, HitDirection = info.HitDirection, Knockback = 1f };
                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile<ProjectileSourceChecks>(out ProjectileSourceChecks proj))
                    {
                        NPC npc = Main.npc[proj.sourceNpcId];
                        npc.StrikeNPC(hit with { Damage = (int)(info.SourceDamage * dmgReflectProjectile) });
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendStrikeNPC(npc, hit);
                    }
                    else if (entity is NPC npc && npc.active == true && npc.immortal == false)
                    {
                        npc.StrikeNPC(hit with { Damage = (int)(info.SourceDamage * dmgReflect) });
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendStrikeNPC(npc, hit);
                    }
                }
                currentStacks--;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.timer <= 0 && Pet.PetInUseWithSwapCd(ItemID.Seaweed) && Keybinds.UsePetAbility.JustPressed)
            {
                timer = shellHardenDuration;
                currentStacks = shellHardenStacks;
                Pet.timer = Pet.timerMax;
                Player.AddBuff(ModContent.BuffType<HardenedShell>(), shellHardenDuration);
            }
        }
    }
    public sealed class Seaweed : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Seaweed;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Turtle turtle = Main.LocalPlayer.GetModPlayer<Turtle>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Seaweed")
                .Replace("<class>", PetTextsColors.ClassText(turtle.PetClassPrimary, turtle.PetClassSecondary))
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                .Replace("<hitCount>", turtle.shellHardenStacks.ToString())
                .Replace("<shellDuration>", Math.Round(turtle.shellHardenDuration / 60f, 2).ToString())
                .Replace("<reducedDmg>", Math.Round(turtle.dmgReduceShellHarden * 100, 2).ToString())
                .Replace("<reflect>", Math.Round(turtle.dmgReflect * 100, 2).ToString())
                .Replace("<projReflect>", Math.Round(turtle.dmgReflectProjectile * 100, 2).ToString())
                        .Replace("<def>", Math.Round(turtle.def * 100, 2).ToString())
                        .Replace("<kbResist>", Math.Round(turtle.kbResist * 100, 2).ToString())
                        .Replace("<moveSpd>", Math.Round(turtle.moveSpd * 100, 2).ToString())
                        ));
        }
    }
}
