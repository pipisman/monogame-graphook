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
        public Vector2 prp;
        private Vector2 vel2;
        int subpx;
        KeyboardState previousState;
        KeyboardState currentState;
        bool wjf = false;
        bool unTouchable;
        public ParticleSystem particleSystem;
        float subpy;
        int particleFrames = 0;
        public bool isActivated;
        raycast ray;
        float ab;
        public Texture2D texture;
        public Texture2D whiteTexture;
        public List<Vector2> positions = [];
        int av;
        private Random random;
        float b;
        float cooldown;
        float x1;
        
        float y1;
        public Entity(List<Collision> Collisions, List<Texture2D> textures, SpriteBatch spriteBatch, List<triCol> triCollisions)
        {
            dcl = new DCollision(new Vector2(90, 85), new Vector2(90, 85), 13, 18, Collisions, triCollisions);
            particleSystem = new ParticleSystem(textures, new Vector2(400, 240), 1);
            
            this.collisions = Collisions;
            whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            this.spriteBatch = spriteBatch;
            random = new Random();
            dcl.isActivated = isActivated;
        }

        private void Move(float q, float triRad, Vector2 center)
        {
            positions = [];

            float angle = q;
            for (int k = 0; k < triRad / 5; k++)
            {
                double ghX1 = k * 5 * Math.Cos(angle * pi / 180);

                double ghY1 = k * 5 * Math.Sin(angle * pi / 180);

                positions.Add(new Vector2((float)ghX1 + center.X + random.Next(-1, 2), (float)ghY1 + center.Y + random.Next(-1, 2)));
            }
            double x1 = triRad * Math.Cos(angle * pi / 180);

            double y1 = triRad * Math.Sin(angle * pi / 180);
            float xr = center.X + (float)x1;
            float yr = center.Y + (float)y1;
            Vector2 _a = new Vector2(xr, yr + 16);
            Vector2 _b = new Vector2(xr + 16, yr + 16);
            Vector2 _c = new Vector2(xr, yr);
            Vector2 _d = new Vector2(xr + 16, yr);
            for (int i = 0; i < collisions.Count; i++)
            {
                if (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y
                    && _b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                {
                    isActivated = false;
                    return;
                }
                else if (_b.X > collisions[i]._c.X && _b.X < collisions[i]._d.X
                    && _b.Y > collisions[i]._c.Y && _b.Y < collisions[i]._a.Y)
                {
                    isActivated = false;
                    return;
                }
                else if (_a.X > collisions[i]._c.X && _a.X < collisions[i]._d.X
                    && _a.Y > collisions[i]._c.Y && _a.Y < collisions[i]._a.Y)
                {
                    isActivated = false;
                    return;
                }
                else if (_d.X > collisions[i]._c.X && _d.X < collisions[i]._d.X
                    && _d.Y > collisions[i]._c.Y && _d.Y < collisions[i]._a.Y)
                {
                    isActivated = false;
                    return;
                }
                else if (_c.X > collisions[i]._c.X && _c.X < collisions[i]._d.X
                    && _c.Y > collisions[i]._c.Y && _c.Y < collisions[i]._a.Y)
                {
                    isActivated = false;
                    return;
                }
            }


            prp = dcl.Position;
            dcl.Position = new Vector2(center.X + (float)x1, center.Y + (float)y1);
        }
        public void Update()
        {

            previousState = currentState;
            currentState = Keyboard.GetState();
            if (vel.X > 0) { vel.X -= 2; }
            if (vel.X > 0 && vel.X < 2) { vel.X = 0; }
            if (vel.X < 0) { vel.X += 2; }
            if (vel.X < 0 && vel.X > -2) { vel.X = 0; }
            if (vel2.Y > 0) { vel2.Y -= 0.2f; }
            if (vel2.Y < 0) { vel2.Y += 0.2f; }
            if (vel2.X > 0) { vel2.X -= 0.125f; }
            if (vel2.X < 0) { vel2.X += 0.125f; }
            if (vel2.X < 1 && vel2.X > -1) { vel2.X = 0; }
            if (vel2.Y < 1 && vel2.Y > -1) { vel2.Y = 0; }

            wjf = false;
                
            if (dcl.istouchingwall && cooldown <= 0 && !previousState.IsKeyDown(Keys.Space) && currentState.IsKeyDown(Keys.Space))
            {
                wjf = true;
                cooldown = 10;
            }
            cooldown--;

            if (vel.Y < -3) {
                subpy = 0;
                if (currentState.IsKeyDown(Keys.Space) && previousState.IsKeyDown(Keys.Space))
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
                    if (((currentState.IsKeyDown(Keys.A) && previousState.IsKeyDown(Keys.A)) || (currentState.IsKeyDown(Keys.D) && previousState.IsKeyDown(Keys.D)))) {
                        subpy = dcl.fallingSpeed + 5;
                    }
                        
                    else
                        subpy = 25;
                }
            }
            if(cframes > 0)
            {
                subpy = 0;
            }

             
            if (!(currentState.IsKeyDown(Keys.A) && currentState.IsKeyDown(Keys.D)) && !(cframes - 5 > 0))
            {
                if (currentState.IsKeyDown(Keys.D))
                {

                    vel.X = dcl.checkCollisionY(20);
                }
                if (currentState.IsKeyDown(Keys.A))
                {
                    vel.X = dcl.checkCollisionY(-20);
                }
            }
            if (currentState.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space) && dcl.coyoteFrames > 0 || wjf)
            {
                vel.Y = -40;
                
                wjf = false;
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
                ray = new raycast(collisions, 125, 5, dcl.Position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
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
                    
                    
                }
            }
            if (isActivated) {
                if (dcl.colliding)
                {
                    isActivated = false;
                    return;
                }
                if (currentState.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space))
                {
                    if (dcl.colliding)
                    {
                        isActivated = false;
                        return;
                    }
                    isActivated = false;
                    float angle = b;

                    double x1 = ab * Math.Cos(angle * pi / 180);

                    double y1 = ab * Math.Sin(angle * pi / 180);



                    vel2 += new Vector2(ab * (float)pi * 2 / 90 * (float)Math.Cos((float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                        center.X + (float)x1 - dcl.Position.X)), ab * (float)pi * 2 / 90 * (float)Math.Sin(
                            (float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                                center.X + (float)x1 - dcl.Position.X)));

                    Move(b, ab, center);
                    return;
                }
                Move(b, ab, center);



                if (x1 < 0)
                {


                    if (dcl.colliding)
                    {
                        isActivated = false;
                        return;
                    }
                    b -= 4f + (5 - ab / 30) + (vel2.X + vel2.Y) / 2 / 30;


                    if ((b < i - (1000 - ab / 60)) || dcl.colliding || currentMouseState.RightButton == ButtonState.Pressed &&
                        previousMouseState.RightButton == ButtonState.Released)
                    {

                        if (dcl.colliding)
                        {
                            isActivated = false;
                            return;
                        }
                        float angle = b;

                        double x1 = ab * Math.Cos(angle * pi / 180);

                        double y1 = ab * Math.Sin(angle * pi / 180);



                        vel2 += new Vector2(ab * (float)pi * 3f / 90 * (float)Math.Cos((float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                            center.X + (float)x1 - dcl.Position.X)), ab * (float)pi * 3f / 90 * (float)Math.Sin(
                                (float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                                    center.X + (float)x1 - dcl.Position.X)));
                        isActivated = false;

                    }
                }
                else if (x1 > 0)
                {

                    b += 4f + (5 - ab / 30) + (vel2.X + vel2.Y) / 2 / 30;
                    //if (b > i + 70)
                    if (dcl.colliding)
                    {
                        isActivated = false;
                        return;
                    }



                    if ((b > i + (1000 - ab / 60)) || dcl.colliding || currentMouseState.RightButton == ButtonState.Pressed &&
                previousMouseState.RightButton == ButtonState.Released)
                    {

                        {
                            if (dcl.colliding)
                            {
                                isActivated = false;
                                return;
                            }
                            float angle = b;

                            double x1 = ab * Math.Cos(angle * pi / 180);

                            double y1 = ab * Math.Sin(angle * pi / 180);



                            vel2 += new Vector2(ab * (float)pi * 4f / 90 * (float)Math.Cos((float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                                center.X + (float)x1 - dcl.Position.X)), ab * (float)pi * 4f / 90 * (float)Math.Sin(
                                    (float)Math.Atan2(center.Y + (float)y1 - dcl.Position.Y,
                                        center.X + (float)x1 - dcl.Position.X)));
                            isActivated = false;
                        }

                    }
                }

            }

        
            
            subpx /= 4;
            dcl.PreviousPosition = dcl.Position;
            prp = dcl.Position;
            dcl.Position = new Vector2(vel.X / 10 + vel2.X + dcl.Position.X + subpx, vel.Y / 10 + vel2.Y + dcl.Position.Y + subpy / 10);


            dcl.isActivated = isActivated;
            //Mouse.GetState().X
            dcl.Update();
        }
        public void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            spriteBatch.Draw(texture, start, null, Microsoft.Xna.Framework.Color.Brown,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)texture.Height),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }


    }

}