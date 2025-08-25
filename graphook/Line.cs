using System.Collections.Generic;
using System.Data;
using editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Line
{
    int x;
    int y;
    public List<Row> rows;
    private int initX;
    private int v;
    private int wide;
    private int square;

    public Line(int x, int y, int width, int square, Texture2D texture)
    {
        rows = [];
        this.x = x;
        this.y = y;
        for (int w = 0; w < width; w++)
        {
            rows.Add(new Row(x + w * (square + 0), y, texture));
        }
    }
}