﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace Pong
{
    public class Paddel
    {
        public Vector2 Scale = new Vector2(20, 70);
        public Vector2 position = new Vector2(0,0);

    }
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Paddel p1;
        Paddel p2;
        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballVelocity;
        Vector2 mousePos;
        float ballSpeed;
        bool down;
        //TODO: Добавете нова булева променлива down, която показва дали топката се движи нагоре/надолу

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                       _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 300f;

            ballVelocity = new Vector2(0, 0);
            //TODO: инициализирайте булевата променлива
            p1 = new Paddel();
            p2 = new Paddel();
            p2.position = new Vector2(790, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if(Keyboard.GetState().IsKeyDown(Keys.S))
            {
                p1.position.Y += 4f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                p1.position.Y -= 4f;
            }

            p2.position.Y = ballPosition.Y-p2.Scale.Y/3;


            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                ballVelocity = mousePos - ballPosition;
            }
            float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //TODO: Изменете ballPosition.Y координатата в зависимост от стойността на булевата променлива down
            ballPosition += ballVelocity;
            if (ballPosition.X > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 /*|| ballPosition.X < 0*/)
            {
                ballVelocity.X = -ballVelocity.X;
            }
            else
            if (p1.position.X+p1.Scale.X > ballPosition.X && p1.position.X < ballPosition.X && p1.position.Y < ballPosition.Y && p1.position.Y + p1.Scale.Y > ballPosition.Y)
            {
                ballVelocity.X = -ballVelocity.X;

            }
            if (ballPosition.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 || ballPosition.Y < 0)
            {
                ballVelocity.Y = -ballVelocity.Y;
            }
            if (ballPosition.X > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 /*|| ballPosition.X < 0*/)
            {
                ballVelocity.X = -ballVelocity.X;
            }
            else
            if (p2.position.X + p2.Scale.X > ballPosition.X && p2.position.X < ballPosition.X && p2.position.Y < ballPosition.Y && p2.position.Y + p2.Scale.Y > ballPosition.Y)
            {
                ballVelocity.X = -ballVelocity.X;

            }
            



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                0.3f,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();
            _spriteBatch.Begin();
            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), p2.position, null, Color.White, 0f, Vector2.One, p2.Scale / 20, SpriteEffects.None, 0f);

            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), p1.position, null, Color.White, 0f, Vector2.One, p1.Scale/20, SpriteEffects.None, 0f);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(Content.Load<SpriteFont>("file"), ballPosition.ToString(), Vector2.Zero, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
