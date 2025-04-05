using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace graphook
{
    internal class Entity
    {
        public bool playable;
        public int id;
        int cframes;
        private List<Collision> collisions;
        public DCollision dcl;
        private Vector2 vel;

        private Vector2 vel2;
        int subpx;
        bool unTouchable;
        public ParticleSystem particleSystem;
        int subpy;
        int particleFrames = 0;
        MouseState previousMouseState;
        raycast ray;
        
        public Entity(List<Collision> Collisions, List<Texture2D> textures)
        {
            dcl = new DCollision(new Vector2(90, 85), new Vector2(90, 85), 16, 16, Collisions);
            particleSystem = new ParticleSystem(textures, new Vector2(400, 240), 1);
            previousMouseState = Mouse.GetState();
            this.collisions = Collisions;
        }
        public void Update()
        {
            KeyboardState newState = Keyboard.GetState();
            if (vel.X > 0) { vel.X -= 1; }
            if (vel.X < 0) { vel.X += 1; }
            if (vel2.Y > 0) { vel2.Y -= 1; }
            if (vel2.Y < 0) { vel2.Y += 1; }
            if (vel2.X > 0) { vel2.X -= 1; }
            if (vel2.X < 0) { vel2.X += 1; }
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
                    vel.X = 16;
                }
                if (newState.IsKeyDown(Keys.A))
                {
                    vel.X = -16;
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

            Debug.WriteLine("vel2.x: " + vel2.X);

            Debug.WriteLine("vel2.y: " + vel2.Y);
            cframes--;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && cframes <= 0)
            {
                ray = new raycast(collisions, 500, 10, dcl.Position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                if (ray.Collidepoint().collided)
                {
                    vel2 = new Vector2(20 * (float)Math.Cos(ray.Collidepoint().angle), 20 * (float)Math.Sin(ray.Collidepoint().angle));
                    unTouchable = false;
                    cframes = 20;
                }
            }
            
            subpx /= 4;
            dcl.PreviousPosition = dcl.Position; 
            dcl.Position = new Vector2((int)vel.X / 10 + vel2.X + dcl.Position.X + subpx, (int)vel.Y / 10 + vel2.Y + dcl.Position.Y + subpy / 10);
            //Mouse.GetState().X
            dcl.Update();
        }


    }
}