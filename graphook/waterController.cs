using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace graphook
{
    internal class waterController
    {
        private int amount;
        private Texture2D whiteTexture;
        List<Spring> water;
        int dd;
        public waterController(int springAmount, int springOffset, Texture2D texture)
        {
            this.amount = springAmount;
            this.whiteTexture = texture;
            water = new List<Spring>();
            for (int i = 0; i < springAmount; i++)
            {
                water.Add(new Spring(1f, 10f, 2f, new Vector2(-6 + i * springOffset, 200 + 150), null, i, whiteTexture));
            }
            for (int i = 0; i < water.Count(); i++)
            {
                water[i].water = water;
            }
        }
        public void Update(GameTime gameTime, KeyboardState newState, Random random)
        {
            float force = 0f;

            force = (float)random.NextDouble() * 3000;
            if (newState.IsKeyDown(Keys.Space))
                force = (float)random.NextDouble() * 4500;


            if (dd == 3)
            {
                water[random.Next(amount)].ApplyForce(force);
                dd = 0;
            }
            dd++;
            for (int i = 0; i < water.Count; i++)
            {
                water[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < water.Count; i++)
            {
                water[i].Draw(spriteBatch);
            }
        }

    }
}
