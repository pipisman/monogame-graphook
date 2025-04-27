using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphook
{
    internal class Explosion
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int ptype;

        public Explosion(List<Texture2D> textures, Vector2 location, int type)
        {

            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            ptype = type;
        }

        public void Update()
        {


            int total = 10;

            for (int i = 0; i < total; i++)
            {


                particles.Add(GenerateNewParticle(random.Next(1, 360)));

            }


        }
        public void SpawnParticles()
        {

            int total = 1;

            for (int i = 0; i < total; i++)
            {


                particles.Add(GenerateNewParticle(random.Next(1, 360)));

            }
        }

        private Particle GenerateNewParticle(int ang)
        {

            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 4 - 2),
                                    1f * (float)(random.NextDouble() * 2 - 4));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(255, 255, 255);
            float size = 1;
            int ttl = 20 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);


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