using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphook
{
    public class ParticleSystem_sin
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle_sin> particles;
        private List<Texture2D> textures;
        double pi = 3.1415926535;
        Effect saturationEffect;
        double i, angle, x1, y1;

        public ParticleSystem_sin(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle_sin>();
            random = new Random();
        }

        public void Update(int type)
        {
            int radius;
            
            for (i = 0; i < 360; i += 6)
            {

                angle = i;
                radius = 20;
                if (type == 1) radius = 50;
                    
                x1 = radius * Math.Cos(angle * pi / 180);

                y1 = radius * Math.Sin(angle * pi / 180);
                
                particles.Add(GenerateNewParticle((float)x1, (float)y1, type));

            }

            

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle_sin GenerateNewParticle(float x, float y, int type)
        {
            if (type == 0)
            {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = new Vector2(x + EmitterLocation.X, y + EmitterLocation.Y);
                Vector2 velocity = new Vector2(random.Next(0, 2) == 0 ? -1 : 1, 0);

                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(255, 255, 255);
                float size = (float)random.NextDouble();
                int ttl = 40;

                return new Particle_sin(texture, position, velocity, angle, angularVelocity, color, size, ttl);
            }
            else if (type == 1) {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = new Vector2(x + EmitterLocation.X, y + EmitterLocation.Y);
                float pAngle = (float)Math.Atan2(EmitterLocation.Y - position.Y, EmitterLocation.X - position.X);
                //Vector2 velocity = new Vector2((float)random.NextDouble() * 2 - 1 + 2 * (float)Math.Cos(pAngle), (float)random.NextDouble() * 2 - 1 + 2 * (float)Math.Sin(pAngle)); ;
                Vector2 velocity = new Vector2(2 * (float)Math.Cos(pAngle), 2 * (float)Math.Sin(pAngle)); ;
                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(255, 255, 255);
                float size = (float)random.NextDouble();
                int ttl = 120;

                return new Particle_sin(texture, position, velocity, angle, angularVelocity, color, size, ttl);
            }
            else
            {
                return null;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}