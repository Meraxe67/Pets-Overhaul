using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
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
        public int pen = 20;

        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public void SpawnGasCloud(NPC target, int damage, DamageClass dmgType)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MudBud))
            {
                for (int i = 0; i < GlobalPet.Randomizer(spawnChance + (int)(spawnChance * Pet.abilityHaste)); i++)
                {
                    Vector2 location = new(target.Center.X + Main.rand.NextFloat(-2f, 2f), target.Center.Y + Main.rand.NextFloat(-2f, 2f));
                    Vector2 velocity = new(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f));
                    short projId = Main.rand.Next(3) switch
                    {
                        0 => ProjectileID.SporeGas,
                        1 => ProjectileID.SporeGas2,
                        2 => ProjectileID.SporeGas3,
                        _ => ProjectileID.SporeGas,
                    };
                    ;
                    Projectile gas = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), location, velocity, projId, Pet.PetDamage(damage * damageMult + flatDmg), knockBack, Main.myPlayer);
                    gas.Resize(gas.width * 2, gas.height * 2);
                    gas.scale *= 2;
                    gas.penetrate = pen;
                    gas.DamageType = dmgType;
                    gas.CritChance = (int)Player.GetTotalCritChance(dmgType);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.GetGlobalProjectile<ProjectileSourceChecks>().petProj == false)
            {
                SpawnGasCloud(target, hit.SourceDamage, hit.DamageType);
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpawnGasCloud(target, hit.SourceDamage, hit.DamageType);
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
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            Plantero plantero = Main.LocalPlayer.GetModPlayer<Plantero>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MudBud")
                .Replace("<class>", PetTextsColors.ClassText(plantero.PetClassPrimary, plantero.PetClassSecondary))
                        .Replace("<chance>", plantero.spawnChance.ToString())
                        .Replace("<dmg>", plantero.damageMult.ToString())
                        .Replace("<flatDmg>", plantero.flatDmg.ToString())
                        .Replace("<kb>", plantero.knockBack.ToString())
                        .Replace("<pen>", plantero.pen.ToString())
                        ));
        }
    }
}
