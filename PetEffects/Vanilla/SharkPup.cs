using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class SharkPup : ModPlayer
    {
        public float seaCreatureResist = 0.85f;
        public float seaCreatureDamage = 1.1f;
        public int shieldOnCatch = 10;
        public int shieldTime = 900;
        public int fishingPow = 10;

        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SharkBait) && npc.GetGlobalNPC<NpcPet>().seaCreature)
            {
                modifiers.FinalDamage *= seaCreatureResist;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SharkBait) && target.GetGlobalNPC<NpcPet>().seaCreature)
            {
                modifiers.FinalDamage *= seaCreatureDamage;
            }
        }
        public override void UpdateEquips()
        {
            if (Pet.PetInUse(ItemID.SharkBait))
            {
                Player.fishingSkill += fishingPow;
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SharkBait))
            {
                Pet.petShield.Add((shieldOnCatch, shieldTime));
            }
        }
    }
    public sealed class SeaCreatureProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool seaCreatureProj = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is NPC npc && npc.GetGlobalNPC<NpcPet>().seaCreature)
            {
                seaCreatureProj = true;
            }
            else
            {
                seaCreatureProj = false;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (seaCreatureProj == true)
            {
                SharkPup player = target.GetModPlayer<SharkPup>();
                if (player.Pet.PetInUseWithSwapCd(ItemID.SharkBait))
                {
                    modifiers.FinalDamage *= player.seaCreatureResist;
                }
            }
        }
    }
    public sealed class SharkBait : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SharkBait;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            SharkPup sharkPup = Main.LocalPlayer.GetModPlayer<SharkPup>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SharkBait")
                        .Replace("<fishingPower>", sharkPup.fishingPow.ToString())
                        .Replace("<seaCreatureDmg>", sharkPup.seaCreatureDamage.ToString())
                        .Replace("<seaCreatureResist>", sharkPup.seaCreatureResist.ToString())
                        .Replace("<shield>", sharkPup.shieldOnCatch.ToString())
                        .Replace("<shieldTime>", Math.Round(sharkPup.shieldTime / 60f, 2).ToString())
                        ));
        }
    }
}
