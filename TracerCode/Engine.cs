using System.Drawing;
using System;
namespace RayTracer.TracerCode;

using Point = Vec3;

class RayHolder
{
    public RayHolder()
    {
        origin = new Point(0, 0, 0); //Will allow to move around later
        xVec = new Point(1, 0, 0);
        yVec = new Point(0, 1, 0);
        zVec = new Point(0, 0, 1);
        rayList = Array.Empty<Ray>();
    }
    public Point origin;
    public Point xVec;
    public Point yVec;
    public Point zVec;
    public Ray[] rayList;

}

class Engine : RayHolder
{
    public Engine(uint width_in, uint height_in, float? distance = (float).1) : base()
    {
        height = height_in;
        width = width_in;
        ViewPort viewPort = new ViewPort(width, height);
        rayList = new Ray[width * height];
        rayList = viewPort.rayList;
    }
    // Data:
    public uint height;
    public uint width;
    public Color RayColor(Ray ray)
    {
        // if (ray.dir.z > 0)
        // {
        //     return new Color(0, 0, 255, 255);
        // }
        // return new Color(0, 255, 0, 255);
        Vec3 v = ray.dir;
        return new Color((int)(255 * v.x), 0, 0, 255);
    }

    public Color[] Render()
    {
        Color[] img = new Color[height * width];
        for (int i = 0; i < width * height; i++)
        {
            img[i] = RayColor(rayList[i]);
        }
        return img;
    }
}


class ViewPort : RayHolder
{
    public ViewPort(uint width, uint height, float distance = (float).1) : base()
    {
        pointDistance = (float)0.01;
        lowLeftCorner = origin - pointDistance * width * xVec / 2 - pointDistance * height * zVec / 2 + distance * yVec;
        rayList = new Ray[width * height];

        int x_index, z_index;
        for (int i = 0; i < height * width; i++)
        {
            // Traverses row by row through the picture from low left to right up,
            x_index = i % (int)width;
            z_index = (int)(i / width);
            rayList[i] = new Ray(origin, lowLeftCorner + x_index * pointDistance * xVec + z_index * pointDistance * zVec);
        }
    }

    public float pointDistance;
    public Point lowLeftCorner;
}

class Utility
{
    public void Sizes()
    {
        unsafe
        {
            int raySize = sizeof(Ray);
            int Vec3Size = sizeof(Vec3);
            int ColorSize = sizeof(Color);

        }
    }
}

class HittableObject
{
    //Will only implement spheres for now
}