using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.ComponentModel.Design;

namespace graphook
{
    internal class Entity
    {
        public bool playable;
        private float i;
        public int id;
        double pi = 3.1415926535;
        int cframes;
        public SpriteBatch spriteBatch;
        public Vector2 center;
        private List<Collision> collisions;
        public DCollision dcl;
        private Vector2 vel;
        private MouseState previousMouseState;
        private MouseState currentMouseState;
        private MouseState huh;
        private Vector2 vel2;
        int subpx;
        bool unTouchable;
        public ParticleSystem particleSystem;
        int subpy;
        int particleFrames = 0;
        public bool isActivated;
        raycast ray;
        float ab;
        public Texture2D whiteTexture;
        int av;
        float b;
        float x1;
        float y1;
        public Entity(List<Collision> Collisions, List<Texture2D> textures, SpriteBatch spriteBatch)
        {
            dcl = new DCollision(new Vector2(90, 85), new Vector2(90, 85), 16, 16, Collisions);
            particleSystem = new ParticleSystem(textures, new Vector2(400, 240), 1);
            
            this.collisions = Collisions;
            whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            this.spriteBatch = spriteBatch;
        }

        private void Move(float i, float triRad, Vector2 center)
        {
            

            float angle = i;

            double x1 = triRad * Math.Cos(angle * pi / 180);

            double y1 = triRad * Math.Sin(angle * pi / 180);

            
            dcl.Position = new Vector2(center.X + (float)x1, center.Y + (float)y1);
        }
        public void Update()
        {
            Debug.WriteLine(dcl.Position.X);
            Debug.WriteLine(dcl.Position.Y);
            KeyboardState newState = Keyboard.GetState();
            if (vel.X > 0) { vel.X -= 1; }
            if (vel.X < 0) { vel.X += 1; }
            if (vel2.Y > 0) { vel2.Y -= 0.5f; }
            if (vel2.Y < 0) { vel2.Y += 0.5f; }
            if (vel2.X > 0) { vel2.X -= 0.5f; }
            if (vel2.X < 0) { vel2.X += 0.5f; }
            if (vel2.X < 1 && vel2.X > -1) { vel2.X = 0; }
            if (vel2.Y < 1 && vel2.Y > -1) { vel2.Y = 0; }

            
                
            
            if (vel.Y < -3) {
                subpy = 0;
                if (newState.IsKeyDown(Keys.Space))
                {

                    vel2.Y = 0;

                    
                    vel.Y += 3;
                    
                }
                else
                {
                    
                    vel.Y += 5;
                    
                }
            }
            else {
                if (!(cframes > 0))
                {
                    subpy = 20;
                }
            }
            if(cframes > 0)
            {
                subpy = 0;
            }

             
            if (!(newState.IsKeyDown(Keys.A) && newState.IsKeyDown(Keys.D)) && !(cframes - 5 > 0))
            {
                if (newState.IsKeyDown(Keys.D))
                {
                    vel.X = 20;
                }
                if (newState.IsKeyDown(Keys.A))
                {
                    vel.X = -20;
                }
            }
            if (newState.IsKeyDown(Keys.Space) && dcl.coyoteFrames > 0)
            {
                vel.Y = -54;
                dcl.coyoteFrames = 0;
                particleFrames = 6;
                
            }
            if (particleFrames > 0) { 
                particleFrames--;
                particleSystem.SpawnParticles();
            }

            
            cframes--;
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                ray = new raycast(collisions, 125, 10, dcl.Position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                if (ray.Collidepoint().collided)
                {

                    //ddcl.Position
                    //Collidepoint().imaginaryPoint.X;
                    float rayx = ray.Collidepoint().pos.X;
                    float rayy = ray.Collidepoint().pos.Y;
                    x1 = dcl.Position.X - rayx;
                    float x = Math.Abs(dcl.Position.X - rayx) * Math.Abs(dcl.Position.X - rayx);
                    float y = Math.Abs(dcl.Position.Y - rayy) * Math.Abs(dcl.Position.Y - rayy);
                    float triRad = (float)Math.Sqrt(x + y);
                    
                    isActivated = true;
                    ab = triRad;
                    
                    float angle = (float)Math.Atan2(rayy - dcl.Position.Y, rayx - dcl.Position.X);
                    i = (float)(angle * (180.0 / Math.PI)) + 180;
                    center = ray.Collidepoint().pos;
                    b = i;
                    
                    Debug.WriteLine(i);
                }
            }
            if (isActivated) {
                Move(b, ab, center);
                
                if (x1 < 0)
                {
                    
                    
                    
                    b -= 4f + (5 - ab / 30) + (vel2.X + vel2.Y) / 2 / 30;


                    if ((b < i - (180 - ab / 60) )|| dcl.colliding || currentMouseState.RightButton == ButtonState.Pressed &&
                previousMouseState.RightButton == ButtonState.Released) {
                        float angle = b;

                        double x1 = ab * Math.Cos(angle * pi / 180);

                        double y1 = ab * Math.Sin(angle * pi / 180);


                        
                        vel2 = new Vector2(ab * (float)pi * 4 / 90 * (float)Math.Cos((float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                            center.X + (float)x1 - dcl.Position.X)), ab * (float)pi * 4 / 90 * (float)Math.Sin(
                                (float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                                    center.X + (float)x1 - dcl.Position.X)));
                        isActivated = false;

                    }
                } else if (x1 > 0) {
                    b += 4f + (5 - ab / 30);
                    //if (b > i + 70)

                    
                    
                    if ((b > i + (180 - ab / 60)) || dcl.colliding || currentMouseState.RightButton == ButtonState.Pressed &&
                previousMouseState.RightButton == ButtonState.Released)
                    {
                    
                        float angle = b;

                        double x1 = ab * Math.Cos(angle * pi / 180);

                        double y1 = ab * Math.Sin(angle * pi / 180);



                        vel2 = new Vector2(ab * (float)pi * 4 / 90 * (float)Math.Cos((float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                            center.X + (float)x1 - dcl.Position.X)), ab * (float)pi * 4 / 90 * (float)Math.Sin(
                                (float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                                    center.X + (float)x1 - dcl.Position.X)));
                        isActivated = false;
                    }
                }
                
            }
            
            subpx /= 4;
            dcl.PreviousPosition = dcl.Position; 
            dcl.Position = new Vector2((int)vel.X / 10 + vel2.X + dcl.Position.X + subpx, (int)vel.Y / 10 + vel2.Y + dcl.Position.Y + subpy / 10);

            

            //Mouse.GetState().X
            dcl.Update();
        }
        public void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            spriteBatch.Draw(texture, start, null, Microsoft.Xna.Framework.Color.Brown,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }


    }

}