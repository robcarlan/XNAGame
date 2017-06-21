using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.UI_Components.Controls
{
    /// <summary>
    /// Class used to manage controls on an interface - icons, buttons
    /// </summary>
    public class ControlCollection
    {
        Texture2D texture;
        SpriteList sprites;
        SpriteFont font;
        Dictionary<int, WindowIcon> controls;
        Dictionary<int, Text> text;
        List<int> textControls;
        

        //Dictionary for larger controls i.e a scroll box
        bool useTextAA;

        //Debug
        public List<string> debug;

        public ControlCollection(Texture2D texture, SpriteFont font, SpriteList sprites)
        {

            textControls = new List<int>();
            debug = new List<string>();
            controls = new Dictionary<int, WindowIcon>();
            text = new Dictionary<int, Text>();

            this.sprites = sprites;
            this.font = font;
            this.texture = texture;
 
            debug.Add(Functions.WriteDebugLine("Initialised new Control Collection"));
        }

        public void setAntialias(bool useAntialias)
        {
            if (useTextAA = useAntialias)
                debug.Add(Functions.WriteDebugLine("Control Collection set to use antialiasing for text."));
            else
                debug.Add(Functions.WriteDebugLine("Control Collection set to not use antialias for text."));  
        }

        public bool addControl(WindowIcon control, int ID)
        {
            if (controls.ContainsKey(ID))
            {
                debug.Add(Functions.WriteDebugLine("Could not add control " + ID + ", id already exists."));
                return false;
            }
            control.setID(ID);
            controls.Add(ID, control);

            if (control.hasText)
            {
                textControls.Add(ID);
                debug.Add(Functions.WriteDebugLine("Added control with ID " + ID + ". Control contains text."));
            }
            else debug.Add(Functions.WriteDebugLine("Added control with ID " + ID));

            return true;
        }

        public bool removeControl(int ID)
        {
            bool containsKey = controls.ContainsKey(ID);
            if (containsKey)
            {
                controls.Remove(ID);
                debug.Add(Functions.WriteDebugLine("Removed control with ID " + ID + "."));
            }
            else
            {
                debug.Add(Functions.WriteDebugLine("Could not find control " + ID + " to remove from control list."));
            }
            if (textControls.Contains(ID))
            {
                textControls.Remove(ID);
                debug.Add(Functions.WriteDebugLine("Removed control with ID " + ID + " from text control list."));
            }

            if (containsKey)
                return true;
            else return false;
        }

        public bool addText(Text textCtrl, int ID)
        {
            if (text.ContainsKey(ID))
            {
                debug.Add(Functions.WriteDebugLine("Could not add text " + ID + ", id already exists."));
                return false;
            }
            textCtrl.setID(ID);

            text.Add(ID, textCtrl);

            debug.Add(Functions.WriteDebugLine("Added text with ID " + ID));

            return true;
        }

        public bool removeText(int ID)
        {
            if (text.ContainsKey(ID))
            {
                text.Remove(ID);
                debug.Add(Functions.WriteDebugLine("Removed text with ID " + ID + "."));
                return true;
            }
            else
            {
                debug.Add(Functions.WriteDebugLine("Could not find control " + ID + " to remove from control list."));
                return false;
            }
        }

        public void drawControls(SpriteBatch Spritebatch)
        {
            //draw base controls
            Spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            foreach (WindowIcon ctrl in controls.Values)
            {
                Spritebatch.Draw(texture, ctrl.absolutePosition, sprites.list[ctrl.spriteName][ctrl.getCurrentState()],
                    Color.White, 0f, Vector2.Zero, consts.UI_CONTROL_SCALE, SpriteEffects.None, 0f);
            }

            Spritebatch.End();

            //draw text, using antialiasing if set
            if (useTextAA)
                Spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
            else
                Spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            for (int itr = 0; itr < textControls.Count; itr++)
            {
                //Draw the text
                if (controls[itr].text.isVisible)
                    consts.drawText(controls[itr].text.text, controls[itr].text.outlineWidth, controls[itr].text.position,
                        ref Spritebatch, controls[itr].text.colour, controls[itr].text.scale, ref font);
                
            }

            foreach (Text textObj in text.Values)
            {
                if (textObj.isVisible)
                    consts.drawText(textObj.text, textObj.outlineWidth, textObj.position,
                        ref Spritebatch, textObj.colour, textObj.scale, ref font);
            }

            Spritebatch.End();
        }

        public void onMove(Point increment)
        {
            foreach (WindowIcon temp in controls.Values)
                temp.incrementPosition(increment);
            foreach (Text temp in text.Values)
                temp.increasePosition(increment);
        }
    }
}
