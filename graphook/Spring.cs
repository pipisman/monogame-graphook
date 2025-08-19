using graphook;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

internal class Spring
{
    
    public float Position { get; private set; } // Y position
    public float Velocity { get; private set; }
    Texture2D whiteTexture;
    private float target;
    private float mass;
    private float stiffness;
    private float damping;
    private Vector2 startPos;
    private float externalForce;
    float hue;
    public static Texture2D PixelTexture;
    public List<Spring> water;
    private int numList;
    float minY = 100f; // highest/least deep
    float maxY = 300f; // lowest/deepest
    public Spring(float mass, float stiffness, float damping, Vector2 startPos, List<Spring> water, int numlist, Texture2D whiteTexture)
    {
        this.mass = mass;
        this.stiffness = stiffness;
        this.damping = damping;
        this.startPos = startPos;
        this.Position = startPos.Y; 
        this.Velocity = 0f;
        this.target = startPos.Y;  
        this.externalForce = 0f;
        this.water = water;
        this.numList = numlist;
        this.whiteTexture = whiteTexture;
        hue = numList / 10;

    }

    public void ApplyForce(float force)
    {
        if (Math.Abs(force) < 30f) return; // Don't bother with tiny forces
        
        externalForce += force;
        

        if (numList > 0)
        {
            water[numList - 1].Chain(force * 0.95f, 0);
        }

        if (numList < water.Count - 1)
        {
            water[numList + 1].Chain(force * 0.95f, 1);
        }
    }
    public void Chain(float force, int direction)
    {
        if (Math.Abs(force) < 30f) return; 

        externalForce += force;


        if (numList > 0 && direction == 0)
        {
            water[numList - 1].Chain(force * 0.9f, direction);
        }

        if (numList < water.Count - 1 && direction == 1)
        {
            water[numList + 1].Chain(force * 0.9f, direction);
        }
        
        if (numList == 0)
        {
            water[numList + 1].Chain(force * 0.9f, 1);
        }
        if (numList == water.Count())
        {
            water[numList + 1].Chain(force * 0.9f, 0);
        }
        
    }




    private bool isForceApplied = false;

    public void SetTarget(float newTarget)
    {
        target = newTarget;
        
    }
    public void Update(float deltaTime)
    {
        if (Math.Abs(Velocity) < 0.001f)
            Velocity = 0f;
        float springForce = -stiffness * (Position - target);
        float dampingForce = -damping * Velocity;
        float netForce = springForce + dampingForce + externalForce;

        float acceleration = netForce / mass;

        Velocity += acceleration * deltaTime;
        Position += Velocity * deltaTime;
        hue += (float)deltaTime * 200f;
            
            if (hue >= 360f) hue -= 360f;
        externalForce = 0f; 
    }


    private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
    {
        spriteBatch.Draw(texture, start, null, Microsoft.Xna.Framework.Color.Green,
                         (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                         new Vector2(0f, (float)texture.Height / 2),
                         new Vector2(Vector2.Distance(start, end), 1f),
                         SpriteEffects.None, 0f);
    }
    private Color ColorFromHSV(float hue, float saturation, float value)
    {
        int hi = (int)(hue / 60) % 6;
        float f = hue / 60 - (int)(hue / 60);

        float v = value * 255;
        float p = v * (1 - saturation);
        float q = v * (1 - f * saturation);
        float t = v * (1 - (1 - f) * saturation);

        return hi switch
        {
            0 => new Color(v / 255f, t / 255f, p / 255f),
            1 => new Color(q / 255f, v / 255f, p / 255f),
            2 => new Color(p / 255f, v / 255f, t / 255f),
            3 => new Color(p / 255f, q / 255f, v / 255f),
            4 => new Color(t / 255f, p / 255f, v / 255f),
            _ => new Color(v / 255f, p / 255f, q / 255f),
        };
    }
    public void Draw(SpriteBatch spriteBatch, int xoffset, int yoffset)
    {
        if (numList == 0) return;
        Rectangle destinationRectangle = new Rectangle(
            (int)startPos.X + xoffset,
            (int)Position + yoffset,
            6, 6
        );
        Rectangle destinationRectangle2 = new Rectangle(
            (int)startPos.X + xoffset,
            (int)Position + 6 + yoffset,
            6, 6
        );
        Rectangle destinationRectangle3 = new Rectangle(
            (int)startPos.X + xoffset,
            (int)Position + 12 + yoffset,
            6, 6
        );
        Rectangle destinationRectangle4 = new Rectangle(
            (int)startPos.X + xoffset,
            (int)Position + 18 + yoffset,
            6, 180
        );

        float minY = 100f;
        float maxY = 300f;
        float depthFactor = MathHelper.Clamp((Position - 250 + 20) / (maxY - minY), 0f, 1f);
        
        Color waterColor2 = new Color(
            50,
            (int)(255 * (1 - depthFactor)),
            (int)(200 * (1 - 0.15f * depthFactor)),
            170
        );
        
        //le funny
        Color rainbowColor = ColorFromHSV(hue, 1f, 1f);
        //Color waterColor2 = new Color(
        //    rainbowColor.R,
        //    (int)(rainbowColor.G * (1 - depthFactor)),
        //    (int)(rainbowColor.B * (1 - 0.15f * depthFactor)),
        //   170
        //);
        Color waterColor = new Color(
            50,
            (int)(255 * (1 - depthFactor)),
            (int)(255 * (1 - 0.15f * depthFactor)),
            1
        );

        
        spriteBatch.Draw(whiteTexture, destinationRectangle, waterColor);
        spriteBatch.Draw(whiteTexture, destinationRectangle, waterColor);
        spriteBatch.Draw(whiteTexture, destinationRectangle, waterColor);
        spriteBatch.Draw(whiteTexture, destinationRectangle2, waterColor2);
        spriteBatch.Draw(whiteTexture, destinationRectangle2, waterColor2);
        spriteBatch.Draw(whiteTexture, destinationRectangle2, waterColor2);
        spriteBatch.Draw(whiteTexture, destinationRectangle3, waterColor);
        spriteBatch.Draw(whiteTexture, destinationRectangle3, waterColor);
        spriteBatch.Draw(whiteTexture, destinationRectangle3, waterColor);
        spriteBatch.Draw(whiteTexture, destinationRectangle4, waterColor2);
    }

}
