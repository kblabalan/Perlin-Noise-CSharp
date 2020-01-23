using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Noise;
using System;
using System.Threading.Tasks;

namespace PerlinNoiseMono
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        string hashStr;
        int seed;
        double offset_x, offset_y, offset_z, scale;
        Color[] buffer;
        Perlin perlin;
        bool isCalculating;
        Texture2D texture;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            hashStr = "hashbrown";
            seed = hashStr.GetHashCode();
            perlin = new Perlin(seed);
            scale = 0.1d;
            texture = new Texture2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            buffer = new Color[graphics.PreferredBackBufferWidth * graphics.PreferredBackBufferHeight];
            isCalculating = false;
            CalculateNoise();
            base.Initialize();
        }
		
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var calculate = false;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();
            if ((state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt)) && state.IsKeyDown(Keys.Enter))
                graphics.ToggleFullScreen();

            if (state.IsKeyDown(Keys.W))
            {
                offset_y -= 1d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.A))
            {
                offset_x -= 1d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.S))
            {
                offset_y += 1d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.D))
            {
                offset_x += 1d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.Q))
            {
                scale -= 0.001d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.E))
            {
                scale += 0.001d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.R))
            {
                offset_z += 0.01d;
                calculate = true;
            }
            if (state.IsKeyDown(Keys.F))
            {
                offset_z -= 0.01d;
                calculate = true;
            }

            if (calculate && !isCalculating)
                Task.Run(() => CalculateNoise());

            base.Update(gameTime);
        }

        private void CalculateNoise()
        {
            isCalculating = true;
            for (int y = 0; y < graphics.PreferredBackBufferHeight; y++)
            {
                for (int x = 0; x < graphics.PreferredBackBufferWidth; x++)
                {
                    double output = perlin.Noise((offset_x + x) * scale, (offset_y + y) * scale, offset_z);
                    int percentToByte = Math.Abs((int)(255d * output));
                    if (percentToByte < 8)
                        buffer[x + (y * graphics.PreferredBackBufferWidth)] = Color.DarkBlue;
                    else if (percentToByte < 16)
                        buffer[x + (y * graphics.PreferredBackBufferWidth)] = Color.Blue;
                    else if (percentToByte < 32)
                        buffer[x + (y * graphics.PreferredBackBufferWidth)] = Color.Brown;
                    else if (percentToByte < 128)
                        buffer[x + (y * graphics.PreferredBackBufferWidth)] = Color.Green;
                    else if (percentToByte < 192)
                        buffer[x + (y * graphics.PreferredBackBufferWidth)] = Color.White;
                }
            }
            isCalculating = false;
            texture.SetData(buffer);
            Window.Title = $"Perlin Noise - {hashStr} ({seed}) - Offset: [{offset_x},{offset_y},{offset_z}] Scale: {scale}";
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
