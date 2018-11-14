using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay
{
    private readonly float convexMultiplier = 1e3f;

    public static Triangle2D Triangulate(params Vector2[] verticies)
    {
        var nVerticies = verticies.Length;
        Debug.Assert(nVerticies > 2, "Triangulate need at least 3 Points to generate Triangels!");

        if (nVerticies == 3)
        {
            return new Triangle2D(verticies[0], verticies[1], verticies[2]);
        }

        var trMax = nVerticies * 4;

        var minX = verticies[0].x;
        var minY = verticies[0].y;
        var maxX = minX;
        var maxY = minY;

        for (int i = 0; i < nVerticies; i++)
        {
            var vertex = verticies[i];

            if (vertex.x < minX) { minX = vertex.x; }
            if (vertex.y < minY) { minY = vertex.y; }
            if (vertex.x < maxX) { maxX = vertex.x; }
            if (vertex.y < maxY) { maxY = vertex.y; }
        }

        return new Triangle2D();


    }
}

public struct Triangle2D
{
    Vector2 p1, p2, p3;
    Edge2D e1, e2, e3;

    public Triangle2D(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;

        e1 = new Edge2D(p1, p2);
        e2 = new Edge2D(p2, p3);
        e3 = new Edge2D(p3, p1);
    }

    private float CrossProduct(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        var x1 = p2.x - p1.x;
        var x2 = p3.x - p2.x;
        var y1 = p2.y - p1.y;
        var y2 = p3.y - p2.y;

        return x1 * y2 - y1 * x2;
    }

    private float QuadCross(float a, float b, float c)
    {
        var p = (a + b + c) * (a + b - c) * (a - b + c) * (-a + b + c);

        return Mathf.Sqrt(p);
    }

    public bool IsCW()
    {
        return CrossProduct(p1, p2, p3) < 0;
    }

    public bool IsCCW()
    {
        return CrossProduct(p1, p2, p3) > 0;
    }

    private Tuple<float, float, float> GetSideLength()
    {
        return new Tuple<float, float, float>(e1.Length, e2.Length, e3.Length);
    }

    public Vector2 Center()
    {
        var x = (p1.x + p2.x + p3.x) / 3;
        var y = (p1.y + p2.y + p3.y) / 3;

        return new Vector2(x, y);
    }

    public Circle2D CircumCircle()
    {
        var p = CircumCenter();
        var r = CircumRadius();

        return new Circle2D(p, r);
    }

    private Vector2 CircumCenter()
    {
        var p1 = this.p1;
        var p2 = this.p2;
        var p3 = this.p3;

        var D = (p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) * 2;

        var x = ((p1.x * p1.x + p1.y * p1.y) * (p2.y - p3.y) + (p2.x * p2.x + p2.y * p2.y) * (p3.y - p1.y) + (p3.x * p3.x + p3.y * p3.y) * (p1.y - p2.y));
        var y = ((p1.x * p1.x + p1.y * p1.y) * (p3.x - p2.x) + (p2.x * p2.x + p2.y * p2.y) * (p1.x - p3.x) + (p3.x * p3.x + p3.y * p3.y) * (p2.x - p1.x));

        return new Vector2(x / D, y / D);
    }

    private float CircumRadius()
    {
        var a = GetSideLength().Item1;
        var b = GetSideLength().Item2;
        var c = GetSideLength().Item3;

        return (a * b * c) / QuadCross(a, b, c);
    }

    public float Area()
    {
        var a = GetSideLength().Item1;
        var b = GetSideLength().Item2;
        var c = GetSideLength().Item3;

        return (QuadCross(a, b, c) / 4);
    }

    public bool InCircumCircle(Vector2 p)
    {
        Circle2D circle = CircumCircle();
        var dx = (circle.Position.x - p.x);
        var dy = (circle.Position.y - p.y);

        return dx * dx + dy * dy <= circle.Radius * circle.Radius;
    }
}

public struct Edge2D
{
    public Vector2 Point1 { get; private set; }
    public Vector2 Point2 { get; private set; }
    public float Length { get; private set; }

    public Edge2D(Vector2 point1, Vector2 point2)
    {
        this.Point1 = point1;
        this.Point2 = point2;

        Length = Vector2.Distance(point1, point2);
    }
}

public struct Circle2D
{
    public Vector2 Position { get; private set; }
    public float Radius { get; private set; }

    public Circle2D(Vector2 position, float radius)
    {
        this.Position = position;
        this.Radius = radius;
    }
}
