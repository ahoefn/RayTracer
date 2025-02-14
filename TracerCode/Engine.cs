using System.Drawing;
using System;
using static System.Math;
namespace RayTracer.TracerCode;

using Point = Vec3;


class RayHolder : GeometricObject
{
    public RayHolder()
    {
        rayList = Array.Empty<Ray>();
    }
    public Ray[] rayList;

}

class ImageHolder : RayHolder
{
    public ImageHolder(uint width_in, uint height_in) : base()
    {
        height = height_in;
        width = width_in;
    }
    // Data:
    public uint height;
    public uint width;
    //Methods:
    public int pixelIndex(int x, int z)
    {//Returns the index i of the ray in rayList corresponding to the pixel at (ox,oy) in the plane coordinates.
        return x + (int)width * z;
    }

}
class Engine : ImageHolder
{
    public Engine(uint width_in, uint height_in, float? distance = (float).1) : base(width_in, height_in)
    {
        viewPort = new ViewPort(width, height);
        rayList = viewPort.rayList;
        sphere = new Sphere(new Point(0, 5, 0), 3);
    }
    // Data:
    public ViewPort viewPort;
    public Sphere sphere;
    //Methods:
    public Color RayColor(Ray ray)
    {
        if (sphere.DoesHit(ray)) { return sphere.OnHit(ray); }

        if (ray.dir.z > 0)
        {
            //Add some niceness to the sky
            Point pos = ray.at(viewPort.distance / ray.dir.y);
            float lerp = -(float)1.5 * ((pos.z - viewPort.upLeftCorner.z) / viewPort.pointDistance) * 255 / height;
            return new Color((uint)lerp, (uint)lerp, 255, 255);
        }
        return new Color(0, 255, 0, 255);
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


class ViewPort : ImageHolder
{
    public ViewPort(uint width_in, uint height_in, float distance_in = (float)1) : base(width_in, height_in)
    {
        distance = distance_in;
        viewPlane = new Plane(distance * yVec, distance * yVec);

        //We have a preferred basis on the plane for this particular plane
        pointDistance = (float)0.01;
        viewPlane.o1 = new Point(1, 0, 0) * pointDistance;
        viewPlane.o2 = new Point(0, 0, -1) * pointDistance;
        upLeftCorner = origin - width * viewPlane.o1 / 2 - height * viewPlane.o2 / 2 + distance * yVec;
        rayList = new Ray[width * height];

        for (int x_index = 0; x_index < width; x_index++)
        {
            for (int z_index = 0; z_index < height; z_index++)
            {
                // Traverses row by row through the picture from left up to right down,
                rayList[pixelIndex(x_index, z_index)] = new Ray(origin, upLeftCorner + x_index * viewPlane.o1 + z_index * viewPlane.o2);
            }
        }
    }
    //Data:
    public float distance;
    public float pointDistance;
    public Point upLeftCorner;
    public Plane viewPlane;

    //Methods:
    public (uint, uint) GetPixel(Ray ray)
    {//Obtain pixel by first intersecting the ray with the viewplane and then obtaining x,z components.
        float t = viewPlane.Intersect(ray);
        Point intersection = ray.at(t);
        float ox, oz; // coefficients on the plane 
        ox = Vec3.Dot(intersection - upLeftCorner, viewPlane.o1);
        oz = Vec3.Dot(intersection - upLeftCorner, viewPlane.o2);


        return ((uint)ox * width, (uint)oz * height);
    }
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