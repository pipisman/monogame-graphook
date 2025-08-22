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
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Data.Common;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Reflection;
namespace graphook
{
    internal class raycast
    {

        
        private List<Collision> collisions;
        private int distance;
        private int frequency;
        private Vector2 obj1;
        private Vector2 obj2;
        public raycast(List<Collision> Collisions, int distance, int frequency, Vector2 obj1, Vector2 obj2)
        {

            this.distance = distance;
            this.frequency = frequency;
            collisions = Collisions;
            this.obj1 = obj1;
            this.obj2 = obj2;
            
        }
        public (bool collided, Vector2 pos, float angle) Collidepoint()
        {
            for (int i = 0; i < distance / frequency; i++)
            {
                int b = i * frequency;
                float angle = (float)Math.Atan2(obj2.Y - obj1.Y, obj2.X - obj1.X);
                float x = obj1.X + b * (float)Math.Cos(angle);
                float y = obj1.Y + b * (float)Math.Sin(angle);
                Vector2 imaginaryPoint = new Vector2(x, y);
                for (int c = 0; c < collisions.Count; c++)
                {
                    if (imaginaryPoint.X > collisions[c]._c.X && imaginaryPoint.X < collisions[c]._d.X
                    && imaginaryPoint.Y > collisions[c]._c.Y && imaginaryPoint.Y < collisions[c]._a.Y)
                    {
                        return (true, imaginaryPoint, angle);
                    }
                }
            }
            return(false, new Vector2(0, 0), 0);
            //(float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
            //float x = start.X + distance * (float)Math.Cos(angle);
            // y = start.Y + distance * (float)Math.Sin(angle);
            //Vector2(x, y)
            
            
        }
        
        
        
    }
}
