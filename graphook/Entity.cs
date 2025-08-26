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
        public List<float> angles = [];
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
            angles = [];

            float angleDeg = q; // rope direction in degrees
            int stepSize = 16;
            double x1 = triRad * Math.Cos(q * pi / 180);

            double y1 = triRad * Math.Sin(q * pi / 180);
            for (int k = 0; k < triRad / stepSize; k++)
            {

                double ghX1 = k * stepSize * Math.Cos(angleDeg * Math.PI / 180);
                double ghY1 = k * stepSize * Math.Sin(angleDeg * Math.PI / 180);

                float px = (float)(ghX1 + center.X);
                float py = (float)(ghY1 + center.Y);

                positions.Add(new Vector2(px, py));

                float dx = dcl.Position.X + 6.5f - px;
                float dy = dcl.Position.Y + 9f - py;
                //angles.Add((float)Math.Atan2(dy, dx) - 0.2f);
                angles.Add((float)Math.Atan2(y1 - ghY1, x1 - ghX1));
            }


            float xr = center.X + (float)x1;
            float yr = center.Y + (float)y1;
            Vector2 _a = new Vector2(xr, yr + 16);
            Vector2 _b = new Vector2(xr + 16, yr + 16);
            Vector2 _c = new Vector2(xr, yr);
            Vector2 _d = new Vector2(xr + 16, yr);
            //isActivated = false
            //return

            Rectangle a = new Rectangle((int)dcl.Position.X, (int)dcl.Position.Y, 16, 8);
            for (int i = 0; i < collisions.Count; i++)
            {
                Rectangle b = new Rectangle((int)collisions[i].Position.X, (int)collisions[i].Position.Y, collisions[i].Width, collisions[i].Height);
                Rectangle aint = Rectangle.Intersect(a, b);
                if (aint != Rectangle.Empty)
                {
                    isActivated = false;
                    return;
                }
            }
            prp = dcl.Position;
            MathHelper.Lerp(dcl.Position.X, center.X + (float)x1, 0.1f);
            //dcl.Position = new Vector2(center.X + (float)x1, center.Y + (float)y1);
            dcl.Position = new Vector2(MathHelper.Lerp(dcl.Position.X, center.X + (float)x1, 0.7f),
            MathHelper.Lerp(dcl.Position.Y, center.Y + (float)y1, 0.7f));
        }
        public void Update(int xoffset, int yoffset)
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
            if (currentState.IsKeyDown(Keys.R))
            {
                dcl.Position = new Vector2(200, 200);
            }
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
                vel.Y = -50;
                
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
                float worldMouseX = Mouse.GetState().X - xoffset;
                float worldMouseY = Mouse.GetState().Y - yoffset;
                
                float angle = (float)Math.Atan2(worldMouseY - dcl.Position.Y,
                                worldMouseX - dcl.Position.X);



                
                ray = new raycast(collisions, 175, 1, dcl.Position, new Vector2(worldMouseX, worldMouseY));


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




                float swingSpeed = 4f + (5 - ab / 30) + (vel2.X + vel2.Y) / 2 / 30;
                b += (x1 < 0 ? -swingSpeed : swingSpeed);

                bool release = dcl.colliding 
                    || currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released
                    || (x1 < 0 && b < i - (1000000 - ab / 60))
                    || (x1 > 0 && b > i + (1000000 - ab / 60));

                if (release)
                {
                    if (dcl.colliding)
                    {
                        isActivated = false;
                        return;
                    }

                    float angle = b;
                    double xPos = ab * Math.Cos(angle * pi / 180);
                    double yPos = ab * Math.Sin(angle * pi / 180);

                    float force = ab * (float)pi * 3.5f / 90;
                    vel2 += new Vector2(
                        force * (float)Math.Cos(Math.Atan2(center.Y + (float)yPos - dcl.Position.Y, center.X + (float)xPos - dcl.Position.X)),
                        force * (float)Math.Sin(Math.Atan2(center.Y + (float)yPos - dcl.Position.Y, center.X + (float)xPos - dcl.Position.X))
                    );

                    isActivated = false;
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