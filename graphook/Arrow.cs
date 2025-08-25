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
using System.Data.Common;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Reflection;
using System.Data;
using System.Dynamic;
using System.Xml.Linq;
namespace graphook
{
    internal class Arrow
    {
        public Vector2 Position;
        MouseState MouseState;
        float gravitySpeed;
        MouseState previousState;
        private static float InitSpeed = 8f;
        private float Speed = 0;
        float angle;
        List<Collision> collisions;
        Texture2D arrowTexture;
        public List<(Texture2D texture, Vector2 position, Rectangle? sourceRect,
              Microsoft.Xna.Framework.Color color, float rotation, Vector2 origin,
              float scale, SpriteEffects effects, float layerDepth)> Arrows;


        //spriteBatch.Draw(arrowTexture, Position, new Rectangle(0, 0, arrowTexture.Width, arrowTexture.Height), color,
        //        angle, new Vector2(arrowTexture.Width / 2f, arrowTexture.Height / 2f), 1.5f, SpriteEffects.None, 0f);
        public Arrow(Vector2 spawnPos, Texture2D texture, List<Collision> Collisions)
        {
            Position = spawnPos;
            arrowTexture = texture;
            collisions = Collisions;
            Arrows = new List<(Texture2D, Vector2, Rectangle?, Microsoft.Xna.Framework.Color, float, Vector2, float, SpriteEffects, float)>();

        }
        public void Update(Vector2 playerPosition)
        {
            previousState = MouseState;
            MouseState = Mouse.GetState();
            if (MouseState.MiddleButton == ButtonState.Pressed && previousState.MiddleButton != ButtonState.Pressed)
            {
                Position = playerPosition;
                Speed = InitSpeed;
            }
            if (Speed > 0)
            {
                gravitySpeed = 0;
                Speed -= 0.1f;
                angle = (float)Math.Atan2(MouseState.Y - Position.Y, MouseState.X - Position.X);
            } 
            if (Speed <= 0) gravitySpeed += 0.25f;
            float x = Position.X + Speed * (float)Math.Cos(angle);
            float y = Position.Y + Speed * (float)Math.Sin(angle);
            Position = new Vector2(x, y + gravitySpeed);
            Rectangle a = new Rectangle((int)Position.X, (int)Position.Y, 16, 8);
            for (int i = 0; i < collisions.Count; i++)
            {
                Rectangle b = new Rectangle((int)collisions[i].Position.X, (int)collisions[i].Position.Y, collisions[i].Width, collisions[i].Height);
                Rectangle aint = Rectangle.Intersect(a, b);
                if (aint != Rectangle.Empty)
                {
                    Arrows.Add(
                        (arrowTexture,
                         Position,
                         new Rectangle(0, 0, arrowTexture.Width, arrowTexture.Height),
                         Microsoft.Xna.Framework.Color.White,
                         angle,
                         new Vector2(arrowTexture.Width / 2f, arrowTexture.Height / 2f),
                         1.5f,
                         SpriteEffects.None,
                         0f)
                    );

                    Position = new Vector2(0, 1000);
                }
            }

        }
        public void Draw(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color color)
        {
            spriteBatch.Draw(arrowTexture, Position, new Rectangle(0, 0, arrowTexture.Width, arrowTexture.Height), color,
                angle, new Vector2(arrowTexture.Width / 2f, arrowTexture.Height / 2f), 1.5f, SpriteEffects.None, 0f);
            foreach (var arrow in Arrows)
            {
                spriteBatch.Draw(
                    arrow.texture,
                    arrow.position,
                    arrow.sourceRect,
                    arrow.color,
                    arrow.rotation,
                    arrow.origin,
                    arrow.scale,
                    arrow.effects,
                    arrow.layerDepth
                );
            }

            



            

        }
        
        
        
    }
}
