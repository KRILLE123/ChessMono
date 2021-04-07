using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Chess
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private MouseState oldState;
        private SpriteBatch _spriteBatch;
        private static GameProperties gameProperties = new GameProperties(1920, 1080);
        private EventHandler eventHandler = new EventHandler(gameProperties);

        static bool test = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = gameProperties.WindowHeight;
            _graphics.PreferredBackBufferWidth = gameProperties.WindowWidth;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //gameProperties.CreateGrid();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameProperties.textures.Add(Content.Load<Texture2D>("line"));

            for (int i = 1; i <= 12; i++)
            {
                gameProperties.pieces[i] = Content.Load<Texture2D>(i.ToString());
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState newState = Mouse.GetState();


            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                string _event = eventHandler.GetCurrentEvent(newState);

                gameProperties.ChoosePiece(gameProperties.currentGridPropery);
            }
            oldState = newState;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            gameProperties.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
