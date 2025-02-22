using static System.Math;
namespace RayTracer.TracerCode;
using Point = Vec3;
using static Basis;
abstract class Shape
{
    public Shape(Point point_in, Material material_in, ILogger logger_in)
    {
        location = point_in;
        material = material_in;
        log = logger_in;
    }
    //Data:
    public Point location;
    public Material material;
    public ILogger log;

    //Methods:
    abstract public float Intersect(Ray ray);
    abstract public bool DoesHit(Ray ray);
    abstract public Color OnHit(Ray ray, float t, LightSource light);
    abstract public Vec3 SurfaceNormal(Point point);
}

class Plane : Shape
{//Plane defined from the span of the orthogonal vectors of p, shifted such that their basepoint is also at p.
    public Plane(Point location_in, Vec3 normal_in, Material material_in, ILogger logger_in) : base(location_in, material_in, logger_in)
    {
        log = logger_in;
        normal = normal_in; //Note that the normal vector does not need to be normalized
        (o1, o2) = VectorMath.OrthogonalVecs(normal);
    }
    //Data:
    public Vec3 normal;
    public Vec3 o1; //Orthogonal vectors
    public Vec3 o2;

    //Methods:
    public override float Intersect(Ray ray)
    {//Returns t such that ray.o+t*ray.dir is in the plane, returns PositiveInfinity if such a t does not exist.
        if (!DoesHit(ray)) { return float.PositiveInfinity; }

        float denom = Vec3.Dot(normal, ray.dir);
        return Vec3.Dot(normal, location - ray.o) / denom;
    }

    public override bool DoesHit(Ray ray)
    {//Checks if a hit is possible by checking if the vector is parallel to the plane
        float angle = Vec3.Dot(normal, ray.dir);
        return !(angle == 0);
    }

    public override Color OnHit(Ray ray, float t, LightSource light)
    {
        throw new System.NotImplementedException();
    }
    public override Point SurfaceNormal(Point point)
    {
        return normal;
    }

}

class Sphere : Shape
{
    public Sphere(Point location_in, float radius_in, Material material_in, ILogger logger_in) : base(location_in, material_in, logger_in)
    {
        radius = radius_in;
    }
    public float radius;
    public override float Intersect(Ray ray)
    {//Returns t for closest hit from ray to origin.
        if (!DoesHit(ray)) { return float.PositiveInfinity; } //Some calculations are duplicated here, can be optimized
        float a = Vec3.Dot(ray.dir, ray.dir);
        float h = Vec3.Dot(ray.dir, location - ray.o);
        float c = Vec3.Dot(location - ray.o, location - ray.o) - radius * radius;
        float discriminant = h * h - a * c;

        //Make sure we return the smallest t s.t. t>0.
        float t;
        float discrimroot = (float)Sqrt(discriminant);
        if (discrimroot < h * h) { t = h / a - discrimroot; }
        else { t = h / a + discrimroot; }

        return t;

    }
    public override bool DoesHit(Ray ray)
    {
        float a = Vec3.Dot(ray.dir, ray.dir);
        float h = Vec3.Dot(ray.dir, location - ray.o);
        float c = Vec3.Dot(location - ray.o, location - ray.o) - radius * radius;
        float discriminant = h * h - a * c;
        return discriminant >= 0;
    }

    public override Color OnHit(Ray ray, float t, LightSource light)
    {//Some duplication of calculation here, could be optimized for sure
        Point intsec = ray.at(t);
        return light.LambertianRefl(SurfaceNormal(intsec), material);
    }

    public override Vec3 SurfaceNormal(Point point)
    {//Returns the normalized surface Normal along the line between the origin and point
        return (point - location) / radius;
    }

}
