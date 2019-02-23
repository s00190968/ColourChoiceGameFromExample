using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ColourGameRemadeFromExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ColourObjectComponent[] playerColours;
        ColourObjectComponent computerColour;
        ColourObjectComponent colorComp;

        Color[] choices = new Color[] { Color.Red, Color.Green, Color.Blue };

        enum colors { Red, Green, Blue };

        bool gameOver = false;
        private string msg = "Click left mouse button to choose a colour.";
        private SpriteFont font;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            new InputEngine(this);
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            setupPlayerChoices();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (InputEngine.IsKeyPressed(Keys.P))
            {
                Reset();
            }

            if (!gameOver)
            {
                checkChosen();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch sp = Services.GetService<SpriteBatch>();

            sp.Begin();
            sp.DrawString(font, msg, new Vector2(10,10), Color.Black);
            sp.End();
            base.Draw(gameTime);
        }

        private void setupPlayerChoices()
        {
            playerColours = new ColourObjectComponent[3];
            int gap = 10;
            Texture2D tx = Content.Load<Texture2D>(@"colourTx");
            font = Content.Load<SpriteFont>(@"msgFont");

            Vector2 startPos = GraphicsDevice.Viewport.Bounds.Center.ToVector2()
                - new Vector2(playerColours.Length * (tx.Width + gap) / 2, tx.Height / 2);

            for (int i = 0; i < playerColours.Length; i++)
            {
                playerColours[i] = new ColourObjectComponent(this, startPos, tx, choices[i]);
                startPos += new Vector2(tx.Width + gap, 0);
            }
        }

        private void checkChosen()
        {
            ColourObjectComponent chosen = null;
            foreach (ColourObjectComponent c in playerColours)
            {
                if (c.Chosen)
                {
                    chosen = c;
                    break;
                }
            }
            if (chosen != null)
            {
                Random rand = new Random();
                computerColour = new ColourObjectComponent(this, new Vector2(200, 200), Content.Load<Texture2D>(@"colourTx"), choices[rand.Next(0, choices.Length)]);

                if (computerColour.MyColour == chosen.MyColour)
                {
                    msg = "You chose right, you win.";
                }
                else
                {
                    msg = "You chose wrong, you lose.";
                }
                msg += "\nPress ESC to quit, P to play again.";
                gameOver = true;
            }
        }

        private void Reset()
        {
            Components.Remove(computerColour);
            computerColour = null;
            foreach (ColourObjectComponent c in playerColours)
            {
                c.Chosen = false;
            }
            msg = "Choose a colour by clicking on a colour of your choice";
            gameOver = false;
        }
    }
}
