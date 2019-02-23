using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColourGameRemadeFromExample
{
    class ColourObject
    {
        Texture2D tx;
        Color myColour;
        bool visible = true;

        Vector2 pos;
        bool chosen = false;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Color MyColour
        {
            get { return myColour; }
            set { myColour = value; }
        }

        public bool Chosen
        {
            get { return chosen; }
            set {chosen = value; }
        }

        public Rectangle Target
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y, tx.Width, tx.Height); }
        }

        public ColourObject(Vector2 pos, Texture2D tx, Color c)
        {
            this.pos = pos;
            this.tx = tx;
            myColour = c;
        }

        public void update(GameTime t)
        {
            MouseState m = Mouse.GetState();
            if (m.LeftButton == ButtonState.Pressed)
            {
                if (Target.Contains(new Point(m.X, m.Y)))
                {
                    chosen = true;
                }
            }
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin();
            sp.Draw(tx, Target, myColour);
            sp.End();
        }
    }
}
