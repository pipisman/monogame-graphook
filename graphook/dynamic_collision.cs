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
namespace graphook
{
    internal class DCollision
    {

        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int coyoteFrames = 0;
        private List<Collision> collisions;

        //4 points
        public Vector2 _a;
        public Vector2 _b;
        public Vector2 _c;
        public Vector2 _d;

        public DCollision(Vector2 position, Vector2 previousPosition, int width, int height, List<Collision> Collisions)
        {
            PreviousPosition = previousPosition;
            Position = position;
            Width = width;
            Height = height;
            _a = new Vector2(position.X, position.Y + height);
            _b = new Vector2(position.X + width, position.Y + height);
            _c = new Vector2(position.X, position.Y);
            _d = new Vector2(position.X + width, position.Y);
            collisions = Collisions;
            
        }
        public void Update()
        {
            
            _a = new Vector2(Position.X, Position.Y + Height);
            _b = new Vector2(Position.X + Height, Position.Y + Height);
            _c = new Vector2(Position.X, Position.Y);
            _d = new Vector2(Position.X + Height, Position.Y);

            for (int i = 0; i < collisions.Count; i++)
            {
                if (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y
                    && _b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                {
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
                        
                    coyoteFrames = 5;
                }
                if (_b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                {
                    Debug.WriteLine("Collision detected at _b");
                    float v = _b.Y - collisions[i]._c.Y;
                    int diffY = Math.Abs((int)v);
                    
                    float b = _b.X - collisions[i]._c.X;
                    int diffX = Math.Abs((int)b);
                    
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y - diffY);
                        Debug.WriteLine("horizontal");
                        coyoteFrames = 4;
                    }
                    else
                    {
                        Position = new Vector2(Position.X - diffX, Position.Y);
                        Debug.WriteLine("vertical");
                    }
                }
                if (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y)
                {
                    Debug.WriteLine("Collision detected at _a");
                    float v = _a.Y - collisions[i]._d.Y;
                    int diffY = Math.Abs((int)v);
                    Debug.WriteLine(diffY);
                    float b = _a.X - collisions[i]._d.X;
                    int diffX = Math.Abs((int)b);
                    Debug.WriteLine(diffX);
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y - diffY);
                        Debug.WriteLine("horizontal");
                        coyoteFrames = 4;
                    }
                    else
                    {
                        Position = new Vector2(Position.X + diffX, Position.Y);
                        Debug.WriteLine("vertical");
                    }
                }
                if (_d.X > collisions[i]._c.X && _d.X < collisions[i]._d.X
                    && _d.Y > collisions[i]._c.Y && _d.Y < collisions[i]._a.Y)
                {
                    Debug.WriteLine("Collision detected at _a");
                    float v = _d.Y - collisions[i]._a.Y;
                    int diffY = Math.Abs((int)v);
                    Debug.WriteLine(diffY);
                    float b = _d.X - collisions[i]._a.X;
                    int diffX = Math.Abs((int)b);
                    Debug.WriteLine(diffX);
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y + diffY);
                        Debug.WriteLine("horizontal");
                    }
                    else
                    {
                        Position = new Vector2(Position.X - diffX, Position.Y);
                        Debug.WriteLine("vertical");
                    }
                }
                if (_c.X > collisions[i]._c.X && _c.X < collisions[i]._d.X
                    && _c.Y > collisions[i]._c.Y && _c.Y < collisions[i]._a.Y)
                {
                    Debug.WriteLine("Collision detected at _a");
                    float v = _c.Y - collisions[i]._b.Y;
                    int diffY = Math.Abs((int)v);
                    Debug.WriteLine(diffY);
                    float b = _c.X - collisions[i]._b.X;
                    int diffX = Math.Abs((int)b);
                    Debug.WriteLine(diffX);
                    if (diffX > diffY)
                    {
                        Position = new Vector2(Position.X, Position.Y + diffY);
                        Debug.WriteLine("horizontal");
                    }
                    else
                    {
                        Position = new Vector2(Position.X + diffX, Position.Y);
                        Debug.WriteLine("vertical");
                    }
                }
                

            }
            if (coyoteFrames > 6) { coyoteFrames--; }
        }
        private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            spriteBatch.Draw(texture, start, null, Microsoft.Xna.Framework.Color.Green,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            Texture2D whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });

            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            spriteBatch.Draw(whiteTexture, destinationRectangle, Microsoft.Xna.Framework.Color.Green);
        }
    }
}
