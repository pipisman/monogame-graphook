using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Drawing;
using System.Reflection.Metadata;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography;
using System.Threading;
namespace graphook
{
    internal class DCollision
    {
        public int Gravity = 20;
        public int fallingSpeed = 20;
        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; set; }
        public int Width { get; set; }
        public bool istouchingwall;
        public int Height { get; set; }
        public int coyoteFrames = 0;
        private List<triCol> triangleCollisions;
        private List<Collision> collisions;
        public bool colliding = false;
        //4 points
        public Entity player;
        public bool isActivated;
        public Vector2 _a;
        public Vector2 _b;
        public int xoffset;
        public int yoffset;
        public Vector2 _c;
        public Vector2 _d;
        private List<Vector2> dots;
        float lfym;
        
        bool coollideLastFrame;
        private Rectangle flaggedRectangle;
        private Rectangle flaggedRectangleY;
        public DCollision(Vector2 position, Vector2 previousPosition, int width, int height, List<Collision> Collisions, List<triCol> triangleCollisions)
        {
            this.triangleCollisions = triangleCollisions;
            PreviousPosition = previousPosition;
            Position = position;
            Width = width;
            Height = height;
            _a = new Vector2(position.X, position.Y + height);
            _b = new Vector2(position.X + width, position.Y + height);
            _c = new Vector2(position.X, position.Y);
            _d = new Vector2(position.X + width, position.Y);
            collisions = Collisions;
            dots = new List<Vector2> { _a, _b, _c, _d };

        }
        public void Update()
        {
            
            istouchingwall = false;
            isActivated = player.isActivated;
            fallingSpeed++;
            fallingSpeed = Math.Clamp(fallingSpeed, 5, 20);
            _a = new Vector2(Position.X, Position.Y + Height);
            _b = new Vector2(Position.X + Height, Position.Y + Height);
            _c = new Vector2(Position.X, Position.Y);
            _d = new Vector2(Position.X + Height, Position.Y);
            dots = new List<Vector2> { _a, _b, _c, _d };
            for (int i = 0; i < triangleCollisions.Count; i++)
            {

                //holy guacamole
                Vector2[] dotsb = new[] { triangleCollisions[i].DotA, triangleCollisions[i].DotB, triangleCollisions[i].DotC };
                for (int j = 0; j < dotsb.Length; j++)
                {
                    Vector2 dot = dotsb[j];
                    if (dot.X > _c.X && dot.X < _d.X
                    && dot.Y > _c.Y && dot.Y < _a.Y)
                    {
                        if (j == 1)
                        {
                            Position = new Vector2(Position.X, Position.Y - Math.Abs(dot.Y - _a.Y));
                            coyoteFrames = 6;
                        }
                    }
                }

                for (int c = 0; c < dots.Count; c++)
                {
                    
                    float areaSum = area(triangleCollisions[i].DotA, triangleCollisions[i].DotB, dots[c])
                      + area(triangleCollisions[i].DotB, triangleCollisions[i].DotC, dots[c])
                      + area(triangleCollisions[i].DotA, triangleCollisions[i].DotC, dots[c]);

                    float triangleArea = area(triangleCollisions[i].DotA, triangleCollisions[i].DotC, triangleCollisions[0].DotB);

                    if (Math.Abs(areaSum - triangleArea) < 0.05f)
                    {


                        //if (Math.Abs(FindIntersection(_a, new Vector2(_b.X, _b.Y - 3f), triangleCollisions[0].DotA, triangleCollisions[0].DotB).X - _b.X) >
                        //Math.Abs(FindIntersection(_b, _d, triangleCollisions[0].DotA, triangleCollisions[0].DotB).Y - _b.Y)) {
                        if (dots[c] == _b)
                        {
                            Vector2 intersection = FindIntersection(_b, _d, triangleCollisions[i].DotA, triangleCollisions[i].DotB);
                            if (intersection != Vector2.Zero)
                            {
                                Position = new Vector2(Position.X, Position.Y - Math.Abs(intersection.Y - _b.Y));
                                coyoteFrames = 6;
                            }
                            
                        }
                        if (dots[c] == _a)
                        {
                            Vector2 intersection = FindIntersection(_a, _c, triangleCollisions[i].DotC, triangleCollisions[i].DotB);
                            if (intersection != Vector2.Zero)
                            {
                                Position = new Vector2(Position.X, Position.Y - Math.Abs(intersection.Y - _b.Y));
                                coyoteFrames = 6;
                            }
                            
                        }

                        //}
                    }
                }
            }

            //pravoygulnik
            /*
            for (int i = 0; i < collisions.Count; i++)
            {
                if (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y
                    && _b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                {
                    if (isActivated)
                    {
                        Position = player.prp;
                        return;
                    }
                    colliding = true;
                    float v = _b.Y - collisions[i]._c.Y;
                    int diffY = Math.Abs((int)v);
                    int yOffset = 0;

                    while (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y
                    && _b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                    {
                        yOffset++;
                        Position = new Vector2(Position.X, collisions[i]._c.Y - yOffset);
                        _a = new Vector2(Position.X, Position.Y + Height);
                        _b = new Vector2(Position.X + Height, Position.Y + Height);
                        _c = new Vector2(Position.X, Position.Y);
                        _d = new Vector2(Position.X + Height, Position.Y);
                    }

                    coyoteFrames = 6;
                }
                else if (_b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                {
                    if (isActivated)
                    {
                        player.isActivated = false;
                        Position = player.prp;
                        return;
                    }
                    ;
                    colliding = true;
                    //Debug.WriteLine("Collision detected at _b");
                    float v = _b.Y - collisions[i]._c.Y;
                    int diffY = Math.Abs((int)v);

                    float b = _b.X - collisions[i]._c.X;
                    int diffX = Math.Abs((int)b);

                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y - diffY);
                        //Debug.WriteLine("horizontal");
                        coyoteFrames = 6;
                    }
                    else
                    {
                        Position = new Vector2(Position.X - diffX, Position.Y);
                        //Debug.WriteLine("vertical");
                        istouchingwall = true;
                        fallingSpeed = 5;
                    }
                }
                else if (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y)
                {
                    if (isActivated)
                    {
                        player.isActivated = false;
                        Position = player.prp;
                        return;
                    }

                    colliding = true;
                    //Debug.WriteLine("Collision detected at _a");
                    float v = _a.Y - collisions[i]._d.Y;
                    int diffY = Math.Abs((int)v);
                    //Debug.WriteLine(diffY);
                    float b = _a.X - collisions[i]._d.X;
                    int diffX = Math.Abs((int)b);
                    //Debug.WriteLine(diffX);
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y - diffY);
                        //Debug.WriteLine("horizontal");
                        coyoteFrames = 6;
                    }
                    else
                    {
                        Position = new Vector2(Position.X + diffX, Position.Y);
                        //Debug.WriteLine("vertical");
                        istouchingwall = true;
                        fallingSpeed = 5;
                    }
                }
                else if (_d.X > collisions[i]._c.X && _d.X < collisions[i]._d.X
                    && _d.Y > collisions[i]._c.Y && _d.Y < collisions[i]._a.Y)
                {
                    if (isActivated)
                    {
                        player.isActivated = false;
                        Position = player.prp;
                        return;
                    }
                    colliding = true;
                    //Debug.WriteLine("Collision detected at _a");
                    float v = _d.Y - collisions[i]._a.Y;
                    int diffY = Math.Abs((int)v);
                    //Debug.WriteLine(diffY);
                    float b = _d.X - collisions[i]._a.X;
                    int diffX = Math.Abs((int)b);
                    Debug.WriteLine(diffX);
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y + diffY);
                        //Debug.WriteLine("horizontal");
                    }
                    else
                    {
                        Position = new Vector2(Position.X - diffX, Position.Y);
                        //Debug.WriteLine("vertical");
                        istouchingwall = true;
                        fallingSpeed = 5;
                    }
                }
                else if (_c.X > collisions[i]._c.X && _c.X < collisions[i]._d.X
                    && _c.Y > collisions[i]._c.Y && _c.Y < collisions[i]._a.Y)
                {
                    if (isActivated)
                    {
                        player.isActivated = false;
                        Position = player.prp;
                        return;
                    }
                    colliding = true;
                    //Debug.WriteLine("Collision detected at _a");
                    float v = _c.Y - collisions[i]._b.Y;
                    int diffY = Math.Abs((int)v);
                    //Debug.WriteLine(diffY);
                    float b = _c.X - collisions[i]._b.X;
                    int diffX = Math.Abs((int)b);
                    //Debug.WriteLine(diffX);
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y + diffY);
                        //Debug.WriteLine("horizontal");
                    }
                    else
                    {
                        Position = new Vector2(Position.X + diffX, Position.Y);
                        //Debug.WriteLine("vertical");
                        istouchingwall = true;

                        fallingSpeed = 5;
                    }
                }
                else
                {
                    colliding = false;
                }


            }
            */
            
            List<Rectangle> flaggedCollisions = new List<Rectangle>();
            List<Rectangle> flaggedCollisionsY = new List<Rectangle>();
            Rectangle a = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            
            for (int i = 0; i < collisions.Count; i++)
            {
                Rectangle b = new Rectangle((int)collisions[i].Position.X, (int)collisions[i].Position.Y, collisions[i].Width, collisions[i].Height);
                Rectangle aint = Rectangle.Intersect(a, b);
                if (aint != Rectangle.Empty)
                {
                    Vector2 aCenter = new Vector2(a.X + a.Width / 2f, a.Y + a.Height / 2f);
                    Vector2 bCenter = new Vector2(b.X + b.Width / 2f, b.Y + b.Height / 2f);

                    //collisions[i-1] 
                    
                    if (aint.Width > aint.Height)
                    {
                        //allCollisions[]=[]
                        //Console.WriteLine("height: " + aint.Height + " width: " + aint.Width + " vector a: " + a.X + ", " + a.Y + " vector b: " + b.X + ", " + b.Y);

                        //if (coollideLastFrame)
                        //{
                            
                        //}   
                        if (aCenter.Y < bCenter.Y)
                        {
                            flaggedCollisions.Add(b);
                            //Position = new Vector2(Position.X, Position.Y - aint.Height);
                            lfym = aint.Height;
                            coyoteFrames = 999999;
                        }
                        else
                        {
                            flaggedCollisions.Add(b);
                        }
                    }
                    else
                    {
                        if (aCenter.X < bCenter.X)
                        {
                            flaggedCollisionsY.Add(b);
                            fallingSpeed = 5;
                        }
                        else
                        {
                            flaggedCollisionsY.Add(b);
                            fallingSpeed = 5;
                        }

                    }
                    //break;
                    coollideLastFrame = true;
                }
                else
                {
                    coollideLastFrame = false;
                    lfym = 0;
                }
            }
            flaggedRectangle = Rectangle.Empty;
            flaggedRectangleY = Rectangle.Empty;

            if (flaggedCollisionsY.Any())
            {
                Vector2 aCenter = new Vector2(a.X + a.Width / 2f, a.Y + a.Height / 2f);

                if (flaggedCollisionsY.Count > 1)
                {

                    int CenterY = (flaggedCollisionsY[0].Y + flaggedCollisionsY[1].Y) / 2;
                    if (CenterY < Position.Y)
                    {
                        flaggedRectangleY = flaggedCollisionsY[0];
                        Vector2 bCenter = new Vector2(flaggedCollisionsY[0].X + flaggedCollisionsY[0].Width / 2f, flaggedCollisionsY[0].Y + flaggedCollisionsY[0].Height / 2f);
                        Rectangle aint = Rectangle.Intersect(a, flaggedCollisionsY[0]);

                        if (aCenter.X < bCenter.X)
                        {
                            Position = new Vector2(Position.X - aint.Width - 2, Position.Y);
                        }
                        else
                        {
                            Position = new Vector2(Position.X + aint.Width + 2, Position.Y);
                        }

                    }
                    if (CenterY > Position.X)
                    {
                        flaggedRectangleY = flaggedCollisionsY[1];
                        Vector2 bCenter = new Vector2(flaggedCollisionsY[1].X + flaggedCollisionsY[1].Width / 2f, flaggedCollisionsY[1].Y + flaggedCollisionsY[1].Height / 2f);

                        Rectangle aint = Rectangle.Intersect(a, flaggedCollisionsY[1]);
                        if (aCenter.X < bCenter.X)
                        {
                            Position = new Vector2(Position.X - aint.Width - 2, Position.Y);
                        }
                        else
                        {
                            Position = new Vector2(Position.X + aint.Width + 2, Position.Y);
                        }
                    }
                    if (CenterY == Position.Y)
                    {
                        flaggedRectangleY = flaggedCollisionsY[0];
                        Vector2 bCenter = new Vector2(flaggedCollisionsY[0].X + flaggedCollisionsY[0].Width / 2f, flaggedCollisionsY[0].Y + flaggedCollisionsY[0].Height / 2f);
                        Rectangle aint = Rectangle.Intersect(a, flaggedCollisionsY[0]);
                        if (aCenter.X < bCenter.X)
                        {
                            Position = new Vector2(Position.X - aint.Width, Position.Y);
                        }
                        else
                        {
                            Position = new Vector2(Position.X + aint.Width, Position.Y);
                        }
                    }
                }
                else
                {
                    flaggedRectangleY = flaggedCollisionsY[0];
                    Vector2 bCenter = new Vector2(flaggedCollisionsY[0].X + flaggedCollisionsY[0].Width / 2f, flaggedCollisionsY[0].Y + flaggedCollisionsY[0].Height / 2f);
                    Rectangle aint = Rectangle.Intersect(a, flaggedCollisionsY[0]);
                    if (aCenter.X < bCenter.X)
                    {
                        Position = new Vector2(Position.X - aint.Width, Position.Y);
                    }
                    else
                    {
                        Position = new Vector2(Position.X + aint.Width, Position.Y);
                    }
                }
            }
            
            
            if (flaggedCollisions.Any())
            {
                Vector2 aCenter = new Vector2(a.X + a.Width / 2f, a.Y + a.Height / 2f);
                if (flaggedCollisions.Count > 1)
                {
                    int Centerx = (flaggedCollisions[0].X + flaggedCollisions[1].X) / 2;
                    if (Centerx < Position.X)
                    {
                        flaggedRectangle = flaggedCollisions[0];
                        Vector2 bCenter = new Vector2(flaggedCollisions[0].X + flaggedCollisions[0].Width / 2f, flaggedCollisions[0].Y + flaggedCollisions[0].Height / 2f);
                        Rectangle aint = Rectangle.Intersect(a, flaggedCollisions[0]);
                        if (aCenter.Y < bCenter.Y)
                        {
                        
                            Position = new Vector2(Position.X, Position.Y - aint.Height);
                        }
                        else
                        {
                            Position = new Vector2(Position.X, Position.Y + aint.Height);
                        }
                    }
                    if (Centerx > Position.X)
                    {
                        flaggedRectangle = flaggedCollisions[1];
                        Vector2 bCenter = new Vector2(flaggedCollisions[1].X + flaggedCollisions[1].Width / 2f, flaggedCollisions[1].Y + flaggedCollisions[1].Height / 2f);
                        Rectangle aint = Rectangle.Intersect(a, flaggedCollisions[1]);
                        if (aCenter.Y < bCenter.Y)
                        {
                            Position = new Vector2(Position.X, Position.Y - aint.Height);
                        }
                        else
                        {
                            Position = new Vector2(Position.X, Position.Y + aint.Height);
                        }
                    }
                    if (Centerx == Position.X)
                    {
                        flaggedRectangle = flaggedCollisions[0];
                        Vector2 bCenter = new Vector2(flaggedCollisions[0].X + flaggedCollisions[0].Width / 2f, flaggedCollisions[0].Y + flaggedCollisions[0].Height / 2f);
                        Rectangle aint = Rectangle.Intersect(a, flaggedCollisions[0]);
                        if (aCenter.Y < bCenter.Y)
                        {
                            Position = new Vector2(Position.X, Position.Y - aint.Height);
                        }
                        else
                        {
                            Position = new Vector2(Position.X, Position.Y + aint.Height);
                        }
                    }
                }
                else
                {
                    flaggedRectangle = flaggedCollisions[0];
                    Vector2 bCenter = new Vector2(flaggedCollisions[0].X + flaggedCollisions[0].Width / 2f, flaggedCollisions[0].Y + flaggedCollisions[0].Height / 2f);
                    Rectangle aint = Rectangle.Intersect(a, flaggedCollisions[0]);
                    if (aCenter.Y < bCenter.Y)
                    {
                        Position = new Vector2(Position.X, Position.Y - aint.Height);
                    }
                    else
                    {
                        Position = new Vector2(Position.X, Position.Y + aint.Height);
                    }
                }
            }
            
            flaggedCollisions.Clear();
            flaggedCollisionsY.Clear();
            


            //triygylnici

            float mousex = Mouse.GetState().X;
            float mousey = Mouse.GetState().Y;
            //float dotAtoPoint = (float)Math.Sqrt(Math.Abs(triangleCollisions[0].DotA.X - _b.X) * Math.Abs(triangleCollisions[0].DotA.X - _b.X) + Math.Abs(triangleCollisions[0].DotA.Y - _b.Y) * Math.Abs(triangleCollisions[0].DotA.Y - _b.Y));
            //float dotBtoPoint = (float)Math.Sqrt(Math.Abs(triangleCollisions[0].DotB.X - _b.X) * Math.Abs(triangleCollisions[0].DotB.X - _b.X) + Math.Abs(triangleCollisions[0].DotB.Y - _b.Y) * Math.Abs(triangleCollisions[0].DotB.Y - _b.Y));
            //float dotCtoPoint = (float)Math.Sqrt(Math.Abs(triangleCollisions[0].DotC.X - _b.X) * Math.Abs(triangleCollisions[0].DotC.X - _b.X) + Math.Abs(triangleCollisions[0].DotC.Y - _b.Y) * Math.Abs(triangleCollisions[0].DotC.Y - _b.Y));
            //float areaABP = 0.5 * Math.Abs(triangleCollisions[0].DotA.X * y2 + triangleCollisions[0].DotB.X * _b.Y + _b.X * y1 - y1 * triangleCollisions[0].DotB.X - y2 * _b.X - _b.Y * triangleCollisions[0].DotA.X)
            //float areaABP = 0.5 * abs(triangleCollisions[0].DotA.X * triangleCollisions[0].DotB.Y + triangleCollisions[0].DotB.X * y3 + x3 * triangleCollisions[0].DotA.Y - triangleCollisions[0].DotA.Y * triangleCollisions[0].DotB.X - triangleCollisions[0].DotB.Y * x3 - y3 * triangleCollisions[0].DotA.X);




            
            /*
            Vector2 mouse = new Vector2(mousex, mousey);

            float areaSum2 = area(triangleCollisions[0].DotA, triangleCollisions[0].DotB, mouse)
              + area(triangleCollisions[0].DotB, triangleCollisions[0].DotC, mouse)
              + area(triangleCollisions[0].DotA, triangleCollisions[0].DotC, mouse);

            float triangleArea2 = area(triangleCollisions[0].DotA, triangleCollisions[0].DotC, triangleCollisions[0].DotB);

            if (Math.Abs(areaSum2 - triangleArea2) < 0.05f)
            {
                Debug.WriteLine("bachka");
            }*/




                coyoteFrames--;
        }
        public int checkCollisionY(int x)
        {
            int xMod = 0;
            while (check(x - xMod))
            {
                if (x > 0)
                    xMod++;
                if (x < 0)
                    xMod--;
                if (!check(x - xMod))
                {
                    Console.WriteLine(x - xMod);
                    
                    return (x - xMod);
                    
                }
                
                    
            }
            return x;
        }
        private bool check(int x)
        {
            Rectangle a = new Rectangle((int)Position.X + x, (int)Position.Y, Width, Height);
            for (int i = 0; i < collisions.Count; i++)
            {
                Rectangle b = new Rectangle((int)collisions[i].Position.X, (int)collisions[i].Position.Y, collisions[i].Width, collisions[i].Height);
                Rectangle aint = Rectangle.Intersect(a, b);
                if (aint != Rectangle.Empty)
                {
                    return true;
                }
            }
            return false;

        }
        private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            spriteBatch.Draw(texture, start, null, Microsoft.Xna.Framework.Color.Green,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }
        private Vector2 FindIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            Vector2 r = B - A;
            Vector2 s = D - C;
            
            float denominator = Cross(r, s);

            if (MathF.Abs(denominator) < 1e-6f)
            {
                return new Vector2(0, 0); //null
                
            }
            //https://content.byui.edu/file/b8b83119-9acc-4a7b-bc84-efacf9043998/1/Math-2-11-2.html
            //https://www.youtube.com/watch?v=R0bGxNzgL2o   
            Vector2 AC = C - A;
            float t = Cross(AC, s) / denominator;
            float u = Cross(AC, r) / denominator;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                return A + t * r;
            }
            return new Vector2(0, 0);

        }

        private static float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        private float area(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return 0.5f * Math.Abs(p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p1.Y * p2.X - p2.Y * p3.X - p3.Y * p1.X);
        }
        public void Draw(SpriteBatch spriteBatch, int xoffset, int yoffset)
        {

            Texture2D whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });

            Rectangle destinationRectangle = new Rectangle((int)Position.X + xoffset, (int)Position.Y + yoffset, Width, Height);
            Rectangle destinationRectangle2 = new Rectangle((int)FindIntersection(_b, _d, triangleCollisions[0].DotA, triangleCollisions[0].DotB).X, (int)FindIntersection(_b, _d, triangleCollisions[0].DotA, triangleCollisions[0].DotB).Y, 16, 16);
            Rectangle destinationRectangle3 = new Rectangle((int)FindIntersection(_a, _c, triangleCollisions[0].DotC, triangleCollisions[0].DotB).X, (int)FindIntersection(_a, _c, triangleCollisions[0].DotC, triangleCollisions[0].DotB).Y, 16, 16);
            spriteBatch.Draw(whiteTexture, destinationRectangle, Microsoft.Xna.Framework.Color.Green);
            spriteBatch.Draw(whiteTexture, destinationRectangle2, Microsoft.Xna.Framework.Color.Purple);
            spriteBatch.Draw(whiteTexture, destinationRectangle3, Microsoft.Xna.Framework.Color.Purple);
            spriteBatch.Draw(whiteTexture, flaggedRectangleY, Microsoft.Xna.Framework.Color.Black);
            spriteBatch.Draw(whiteTexture, flaggedRectangle, Microsoft.Xna.Framework.Color.Purple);
        }
    }
}