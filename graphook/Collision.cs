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
namespace graphook
{
    internal class Collision
    {

        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        //4 points
        public Vector2 _a;
        public Vector2 _b;
        public Vector2 _c;
        public Vector2 _d;

        public Collision(Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
            _a = new Vector2(position.X, position.Y + height);
            _b = new Vector2(position.X + width, position.Y + height);
            _c = new Vector2(position.X, position.Y);
            _d = new Vector2(position.X + width, position.Y);

        }
        public void Update()
        {
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });

            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            spriteBatch.Draw(whiteTexture, destinationRectangle, Microsoft.Xna.Framework.Color.Red);
        }
    }
}
