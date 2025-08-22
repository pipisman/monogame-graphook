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
        Rectangle destinationRectangle;
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 _Pposition;

        //4 points
        public Vector2 _a;
        public Vector2 _b;
        public Vector2 _c;
        public Vector2 _d;
        private Texture2D texture;
        public bool isFlagged;
        

        public Collision(Vector2 position, int width, int height, Texture2D texture)
        {
            _Pposition = position;
            Position = position;
            Width = width;
            Height = height;
            _a = new Vector2(position.X + 5, position.Y + height);
            _b = new Vector2(position.X + width, position.Y + height);
            _c = new Vector2(position.X + 5, position.Y);
            _d = new Vector2(position.X + width, position.Y);
            destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            this.texture = texture;
        }   
        public void Update()
        {
            
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D whiteTexture, int xoffset, int yoffset)
        {
            
            
            destinationRectangle = new Rectangle((int)Position.X + xoffset, (int)Position.Y + yoffset, Width, Height);
            
            if (whiteTexture == null)
            {
                // Handle the error, maybe load the texture, or throw an exception
                // Example: throw new ArgumentNullException("whiteTexture cannot be null");
                return; // exit if texture is null to avoid further errors
            }
            if (!isFlagged)
            {
                spriteBatch.Draw(texture, destinationRectangle, Microsoft.Xna.Framework.Color.White);
            }
            
            else
            {
                spriteBatch.Draw(texture, destinationRectangle, Microsoft.Xna.Framework.Color.Purple);
            }
            
            
        }
    }
}