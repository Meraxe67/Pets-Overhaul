using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
namespace PetsOverhaul.UI
{
    class AbilityCooldown : UIElement
    {
        Color color = Main.MouseTextColorReal;
        int RemainingCooldown => Main.LocalPlayer.GetModPlayer<GlobalPet>().timer;
        int BaseCooldown => Main.LocalPlayer.GetModPlayer<GlobalPet>().timerMax;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ModContent.GetInstance<Personalization>().AbilityDisplayDisable == false && Main.playerInventory == false && (ModContent.GetInstance<Personalization>().AbilityDisplayUnused == false || BaseCooldown > 0))
            {
                if (ModContent.GetInstance<Personalization>().AbilityDisplayInfo == false)
                {
                    spriteBatch.DrawString(FontAssets.MouseText.Value, "Base Pet Ability Cooldown:\n" + (BaseCooldown == 0 ? "Current Pet has no Ability Cooldown!" : Math.Round((float)BaseCooldown / 60, 1).ToString() + " second" + (BaseCooldown > 60 ? "s" : "")), new Vector2(Main.screenWidth - 348, Main.screenHeight - 220), Color.Black, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(FontAssets.MouseText.Value, "Base Pet Ability Cooldown:\n" + (BaseCooldown == 0 ? "Current Pet has no Ability Cooldown!" : Math.Round((float)BaseCooldown / 60, 1).ToString() + " second" + (BaseCooldown > 60 ? "s" : "")), new Vector2(Main.screenWidth - 350, Main.screenHeight - 220), color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
                if (RemainingCooldown > 0)
                {
                    spriteBatch.DrawString(FontAssets.MouseText.Value, "Remaining Pet Ability Cooldown:\n" + Math.Round((float)RemainingCooldown / 60, 1).ToString() + " second" + (RemainingCooldown > 60 ? "s" : ""), new Vector2(Main.screenWidth - 348, Main.screenHeight - 150), Color.Black, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(FontAssets.MouseText.Value, "Remaining Pet Ability Cooldown:\n" + Math.Round((float)RemainingCooldown / 60, 1).ToString() + " second" + (RemainingCooldown > 60 ? "s" : ""), new Vector2(Main.screenWidth - 350, Main.screenHeight - 150), color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    spriteBatch.DrawString(FontAssets.MouseText.Value, "Remaining Pet Ability Cooldown:\n" + "Pet Ability Ready!", new Vector2(Main.screenWidth - 348, Main.screenHeight - 150), Color.Black, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(FontAssets.MouseText.Value, "Remaining Pet Ability Cooldown:\n" + "Pet Ability Ready!", new Vector2(Main.screenWidth - 350, Main.screenHeight - 150), new Color(236, 201, 201), 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
    }
    class CooldownCanvas : UIState
    {
        public AbilityCooldown cooldownDisplay;
        public override void OnInitialize()
        {
            cooldownDisplay = new AbilityCooldown();
            Append(cooldownDisplay);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class AbilityCooldownDisplaySystem : ModSystem
    {
        internal CooldownCanvas cooldownDisplay;
        private UserInterface _cooldownDisplay;
        public override void Load()
        {
            cooldownDisplay = new CooldownCanvas();
            cooldownDisplay.Activate();
            _cooldownDisplay = new UserInterface();
            _cooldownDisplay.SetState(cooldownDisplay);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _cooldownDisplay?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PetsOverhaul: Pet Ability Cooldown Display",
                    delegate
                    {
                        _cooldownDisplay.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}