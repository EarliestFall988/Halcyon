using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.GUI_Elements
{
    /// <summary>
    /// the button
    /// </summary>
    public sealed class Button : AtlasSprite
    {
        public string Text { get; set; }

        private GUIState ButtonState = GUIState.Enabled;

        public SpriteFont Font { get; set; }

        #region colors
        public Color TextColor { get; set; } = Color.Black;
        public Color EnabledColor { get; set; } = Color.White;
        public Color DisabledColor { get; set; } = Color.Gray;
        #endregion

        public Action ButtonClickAction;


        /// <summary>
        /// The button constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="location"></param>
        public Button(string text, Rectangle location, SpriteFont font)
        {
            Text = text;
            SpriteLocation = location;
            WorldType = SpriteType.UI;
            Font = font;
        }


        public void MouseClick()
        {
            ButtonClickAction();
        }

        public void SetButtonEnabled(bool enabled)
        {
            ButtonState = enabled ? GUIState.Enabled : GUIState.Disabled;
        }


        protected override void UpdateObject(GameTime time)
        {
            base.UpdateObject(time);

            switch (ButtonState)
            {
                case GUIState.Enabled:
                    color = EnabledColor;
                    break;
                case GUIState.Disabled:
                    color = DisabledColor;
                    break;
                case GUIState.Hovered:
                    color = Color.LightGray;
                    break;
                case GUIState.Clicked:
                    color = Color.DarkGray;
                    break;
            }

            if(ButtonState == GUIState.Disabled)
            {
                return;
            }

            if(Mouse.GetState().X > transform.position.X && Mouse.GetState().X < transform.position.X + SpriteLocation.Width * transform.scaleValue)
            {
                if (Mouse.GetState().Y > transform.position.Y && Mouse.GetState().Y < transform.position.Y + SpriteLocation.Height * transform.scaleValue)
                {
                    if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        ButtonState = GUIState.Clicked;
                    }
                    else
                    {
                        if (ButtonState == GUIState.Clicked)
                        {
                            MouseClick();
                        }

                        ButtonState = GUIState.Hovered;
                    }
                }
                else
                {
                    ButtonState = GUIState.Enabled;
                }
            }
            else
            {
                ButtonState = GUIState.Enabled;
            }
        }

        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float rotationOffset)
        {
            base.DrawObject(time, cameraPositionOffset, rotationOffset);

            Vector2 textMiddlePoint = Font.MeasureString(Text) / 2;

            batch.DrawString(Font, Text, new Vector2(transform.position.X + SpriteLocation.Width / 2 * transform.scaleValue, transform.position.Y + SpriteLocation.Height / 2 * transform.scaleValue), TextColor, 0, textMiddlePoint, 1, Effect, 0.5f);

        }

    }
}
