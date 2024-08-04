using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Plantero : PetEffect
    {
        public int spawnChance = 15;
        public float damageMult = 0.7f;
        public float knockBack = 0.4f;
        public int flatDmg = 15;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj == false && Pet.PetInUseWithSwapCd(ItemID.MudBud))
            {
                for (int i = 0; i < ItemPet.Randomizer(spawnChance + (int)(spawnChance * Pet.abilityHaste)); i++)
                {
                    Vector2 location = new(target.position.X + Main.rand.NextFloat(-2f, 2f), target.position.Y + Main.rand.NextFloat(-2f, 2f));
                    Vector2 velocity = new(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f));
                    int projId = ProjectileID.SporeGas;
                    switch (Main.rand.Next(3))
                    {
                        case 1:
                            projId = ProjectileID.SporeGas2;
                            break;
                        case 2:
                            projId = ProjectileID.SporeGas3;
                            break;
                    }
                    Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), location, velocity, projId, (int)(damageDone * damageMult) + flatDmg, knockBack, Main.myPlayer);
                }
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MudBud))
            {
                for (int i = 0; i < ItemPet.Randomizer(spawnChance + (int)(spawnChance * Pet.abilityHaste)); i++)
                {
                    Vector2 location = new(target.position.X + Main.rand.NextFloat(-2f, 2f), target.position.Y + Main.rand.NextFloat(-2f, 2f));
                    Vector2 velocity = new(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f));
                    int projId = ProjectileID.SporeGas;
                    switch (Main.rand.Next(3))
                    {
                        case 1:
                            projId = ProjectileID.SporeGas2;
                            break;
                        case 2:
                            projId = ProjectileID.SporeGas3;
                            break;
                    }
                    Projectile.NewProjectile(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), location, velocity, projId, (int)(damageDone * damageMult) + flatDmg, knockBack, Main.myPlayer);
                }
            }
        }
    }
    public sealed class MudBud : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MudBud;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Plantero plantero = Main.LocalPlayer.GetModPlayer<Plantero>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MudBud")
                .Replace("<class>", PetColors.ClassText(plantero.PetClassPrimary, plantero.PetClassSecondary))
                        .Replace("<chance>", plantero.spawnChance.ToString())
                        .Replace("<dmg>", plantero.damageMult.ToString())
                        .Replace("<flatDmg>", plantero.flatDmg.ToString())
                        .Replace("<kb>", plantero.knockBack.ToString())
                        ));
        }
    }
}
