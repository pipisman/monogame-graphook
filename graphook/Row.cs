using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Schema;
using editor;
using graphook;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Row
{
    SpriteFont font1;
    Vector2 fontPos;
    public int x;
    KeyboardState previousState;
    KeyboardState currentState;
    public int y;
    public bool selected;
    int mouseX;
    int mouseY;
    public bool placed;
    Texture2D texture;
    Collision myCollision;
    int texturesize = 16;
    public Row(int x, int y, Texture2D texture)
    {
        this.x = x;
        this.y = y;
        this.texture = texture;
    }
    public void Update(int mouseX, int mouseY, int xoffset, int yoffset, List<Collision> collisions, KeyboardState keyboardState)
    {
        previousState = currentState;
        currentState = keyboardState;
    
        int adjustedMouseX = mouseX - xoffset;
        int adjustedMouseY = mouseY - yoffset;
    
        if (adjustedMouseX > x && adjustedMouseX < x + texturesize &&
            adjustedMouseY > y && adjustedMouseY < y + texturesize)
        {
            selected = true;
    
            if (!previousState.IsKeyDown(Keys.F) && currentState.IsKeyDown(Keys.F))
            {
                if (!placed)
                {
                    myCollision = new Collision(new Vector2(x, y), texturesize, texturesize, texture);
                    collisions.Add(myCollision);
                    placed = true;
                }
                else
                {
                    collisions.Remove(myCollision);
                    placed = false;
                }
            }
        }
        else
        {
            selected = false;
        }
    
        if (currentState.IsKeyDown(Keys.G) && placed)
        {
            collisions.Remove(myCollision);
            placed = false;
        }
    }

}