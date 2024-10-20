using PetsOverhaul.Config;
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
    public sealed class BabyFaceMonster : PetEffect
    {
        private int timer = 0;
        public int stage2time = 720;
        public int stage1time = 420;
        public int stage1regen = 3;
        public int stage2regen = 9;
        public float stage2ShieldMult = 0.05f;

        public override PetClasses PetClassPrimary => PetClasses.Summoner;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BoneRattle))
            {
                stage1regen = 3;
                stage2regen = 9;
                timer--;
                if (timer < -1000)
                {
                    timer = -1000;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BoneRattle))
            {
                stage1regen = (int)Math.Round(stage1regen * Pet.petHealMultiplier);
                stage2regen = (int)Math.Round(stage2regen * Pet.petHealMultiplier);
                if (timer == 0)
                {
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie21 with { Pitch = -0.7f, PitchVariance = 0.3f, Volume = 0.75f }, Player.Center);
                    }
                }
                if (timer == (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
                {
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(new SoundStyle(SoundID.DD2_DrakinShot.SoundPath + "0") with { Pitch = -0.7f, PitchVariance = 0.3f, Volume = 0.75f }, Player.Center);
                    }
                }
                if (timer <= 0)
                {
                    Pet.AddShield((int)(Player.statLifeMax2 * stage2ShieldMult), 1);
                    Player.lifeRegen += stage2regen;
                    Player.crimsonRegen = true;
                }
                else if (timer <= (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
                {
                    Player.lifeRegen += stage1regen;
                    Player.crimsonRegen = true;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUse(ItemID.BoneRattle) && timer < (int)(stage1time * (1 / (1 + Pet.abilityHaste))))
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
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyFaceMonster babyFaceMonster = Main.LocalPlayer.GetModPlayer<BabyFaceMonster>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BoneRattle")
                .Replace("<class>", PetTextsColors.ClassText(babyFaceMonster.PetClassPrimary, babyFaceMonster.PetClassSecondary))
                .Replace("<stage1Time>", Math.Round((babyFaceMonster.stage2time - babyFaceMonster.stage1time) / 60f, 2).ToString())
                .Replace("<stage2Time>", Math.Round(babyFaceMonster.stage2time / 60f, 2).ToString())
                .Replace("<stage1Regen>", babyFaceMonster.stage1regen.ToString())
                .Replace("<stage2Regen>", babyFaceMonster.stage2regen.ToString())
                .Replace("<shieldAmount>", Math.Round(babyFaceMonster.stage2ShieldMult * 100, 2).ToString())
            ));
        }
    }
}
