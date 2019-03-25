#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NeonShooter.Shared.Source;

#endregion

namespace NeonShooter.Shared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;

            Instance = this;
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
            Art.LoadContent(Content);


            EntityManager.Add(PlayerShip.Instance);
            //TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif
            // TODO: Add your update logic here	
            Input.Update(gameTime);
            EntityManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
