using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Monogame1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D ballTexture;
        Vector2 ballPos;
        float ballSpeed;
        SpriteFont font;
        Rectangle rect;
        Vector2 rectPos;
        Vector2 rect2Pos;
        Rectangle rect2;
        float ballRadiusW;
        float ballRadiusH;
        Texture2D pixel;
        Texture2D pixelBound;
        float Gravity = 25;
        List<Rectangle> rectans;
        double nn = 0;
        double nnn = 0;
        Stopwatch sw;
        Rectangle line;
        List<Vector2> points;
        List<Vector2> points2;
        Vector2 vecbuff;
        bool isbuff;
        bool btnp;
        Draw draw;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
                                
            ballPos = new Vector2(_graphics.PreferredBackBufferWidth / 2, 
                _graphics.PreferredBackBufferHeight / 2);

            ballSpeed = 100f;

            CreateRects();

            rectPos = new Vector2(_graphics.PreferredBackBufferWidth - (rect.Width / 2), _graphics.PreferredBackBufferHeight / 2);
            rect2Pos = new Vector2((rect2.Width / 2), _graphics.PreferredBackBufferHeight / 2);

            sw = new Stopwatch();
            sw.Start();

            //TargetElapsedTime = new System.TimeSpan(10000);

            rectans = new();

            new Boundary(new Vector2(_graphics.PreferredBackBufferWidth - (rect.Width), (_graphics.PreferredBackBufferHeight / 2) - (rect.Height / 2)), 
                new Vector2(_graphics.PreferredBackBufferWidth - (rect.Width), (_graphics.PreferredBackBufferHeight / 2) + (rect.Height / 2)));

            new Boundary(new Vector2(rect2.Width, (_graphics.PreferredBackBufferHeight / 2) - (rect2.Height / 2)),
                new Vector2(rect2.Width, (_graphics.PreferredBackBufferHeight / 2) + (rect2.Height / 2)));

            new Boundary(new Vector2(0, 0), new Vector2(_graphics.PreferredBackBufferWidth, 0));
            new Boundary(new Vector2(_graphics.PreferredBackBufferWidth, 0), new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            new Boundary(new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), new Vector2(0, _graphics.PreferredBackBufferHeight));
            new Boundary(new Vector2(0, _graphics.PreferredBackBufferHeight), new Vector2(0, 0));

            Vector2 Mid = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            for (float i = 0f; i < 360f; i += 3f)
            {
                float rad1 = Geometry.AngleToRadian(i);
                float rad2 = Geometry.AngleToRadian(i + 3f);

                float x1 = (float)Math.Cos(rad1) * 100;
                float y1 = (float)Math.Sin(rad1) * 100;

                float x2 = (float)Math.Cos(rad2) * 100;
                float y2 = (float)Math.Sin(rad2) * 100;
                Vector2 point1 = new Vector2(x1 + Mid.X, y1 + Mid.Y);
                Vector2 point2 = new Vector2(x2 + Mid.X, y2 + Mid.Y);

                new Boundary(point1, point2);

            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            draw = new Draw(_spriteBatch);
            ballTexture = Content.Load<Texture2D>("ball");
            font = Content.Load<SpriteFont>("font");          
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            ballRadiusW = 12;
            ballRadiusH = 12;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {           

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
          
            GetInputs(gameTime);

            RayCasting();

            nn = gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //Draw Circle

            draw.DrawCircle(ballPos, 12, Color.White, pixel);
            draw.FillCircle(12, ballPos, Color.White * 0.1f, pixel);

            //Draw Circle

            _spriteBatch.Draw(pixel, rectPos, rect, Color.DarkViolet, 0f, 
                new Vector2(rect.Width / 2, rect.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            _spriteBatch.Draw(pixel, rect2Pos, rect2, Color.DarkViolet, 0f,
                new Vector2(rect2.Width / 2, rect2.Height / 2), Vector2.One, SpriteEffects.None, 0f);

         
            for (int i = 0; i < points.Count; i++)
            {              
                draw.DrawLine(pixel, points[i], points2[i], Color.White, 1f);
            }

            foreach (Boundary b in Boundary.Boundaries)
            {              
                draw.DrawLine(pixel, b.Vec1, b.Vec2, Color.Black, 3f);
            }

            nnn = gameTime.ElapsedGameTime.TotalSeconds;
            
            _spriteBatch.DrawString(font, nn.ToString(), new Vector2(0, 0), Color.Black);
            //_spriteBatch.DrawString(font, ((float)sw.ElapsedMilliseconds / 1000).ToString(), new Vector2(150, 0), Color.Black);
            _spriteBatch.DrawString(font, nnn.ToString(), new Vector2(500, 0), Color.Black);

            _spriteBatch.End();         

            base.Draw(gameTime);
        }

        public void GetInputs(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                ballPos.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                ballPos.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                ballPos.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                ballPos.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Q))
            {
                ballPos = new Vector2(mstate.X, mstate.Y);             
            }

            if (mstate.LeftButton == ButtonState.Pressed)
            {
                Vector2 point = new Vector2(mstate.X, mstate.Y);

                if (!btnp)
                {
                    if (isbuff)
                    {
                        new Boundary(vecbuff, point);
                        vecbuff = new Vector2();
                        isbuff = false;
                    }
                    else
                    {
                        vecbuff = point;
                        isbuff = true;
                    }
                    btnp = true;
                }
            }

            if (mstate.LeftButton == ButtonState.Released)
            {
                if (btnp) btnp = false;
            }
           
        }
                 
        public void RayCasting()
        {
            points = new();
            points2 = new();

            for (float i = 0f; i <= (Math.PI * 2); i += 0.05f)
            {
                Vector2 point = Geometry.VectorRotation(i, 12);

                float x = (point.X + ballPos.X);
                float y = (point.Y + ballPos.Y);

                Vector2 p = new Vector2(x, y);
                Vector2 vec2;

                bool isadd = false;
                float distance = float.PositiveInfinity;
                Vector2 victor = new Vector2();
                float temp;

                foreach (Boundary bound in Boundary.Boundaries)
                {
                    vec2 = Geometry.LineIntersect(p, Geometry.GetRotation(p, ballPos), bound.Vec1, bound.Vec2);
                                     
                    if (vec2.X == p.X && vec2.Y == p.Y)
                    {
                        continue;
                    }

                    else
                    {
                        temp = Vector2.Distance(vec2, p);

                        if (temp < distance)
                        {
                            victor = vec2;
                            distance = temp;
                            isadd = true;
                        }
                    }

                }                                    

                if (isadd)
                {
                    points.Add(p);
                    points2.Add(victor);
                    CollisionBoundry(p, victor);                  
                }
                              
            }
          
        }
      
        public void CollisionBoundry(Vector2 point1, Vector2 point2)
        {

            float distance = Vector2.Distance(point1, point2);

            if (distance <= 1f)
            {                  
                float rad = Geometry.GetRotation(point2, point1);
                Vector2 vec = Geometry.VectorRotation(rad, ballRadiusW * 1.2f);
                float x = point2.X - vec.X;
                float y = point2.Y - vec.Y;

                ballPos = new Vector2(x, y);
            }                           
            
        }
                             
        public void CreateRects()
        {
            rect = new Rectangle();
            rect.Height = 150;
            rect.Width = 25;
            rect2 = new Rectangle();
            rect2.Height = 150;
            rect2.Width = 25;
            line = new Rectangle();
            line.Width = 200;
            line.Height = 1;
        }

    }
}
