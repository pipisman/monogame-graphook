using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphook
{
    public class bhcontroller
    {
        float initRadius = 40;
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<bh_particle> particles;
        private List<Texture2D> textures;
        double pi = 3.1415926535;
        Effect saturationEffect;
        double i, angle, x1, y1;

        public bhcontroller(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<bh_particle>();
            random = new Random();
        }

        public void Update(int type)
        {
            
            particles.Add(GenerateNewParticle());

            

            

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].initRadius <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private bh_particle GenerateNewParticle()
        {
            
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = new Vector2(EmitterLocation.X, EmitterLocation.Y);
            Vector2 velocity = new Vector2(0, 0);

            float angle = 0;
            float angularVelocity = 0;
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(0, 0, 0);
            float size = (float)random.NextDouble() * 0.7f;
            int ttl = 40;

            return new bh_particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, initRadius);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            
        }
    }
}