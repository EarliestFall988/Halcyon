using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        #region colors
        public Color TextColor { get; set; } = Color.White;
        public Color EnabledColor { get; set; } = Color.White;
        public Color DisabledColor { get; set; } = Color.Gray;
        #endregion


        /// <summary>
        /// The button constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="location"></param>
        public Button(string text, Rectangle location)
        {
            Text = text;
            SpriteLocation = location;
            WorldType = SpriteType.UI;
        }

        public void MouseOver()
        {
            if (ButtonState == GUIState.Disabled)
                return;
        }

        public void SetButtonEnabled(bool enabled)
        {
            ButtonState = enabled ? GUIState.Enabled : GUIState.Disabled;
        }

        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float rotationOffset)
        {
            base.DrawObject(time, cameraPositionOffset, rotationOffset);

            //draw the text here...
        }

    }
}
