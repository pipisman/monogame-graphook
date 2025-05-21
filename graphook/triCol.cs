using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Drawing;
using System.Reflection.Metadata;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
namespace graphook
{
    internal class triCol
    {

        public Vector2 DotA { get; set; }
        public Vector2 DotB { get; set; }
        public Vector2 DotC { get; set; }
        

        public triCol(Vector2 dot1, Vector2 dot2, Vector2 dot3)
        {
            DotA = dot1;
            DotB = dot2;
            DotC = dot3;

        }
        public void Update()
        {

        }
        private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(texture,
                             start,
                             null,
                             Microsoft.Xna.Framework.Color.Green,
                             angle,
                             Vector2.Zero, // Anchor at top-left
                             new Vector2(edge.Length(), 1f),
                             SpriteEffects.None,
                             0f);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });



            DrawLine(spriteBatch, whiteTexture, DotA, DotB);
            DrawLine(spriteBatch, whiteTexture, DotB, DotC);
            DrawLine(spriteBatch, whiteTexture, DotC, DotA);

        }
    }
}
