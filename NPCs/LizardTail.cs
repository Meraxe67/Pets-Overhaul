using Microsoft.Xna.Framework;
using MonoMod.Cil;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace PetsOverhaul.NPCs
{
    public class LizardTail : ModNPC
    {
        public int waitTime = 0;
        public int lifespan = 0;
        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 50;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.friendly = true;
            NPC.dontTakeDamageFromHostiles = false;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = -1;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lihzahrd, 2 * hit.HitDirection, -2f);
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale = 1.2f * NPC.scale;
                    }
                    else
                    {
                        dust.scale = 0.7f * NPC.scale;
                    }
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(2f, 2f), 259, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(2f, 2f), 259, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(2f, 2f), 261, NPC.scale);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Pet)
            {
                NPC.lifeMax = (int)NPC.ai[0];
                waitTime = (int)NPC.ai[1];
                lifespan = (int)NPC.ai[2];
                NPC.life = NPC.lifeMax;
            }
        }
        void Kill()
        {
            NPC.life = 0;
            NPC.HitEffect();
            NPC.active = false;
            SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.position);
        }
        public override Color? GetAlpha(Color drawColor)
        {
            int alpha = lifespan / 6 + 1;
            if (alpha > 255)
                alpha = 255;
            return drawColor with { A = (byte)alpha };
        }

        public override void AI()
        {
            waitTime--;
            lifespan--;
            if (lifespan <= 0)
            {
                Kill();
            }
            if (waitTime <= 0)
            {
                Lighting.AddLight(NPC.position, Color.GreenYellow.ToVector3() * (lifespan / 400f) * Main.mouseTextColor * 0.0255f);
                Player player = Main.player[NPC.FindClosestPlayer()];
                if (NPC.Distance(player.position) < 50)
                {
                    Lizard lizard = player.GetModPlayer<Lizard>();
                    lizard.Pet.PetRecovery(player.statLifeMax2, lizard.percentHpRecover, isLifesteal: false);
                    lizard.Pet.timer = (int)(lizard.Pet.timer * lizard.tailCdRefund);
                    Kill();
                }
            }
        }

    }
}