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

            Vector2 Mid = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            for (float i = 0f; i < 360f; i += 6f)
            {
                float rad1 = AngleToRadian(i);
                float rad2 = AngleToRadian(i + 6f);

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
            ballTexture = Content.Load<Texture2D>("ball");
            font = Content.Load<SpriteFont>("font");          
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            //ballRadiusW = ballTexture.Width / 2;
            //ballRadiusH = ballTexture.Height / 2;
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

            HitWall();
            RayCasting();
            //CollisionBoundry();

            //ballPos.Y += Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            nn = gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //_spriteBatch.Draw(ballTexture, ballPos, null, Color.White,
            //0f, new Vector2(ballRadiusW, ballRadiusH),
            //Vector2.One, SpriteEffects.None, 0f);

            //Draw Circle

            DrawCircle(ballPos, 12, Color.White, pixel);
            FillCircle(12, ballPos, Color.White * 0.1f, pixel);

            //Draw Circle

            _spriteBatch.Draw(pixel, rectPos, rect, Color.DarkViolet, 0f, 
                new Vector2(rect.Width / 2, rect.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            _spriteBatch.Draw(pixel, rect2Pos, rect2, Color.DarkViolet, 0f,
                new Vector2(rect2.Width / 2, rect2.Height / 2), Vector2.One, SpriteEffects.None, 0f);

         
            for (int i = 0; i < points.Count; i++)
            {              
                DrawLine(pixel, points[i], points2[i], Color.White, 1f);
            }

            foreach (Boundary b in Boundary.Boundaries)
            {              
                DrawLine(pixel, b.Vec1, b.Vec2, Color.Black, 3f);
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

        public void DrawLine(Texture2D texture, Vector2 Vec1, Vector2 Vec2, Color color, float scale)
        {
            _spriteBatch.Draw(texture, Vec1, null, color,
                         (float)Math.Atan2(Vec2.Y - Vec1.Y, Vec2.X - Vec1.X),
                         new Vector2(0f, (float)texture.Height / 2),
                         new Vector2(Vector2.Distance(Vec1, Vec2), scale),
                         SpriteEffects.None, 0f);
        }

        public void DrawCircle(Vector2 Mid, float Radius, Color color, Texture2D Texture)
        {
            for (int i = 0; i <= 360; i++)
            {
                float rad1 = AngleToRadian(i);
                float rad2 = AngleToRadian(i + 1);

                float x1 = (float)Math.Cos(rad1) * Radius;
                float y1 = (float)Math.Sin(rad1) * Radius;

                float x2 = (float)Math.Cos(rad2) * Radius;             
                float y2 = (float)Math.Sin(rad2) * Radius;

                Vector2 point1 = new Vector2(x1 + Mid.X, y1 + Mid.Y);
                Vector2 point2 = new Vector2(x2 + Mid.X, y2 + Mid.Y);

                DrawLine(Texture, point1, point2, color, 1f);

            }
        }

        public void FillCircle(float radius, Vector2 Mid, Color color, Texture2D Texture)
        {
            for (int i = 0; i <= 360; i++)
            {
                float rad = AngleToRadian(i);
                var point = GetComps(rad, radius);                           
                DrawLine(Texture, Mid, new Vector2(point.X + Mid.X, point.Y + Mid.Y), color, 1f);
            }
        }

        public void HitWall()
        {
            if (ballPos.X > _graphics.PreferredBackBufferWidth - ballRadiusW)
            {
                ballPos.X = _graphics.PreferredBackBufferWidth - ballRadiusW;
            }
            else if (ballPos.X < ballRadiusW)
            {
                ballPos.X = ballRadiusW;
            }

            if (ballPos.Y > _graphics.PreferredBackBufferHeight - ballRadiusH)
            {
                ballPos.Y = _graphics.PreferredBackBufferHeight - ballRadiusH;
            }
            else if (ballPos.Y < ballRadiusH)
            {
                ballPos.Y = ballRadiusH;
            }
        }

        public void HitRect()
        {
            if (ballPos.Y + ballRadiusH >= rectPos.Y - (rect.Height / 2) && ballPos.Y - ballRadiusH <= rectPos.Y + (rect.Height / 2))
            {
                if (ballPos.X + ballRadiusW >= rectPos.X - (rect.Width / 2))
                {
                    ballPos.X = (rectPos.X - (rect.Width / 2)) - ballRadiusW;
                }
            }
        }

        public void RayCasting()
        {
            points = new();
            points2 = new();

            for (float i = 0f; i <= (Math.PI * 2); i += 0.05f)
            {
                Vector2 point = GetComps(i, 12);

                float x = (point.X + ballPos.X);
                float y = (point.Y + ballPos.Y);

                Vector2 p = new Vector2(x, y);

                Vector2 vec2 = new Vector2();

                List<Vector2> vectors = new();
                bool isadd = false;

                foreach (Boundary bound in Boundary.Boundaries)
                {
                    vec2 = LookAt(p, GetRad(p, ballPos), bound.Vec1, bound.Vec2);

                    if (vec2.X == p.X && vec2.Y == p.Y)
                    {
                        continue;
                    }
                    else
                    {
                        vectors.Add(vec2);
                    }

                }
          
                float distance = float.PositiveInfinity;
                Vector2 victor = new Vector2();

                for (int j = 0; j < vectors.Count; j++)
                {
                    float temp = Vector2.Distance(vectors[j], p);

                    if (temp < distance)
                    {
                        victor = vectors[j];
                        distance = temp;
                        isadd = true;
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

        public Vector2 LookAt(Vector2 p, float angle, Vector2 wall1, Vector2 wall2)
        {
            Vector2 dir = GetComps(angle, 1);

            float x1 = wall1.X;
            float x2 = wall2.X;
            float x3 = p.X;
            float x4 = (p.X + dir.X);

            float y1 = wall1.Y;
            float y2 = wall2.Y;
            float y3 = p.Y;
            float y4 = (p.Y + dir.Y);         
            

            double den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
            double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

            double newx;
            double newy;

            if (t > 0 && t < 1 && u > 0)
            {
                newx = x1 + (t * (x2 - x1));
                newy = y1 + (t * (y2 - y1));

                return new Vector2((float)newx, (float)newy);
            }
            else
            {
                return p;
            }
            
        }

        public void CollisionBoundry(Vector2 point1, Vector2 point2)
        {
            float dist = float.PositiveInfinity;
            var vec1 = new Vector2();
            var vec2 = new Vector2();
            bool isyes = false;

            //for (int i = 0; i < points.Count; i++)
            //{
                //var point1 = points[i];
                //var point2 = points2[i];

                float distance = Vector2.Distance(point1, point2);

                if (distance <= 1f && distance < dist)
                {                  
                    dist = distance;
                    vec1 = point1;
                    vec2 = point2;
                    isyes = true;                                  
                }                           
            //}

            if (isyes)
            {
                float rad = GetRad(vec2, vec1);
                Vector2 vec = GetComps(rad, ballRadiusW * 1.3f);
                float x = vec2.X - vec.X;
                float y = vec2.Y - vec.Y;

                ballPos = new Vector2(x, y);
            }

        }
             

        public Vector2 GetComps(float angle, float radius)
        {
            double x = Math.Cos(angle) * radius;
            double y = Math.Sin(angle) * radius;
            return new Vector2((float)x, (float)y);
        }

        public float GetRad(Vector2 vec, Vector2 vec2)
        {
            double theta = Math.Atan2(vec.Y - vec2.Y, vec.X - vec2.X);

            return (float)theta;
        }

        public float AngleToRadian(float angle)
        {
            return angle * (float)(Math.PI / 180);
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
