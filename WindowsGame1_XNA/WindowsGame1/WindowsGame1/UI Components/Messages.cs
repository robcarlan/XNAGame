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

namespace WindowsGame1
{
    public class Message
    {
        public short timeSpan;
        public short fadeValue = 1000;  //Start at 100, used to get alpha component
        string text;
        string owner;

        Rectangle source;
        public enum msgType { Default, Quest, System, NPC };
        public msgType type;
        public string getText() { return text; }
        public string getOwner() { return owner; }
        public string getFullText()
        {
            if (getOwner() == null)
                return getText();
            else
                return ("[" + getOwner() + "]: " + getText());
        }
        public Message(string Owner, string msg)
        {
            owner = Owner;
            text = msg;
            type = msgType.Default;
        }
        public Message(string Owner, string msg, msgType messageType)
        {
            owner = Owner;
            text = msg;
            type = messageType;
        }
        public Message(string Owner, string msg, msgType messageType, Rectangle source)
        {
            owner = Owner;
            text = msg;
            type = messageType;
            this.source = source;
        }
    }
}
