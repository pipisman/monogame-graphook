using System.Collections.Generic;
using System.Net;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace editor;

public class Grid
{
    int initX;
    int initY;
    public List<Line> lines;
    public Grid(int initX, int initY, int wide, int height, int square, Texture2D texture)
    {
        lines = [];
        this.initX = initX;
        this.initY = initY;
        for (int h = 0; h < height; h++)
        {
            lines.Add(new Line(initX, initY + h * (square + 0), wide, square, texture));
        }
    }

    
}
