using Engine.Engines;
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
    class ColourObjectComponent : DrawableGameComponent
    {
        Texture2D tx;
        Color myColour;
        public Color hoverColor = new Color(new Vector3(0, 0, 0));
        Color currentColour;

        Vector2 pos;
        bool chosen = false;

        public Color MyColour
        {
            get{return myColour;}
            set { myColour = value; }
        }

        public bool Chosen
        {
            get { return chosen; }
            set { chosen = value; }
        }

        public Rectangle Target
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y, tx.Width, tx.Height); }
        }

        public ColourObjectComponent(Game g, Vector2 pos, Texture2D tx, Color c):base(g)
        {
            g.Components.Add(this);
            this.pos = pos;
            this.tx = tx;
            myColour = c;
            currentColour = c;
        }

        public override void Update(GameTime gameTime)
        {
            if (chosen && InputEngine.CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                chosen = false;
            }
            if (OthersChecked())
            {
                return;
            }
            if (InputEngine.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (Target.Contains(InputEngine.MousePosition) && !chosen)
                {
                    chosen = true;
                    currentColour = new Color(MyColour, 128);
                }
            }
            if (Target.Contains(InputEngine.MousePosition) && !chosen)
            {
                currentColour = hoverColor;
            }
            else if (!Target.Contains(InputEngine.MousePosition) && !chosen)
            {
                currentColour = myColour;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            sp.Begin();
            sp.Draw(tx, Target, currentColour);
            sp.End();
            base.Draw(gameTime);
        }

        public bool OthersChecked()
        {
            var others = Game.Components.Where(c => c.GetType() == typeof(ColourObjectComponent) && !c.Equals(this)).ToList();

            foreach(var item in others)
            {
                if (((ColourObjectComponent)item).Chosen)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
