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

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class BabyFaceMonster : ModPlayer
    {
        private int timer = 0;
        public int stage2time = 1200;
        public int stage1time = 900;
        public int stage1regen = 3;
        public int stage2regen = 15;
        public float stage2ShieldMult = 0.05f;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BoneRattle))
            {
                timer--;
                if (timer < -1000)
                {
                    timer = -1000;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BoneRattle))
            {
                if (timer == 0)
                {
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie21 with { Pitch = -0.7f, PitchVariance = 0.3f, Volume = 0.75f }, Player.position);
                    }
                }
                if (timer <= 0)
                {
                    Pet.petShield.Add(((int)(Player.statLifeMax2 * stage2ShieldMult), 1));
                    Player.lifeRegen += stage2regen;
                    Player.crimsonRegen = true;
                }
                if (timer == (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
                {
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(new SoundStyle(SoundID.DD2_DrakinShot.SoundPath + "0") with { Pitch = -0.7f, PitchVariance = 0.3f, Volume = 0.75f }, Player.position);
                    }
                }
                if (timer <= (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
                {
                    Player.lifeRegen += stage1regen;
                    Player.crimsonRegen = true;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUse(ItemID.BoneRattle))
            {
                timer = (int)(stage1time * (1 / (1 + Pet.abilityHaste))) - 1;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUse(ItemID.BoneRattle))
            {
                timer = (int)(stage2time * (1 / (1 + Pet.abilityHaste)));
            }
        }
    }
    public sealed class BoneRattle : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BoneRattle;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            BabyFaceMonster babyFaceMonster = Main.LocalPlayer.GetModPlayer<BabyFaceMonster>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BoneRattle")
                .Replace("<stage1Time>", Math.Round((babyFaceMonster.stage2time - babyFaceMonster.stage1time) / 60f, 5).ToString())
                .Replace("<stage2Time>", Math.Round(babyFaceMonster.stage2time / 60f, 5).ToString())
                .Replace("<stage1Regen>", babyFaceMonster.stage1regen.ToString())
                .Replace("<stage2Regen>", babyFaceMonster.stage2regen.ToString())
                .Replace("<shieldAmount>", Math.Round(babyFaceMonster.stage2ShieldMult * 100, 5).ToString())
            ));
        }
    }
}
