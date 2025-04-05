using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphook
{
    public class ParticleSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int ptype;

        public ParticleSystem(List<Texture2D> textures, Vector2 location, int type)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            ptype = type;
        }

        public void Update()
        {
            if (ptype == 0)
            {
                int total = 10;

                for (int i = 0; i < total; i++)
                {


                    particles.Add(GenerateNewParticle());

                }
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
        public void SpawnParticles()
        {

            int total = 1;

            for (int i = 0; i < total; i++)
            {


                particles.Add(GenerateNewParticle());

            }
        }

        private Particle GenerateNewParticle()
        {
            if (ptype == 0)
            {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = EmitterLocation;
                Vector2 velocity = new Vector2(
                                        1f * (float)(random.NextDouble() * 4 - 2),
                                        1f * (float)(random.NextDouble() * 2 - 4));
                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                Color color = new Color(255, 255, 255);
                float size = (float)random.NextDouble();
                int ttl = 20 + random.Next(40);

                return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
            }
            if (ptype == 1) {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = EmitterLocation;
                Vector2 velocity = new Vector2(
                                        1f * (float)(random.NextDouble() * 0.5 - 0.25),
                                        1f * (float)(random.NextDouble() * 0.3));
                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                Color color = new Color(255, 255, 255);
                float size = (float)random.NextDouble();
                int ttl = 20 + random.Next(40);
                return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
            }
            return null;

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