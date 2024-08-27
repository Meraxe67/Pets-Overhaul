using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace PetsOverhaul.PetEffects
{
    public sealed class Turtle : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public float moveSpd = 0.12f;
        public float def = 0.12f;
        public float kbResist = 0.9f;
        public float dmgReduce = 0.04f;
        public int flatDef = 1;
        private int timer = 0;
        public int shellUpTime = 480;
        public float shellUpMult = 5f;
        public int shellUpCd = 1800;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.Seaweed))
            {
                Pet.SetPetAbilityTimer(shellUpCd);
                timer--;
                if (timer <= 0)
                    timer = 0;
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                if (timer > 0)
                {
                    Player.statDefense += (int)Math.Ceiling(flatDef*shellUpMult);
                    Player.statDefense *= def*shellUpMult + 1f;
                    Player.moveSpeed -= moveSpd * shellUpMult;
                    Player.GetDamage<GenericDamageClass>() -= dmgReduce*shellUpMult;
                    Player.noFallDmg = true;
                    Player.frozen = true;
                    return;
                }

                Player.statDefense += flatDef;
                Player.statDefense *= def+1f;
                Player.moveSpeed -= moveSpd;
                Player.GetDamage<GenericDamageClass>() -= dmgReduce;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seaweed))
            {
                if (timer > 0)
                {
                    modifiers.Knockback *= 0;
                    return;
                }
                modifiers.Knockback *= 1f-kbResist;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.timer <= 0 && Pet.PetInUseWithSwapCd(ItemID.Seaweed) && Keybinds.UsePetAbility.JustPressed)
            {
                timer = shellUpTime;
                Pet.timer = Pet.timerMax;
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
                        .Replace("<def>", turtle.def.ToString())
                        .Replace("<kbResist>", Math.Round(turtle.kbResist*100,2).ToString())
                        .Replace("<flat>", turtle.flatDef.ToString())
                        .Replace("<moveSpd>", Math.Round(turtle.moveSpd * 100, 2).ToString())
                        .Replace("<dmg>", Math.Round(turtle.dmgReduce * 100, 2).ToString())
                        ));
        }
    }
}
