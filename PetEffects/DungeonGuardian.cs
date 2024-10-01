using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class DungeonGuardian : PetEffect
    {
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public int armorPen = 10;
        public int lifeRegen = 8;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BoneKey))
            {
                Player.npcTypeNoAggro[NPCID.AngryBones] = true;
                Player.npcTypeNoAggro[NPCID.AngryBonesBig] = true;
                Player.npcTypeNoAggro[NPCID.AngryBonesBigHelmet] = true;
                Player.npcTypeNoAggro[NPCID.AngryBonesBigMuscle] = true;
                Player.npcTypeNoAggro[NPCID.FromNetId(NPCID.ShortBones)] = true;
                Player.npcTypeNoAggro[NPCID.FromNetId(NPCID.BigBoned)] = true;
                Player.npcTypeNoAggro[NPCID.DarkCaster] = true;
                Player.npcTypeNoAggro[NPCID.DungeonSlime] = true;
                Player.npcTypeNoAggro[NPCID.CursedSkull] = true;
                Player.npcTypeNoAggro[NPCID.BlazingWheel] = true;
                Player.npcTypeNoAggro[NPCID.SpikeBall] = true;
                Player.npcTypeNoAggro[NPCID.WaterSphere] = true;
                if (Player.ZoneDungeon == true)
                {
                    Player.lifeRegen += lifeRegen;
                }
                Player.GetArmorPenetration<GenericDamageClass>() += armorPen;
            }
        }
        public override void Load()
        {
            On_Collision.CanTileHurt += PreventSpikesHurt;
        }
        private static bool PreventSpikesHurt(On_Collision.orig_CanTileHurt orig, ushort type, int i, int j, Player player)
        {
            bool hurt = orig(type, i, j, player);
            if (type == TileID.Spikes && player.ZoneDungeon && player.miscEquips[0].type == ItemID.BoneKey && player.HasBuff(ModContent.BuffType<ObliviousPet>()) == false)
            {
                hurt = false;
            }
            return hurt;
        }
    }
    public sealed class BoneKey : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BoneKey;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            DungeonGuardian dungeonGuardian = Main.LocalPlayer.GetModPlayer<DungeonGuardian>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BoneKey")
                .Replace("<class>", PetTextsColors.ClassText(dungeonGuardian.PetClassPrimary, dungeonGuardian.PetClassSecondary))
                        .Replace("<armorPen>", dungeonGuardian.armorPen.ToString())
                        .Replace("<dungRegen>", dungeonGuardian.lifeRegen.ToString())
                        ));
        }
    }
}
