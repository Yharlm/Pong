using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vector2 = System.Numerics.Vector2;

namespace Pong
{
    public class Paddel
    {
        public Vector2 Scale = new Vector2(20, 70);
        public Vector2 position = new Vector2(10, 0);
        public int score = 0;
        public bool HitBall(Vector2 Pos, Vector2 velocity)
        {
            
            if (Pos.X < position.X + Scale.X && Pos.X > position.X)
            {
                if (Pos.Y > position.Y && Pos.Y < position.Y + Scale.Y)
                    return true;


                //ballVelocity.X = p2.HitBall(ballVelocity, ballPosition);
            }
            return false;
        }

        public void BotMove(Vector2 ball_hit)
        {
            position.Y -= (position.Y - ball_hit.Y) / 10;
        }
    }
    public class Game : Microsoft.Xna.Framework.Game
    {
        Random random = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Paddel p1;
        Paddel p2;

        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 ballVelocity;
        Vector2 ball_hit;
        Vector2 mousePos;

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


            ballVelocity = new Vector2(0, 0);
            //TODO: инициализирайте булевата променлива
            p1 = new Paddel();
            p2 = new Paddel();
            p2.position = new Vector2(770, 0);
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


            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                p1.position.Y += 14f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                p1.position.Y -= 14f;
            }

            //Raycast
            for (int i = 0; i < 60; i++)
            {
                if ((ballPosition + (ballVelocity * i)).X >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 10 || (ballPosition + (ballVelocity * i)).X < 10)
                {

                    ball_hit.Y = ((ballPosition + (ballVelocity * i)).Y) - p2.Scale.Y / 2;
                    break;

                }

            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                ballVelocity = mousePos - ballPosition;
            }

            //TODO: Изменете ballPosition.Y координатата в зависимост от стойността на булевата променлива down
            ballPosition += ballVelocity;
            if (ballVelocity.X > 0)
            {
                p2.BotMove(ball_hit);
            }
            else
            {
                p1.BotMove(ball_hit);
            }

            



            //Window it borders

            Collisions();
            Thread thread = new Thread(() =>
            {
                ballPosition.X = 390;
                ballVelocity.Y = 0;
                ballVelocity.X = 0;
                Thread.Sleep(2000);
                
                ballVelocity.X = -10;

            });

            if (ballPosition.X > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2)
            {
                p1.score += 1;
                thread.Start();


            }
            if (ballPosition.X < 0)
            {
                p2.score += 1;
                thread.Start();

            }
            

            base.Update(gameTime);
        }

        

        private void Collisions()
        {
            
            
            if (ballPosition.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 || ballPosition.Y < 0)
            {
                ballVelocity.Y = -ballVelocity.Y;
            }
            if(p1.HitBall(ballPosition, ballVelocity))
            {

                ballVelocity.X = -ballVelocity.X+p1.score;
                if(ballPosition.Y < p1.position.Y + p1.Scale.Y/2)
                {
                    ballVelocity.Y -= random.Next(0, 4); ;
                }
                else
                {
                    ballVelocity.Y += random.Next(0, 4); ;
                }

            }
            if (p2.HitBall(ballPosition, ballVelocity))
            {
                
                ballVelocity.X = -ballVelocity.X + 0.2f;
                if (ballPosition.Y < p2.position.Y + p2.Scale.Y / 2)
                {
                    ballVelocity.Y -= random.Next(0, 4); ;
                }
                else
                {
                    ballVelocity.Y += random.Next(0, 4); ;
                }

            }


        }
        
        protected override void Draw(GameTime gameTime)
        {
            string Score = $"{p1.score}:{p2.score}";

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("file"), ballPosition.ToString(), Vector2.One, Color.White);
            _spriteBatch.DrawString(Content.Load<SpriteFont>("file"), Score, new Vector2(300, 200), Color.White, 0f, Vector2.One, 7f, SpriteEffects.None, 0f);

            _spriteBatch.End();



            _spriteBatch.Begin();
            _spriteBatch.Draw(
                ballTexture,
                ballPosition + Vector2.One * 0.3f,
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
            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), p2.position, null, Color.White, 0f, Vector2.One, 1, SpriteEffects.None, 0f);
            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), p2.position, null, Color.White, 0f, Vector2.One, 1, SpriteEffects.None, 0f);

            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), new Vector2(p2.position.X, p2.position.Y), null, Color.Wheat, 0f, Vector2.One, p1.Scale / 14, SpriteEffects.None, 0f);

            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), new Vector2(p2.position.X + p2.Scale.X, p2.position.Y), null, Color.White, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), new Vector2(p2.position.X + p2.Scale.X, p2.position.Y + p2.Scale.Y), null, Color.Red, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);

            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), new Vector2(p1.position.X, p1.position.Y), null, Color.Wheat, 0f, Vector2.One, p1.Scale / 14, SpriteEffects.None, 0f);

            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), new Vector2(p1.position.X + p1.Scale.X, p1.position.Y), null, Color.White, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), new Vector2(p1.position.X + p1.Scale.X, p1.position.Y + p1.Scale.Y), null, Color.Red, 0f, Vector2.One, 0.3f, SpriteEffects.None, 0f);
            for (int i = 0; i < 60; i++)
            {
                if ((ballPosition + (ballVelocity * i)).X >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - 10)
                {


                    break;

                }
                _spriteBatch.Draw(Content.Load<Texture2D>("dirt"), ballPosition + (ballVelocity * i), null, Color.Red, 0f, Vector2.One, 0.5f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();

            

            
            base.Draw(gameTime);
        }
    }
}
