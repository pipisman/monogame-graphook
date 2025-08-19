using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphook
{
    public class fireSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<fireParticle> particles;
        private List<Texture2D> textures;
        double pi = 3.1415926535;
        Effect saturationEffect;
        double i, angle, x1, y1;

        public fireSystem(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<fireParticle>();
            random = new Random();
        }

        public void Update(int type)
        {

            particles.Add(GenerateNewParticle(1));
            particles.Add(GenerateNewParticle(1));
            for (i = 0; i < 8; i += 1)
            {
                particles.Add(GenerateNewParticle(type));

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

        private fireParticle GenerateNewParticle(int type)
        {
            if (type == 0)
            {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = new Vector2(EmitterLocation.X + random.Next(0, 17) - 8, EmitterLocation.Y);
                Vector2 velocity = new Vector2((float)random.NextDouble() * 1f - 0.75f, -(float)random.NextDouble() - 1);

                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(255, 255, 255);
                float size = (float)random.NextDouble();
                int ttl = random.Next(20, 43);

                return new fireParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, type);
            }
            else if (type == 1)
            {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = new Vector2(EmitterLocation.X + random.Next(0, 17) - 8, EmitterLocation.Y);
                Vector2 velocity = new Vector2((float)random.NextDouble() * 1.5f - 0.75f, -(float)random.NextDouble() - 1);

                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                int uhhy = random.Next(100, 150);
                Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(uhhy, uhhy, uhhy);
                float size = (float)random.NextDouble();
                int ttl = random.Next(30, 50);

                return new fireParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, type);
            }
            else
            {
                return null;
            }

        }

        public void Draw(SpriteBatch spriteBatch, int xoffset, int yoffset)
        {
            spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch, xoffset, yoffset);
            }
            spriteBatch.End();
        }
        
    }
}