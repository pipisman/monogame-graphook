using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel.Design;
using System.Drawing;


namespace graphook
{
    public class Particle_sin
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        private Random random;
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Microsoft.Xna.Framework.Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }
        double pi = 3.1415926535;
        double angle, x1, y1;
        Microsoft.Xna.Framework.Color colorMod;

        public Particle_sin(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Microsoft.Xna.Framework.Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            random = new Random();
            //colorMod = new Color((int)((float)random.NextDouble() * 255), 255, 255);
            colorMod = color;
        }

        public void Update()
        {
            TTL--;
            Color = Color;
            Position += Velocity * 0.75f;
            Angle += AngularVelocity;
            

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Microsoft.Xna.Framework.Rectangle sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            //spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y + 3), sourceRectangle, Color.DarkCyan,
            //    Angle, origin, Size, SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture, Position, sourceRectangle, colorMod,
                Angle, origin, Size, SpriteEffects.None, 0f);

        }
    }
}