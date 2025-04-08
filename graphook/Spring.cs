using graphook;
using Microsoft.Xna.Framework;
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
        

    }

    public void ApplyForce(float force)
    {
        if (Math.Abs(force) < 30f) return; // Don't bother with tiny forces
        
        externalForce += force;
        Console.WriteLine($"Applying force to spring {numList}: {force}");

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
    public void Draw(SpriteBatch spriteBatch)
    {
        if (numList == 0) return;
        Rectangle destinationRectangle = new Rectangle(
            (int)startPos.X,
            (int)Position,
            6, 6
        );
        Rectangle destinationRectangle2 = new Rectangle(
            (int)startPos.X,
            (int)Position + 6,
            6, 6
        );
        Rectangle destinationRectangle3 = new Rectangle(
            (int)startPos.X,
            (int)Position + 12,
            6, 6
        );
        Rectangle destinationRectangle4 = new Rectangle(
            (int)startPos.X,
            (int)Position + 18,
            6, 180
        );

        float minY = 100f;
        float maxY = 300f;
        float depthFactor = MathHelper.Clamp((Position - 250 - 20) / (maxY - minY), 0f, 1f);
        
        Color waterColor2 = new Color(
            50,
            (int)(255 * (1 - depthFactor)),
            (int)(200 * (1 - 0.15f * depthFactor)),
            200
        );
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
