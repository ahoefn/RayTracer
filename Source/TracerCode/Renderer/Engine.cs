using System;
namespace RayTracer.TracerCode.Renderer;
using Point = Vec3;
using static Basis;

interface IRayHolder
{
    public Ray[] rayList { get; set; }

}

class ImageHolder
{
    public ImageHolder(uint width_in, uint height_in, ILogger log_in) : base()
    {
        height = height_in;
        width = width_in;
        log = log_in;
    }
    // Data:
    public uint height;
    public uint width;
    public ILogger log;
    //Methods:
    public int pixelIndex(int x, int z)
    {//Returns the index i of the ray in rayList corresponding to the pixel at (ox,oy) in the plane coordinates.
        return x + (int)width * z;
    }

}
class Engine : ImageHolder, IRayHolder
{
    public Engine(uint width_in, uint height_in, ILogger log_in, float? distance = (float).1) : base(width_in, height_in, log_in)
    {
        //Geometric defs
        viewPort = new ViewPort(width, height, log);
        rayList = viewPort.rayList;
        var sphere1 = new Sphere(new Point(0, 10, 5), 4, new Material(new Color(255, 0, 0, 255)), log);
        var sphere2 = new Sphere(new Point(0, 10, -5), 5, new Material(new Color(0, 100, 0, 255)), log);
        shapelist = new ShapeList([sphere1, sphere2]);
        light = new LightSource(new Vec3(1, 1, -1), log);
    }
    // Data:
    public ViewPort viewPort;
    public ShapeList shapelist;
    public LightSource light;
    public Ray[] rayList { get; set; }
    //Methods:
    public Color[] Render()
    {
        Color[] img = new Color[height * width];
        for (int i = 0; i < width * height; i++)
        {
            img[i] = CastRay(rayList[i]);
        }
        // img = Blurs.Linear(img, width, height, (float)0.6);
        return img;
    }
    public Color CastRay(Ray ray)
    {
        HitRecord record = shapelist.GetHit(ray);
        if (record.t != float.PositiveInfinity)
        {
            return light.LambertianRefl(record.normal, record.material);
        }

        if (ray.dir.z > 0)
        {
            //Add some niceness to the sky
            Point pos = ray.at(viewPort.distance / ray.dir.y);
            float lerp = -(float)1.5 * ((pos.z - viewPort.upLeftCorner.z) / viewPort.pointDistance) * 255 / height;
            return new Color((uint)lerp, (uint)lerp, 255, 255);
        }
        return new Color(0, 255, 0, 255);
    }
}


class ViewPort : ImageHolder
{
    public ViewPort(uint width_in, uint height_in, ILogger logger_in, float distance_in = (float)1) : base(width_in, height_in, logger_in)
    {
        distance = distance_in;
        viewPlane = new Plane(distance * yVec, distance * yVec, new Material(), logger_in);

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
    public Ray[] rayList { get; set; }

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

