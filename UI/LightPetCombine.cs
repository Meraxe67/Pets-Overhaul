using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
namespace PetsOverhaul.UI
{   // This class wraps the vanilla ItemSlot class into a UIElement. The ItemSlot class was made before the UI system was made, so it can't be used normally with UIState. 
    // By wrapping the vanilla ItemSlot class, we can easily use ItemSlot.
    // ItemSlot isn't very modder friendly and operates based on a "Context" number that dictates how the slot behaves when left, right, or shift clicked and the background used when drawn. 
    // If you want more control, you might need to write your own UIElement.
    // I've added basic functionality for validating the item attempting to be placed in the slot via the validItem Func. 
    // See ExamplePersonUI for usage and use the Awesomify chat option of Example Person to see in action.
    // Took from VanillaItemSlotWrapper.cs of ExampleMod
    internal class LightPetSlot : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public LightPetSlot(int context = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);

            Width.Set(40 * scale, 0f);
            Height.Set(40 * scale, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }
    }

    //public class LightPetSlot : UIPanel //first custom slot prototype
    //{
    //    public Item itemInTheSlot = new();
    //    public override void LeftClick(UIMouseEvent evt)
    //    {
    //        if (Main.mouseItem.IsAir && itemInTheSlot.IsAir)
    //            return;

    //        (Main.mouseItem, itemInTheSlot) = (itemInTheSlot, Main.mouseItem); //Apparently you can swap values using Tuples without typing out a temporary variable
    //        SoundEngine.PlaySound(SoundID.Grab);
    //    }
    //}

    public class ClickPreventedPanel : UIPanel
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }
    public class LightPetCombineCanvas : UIState
    {
        internal LightPetSlot slot1;
        internal LightPetSlot slot2;
        public override void OnInitialize()
        {
            ClickPreventedPanel panel = new ClickPreventedPanel();
            panel.Width.Set(550, 0);
            panel.Height.Set(210, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            UIText header = new UIText("Light Pet Combining Menu");
            header.HAlign = 0.5f;
            header.Top.Set(15, 0);
            panel.Append(header);

            UIPanel button = new UIPanel();
            button.Width.Set(80, 0);
            button.Height.Set(30, 0);
            button.HAlign = 0.5f;
            button.VAlign = 0.5f;
            button.OnLeftClick += OnButtonClick;
            panel.Append(button);

            UIText text = new UIText("Combine");
            text.HAlign = text.VAlign = 0.5f;
            button.Append(text);

            UIText infoText = new UIText("Put down two Light Pets of same type to combine them into one.\nNew Light Pet will inherit highest qualities of its parents.");
            infoText.Top.Set(125, 0);
            panel.Append(infoText);

            slot1 = new LightPetSlot();
            slot1.Top.Set(55, 0);
            slot1.Left.Set(35, 0);
            slot1.Width.Set(40, 0);
            slot1.Height.Set(40, 0);
            panel.Append(slot1);

            slot2 = new LightPetSlot();
            slot2.Top.Set(55, 0);
            slot2.Left.Set(115, 0);
            slot2.Width.Set(40, 0);
            slot2.Height.Set(40, 0);
            panel.Append(slot2);
        }
        public void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Item item = LightPetItem.CombineLightPets(slot1.Item, slot2.Item);
            if (item != null) 
            {
                Main.LocalPlayer.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc,"Light Pet Combine"), item);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PetTamer.openLightCombineMenu == false)
            {
                if (slot1.Item is not null && slot1.Item.IsAir == false)
                {
                    Main.LocalPlayer.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc, "Light Pet Combine Close"), slot1.Item);
                    slot1.Item.TurnToAir();
                }
                if (slot2.Item is not null && slot2.Item.IsAir == false)
                {
                    Main.LocalPlayer.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc, "Light Pet Combine Close"), slot2.Item);
                    slot2.Item.TurnToAir(); 
                }
                return;
            }
            base.Draw(spriteBatch);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class LightPetCombineDisplaySystem : ModSystem
    {
        internal LightPetCombineCanvas Display;
        private UserInterface _display;
        public override void Load()
        {
            Display = new LightPetCombineCanvas();
            Display.Activate();
            _display = new UserInterface();
            _display.SetState(Display);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _display?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PetsOverhaul: Light Pet Combine Menu",
                    delegate
                    {
                        _display.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}