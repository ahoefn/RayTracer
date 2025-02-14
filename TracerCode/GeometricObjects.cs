
namespace RayTracer.TracerCode;
using Point = Vec3;
using static System.Math;

class GeometricObject
{
    public GeometricObject()
    {
        origin = new Point(0, 0, 0); //Will allow to move around later
        xVec = new Point(1, 0, 0);
        yVec = new Point(0, 1, 0);
        zVec = new Point(0, 0, 1);
    }
    public Point origin;
    public Point xVec;
    public Point yVec;
    public Point zVec;

}

abstract class HittableObject : GeometricObject
{
    abstract public float Intersect(Ray ray);
    abstract public bool DoesHit(Ray ray);

    abstract public Color OnHit(Ray ray);

}

class Plane : HittableObject
{//Plane defined from the span of the orthogonal vectors of p, shifted such that their basepoint is also at p.
    public Plane(Point p_in, Vec3 normal_in)
    {
        p = p_in;
        normal = normal_in; //Note that the normal vector does not need to be normalized
        (o1, o2) = VectorMath.OrthogonalVecs(normal);
    }
    //Data:
    public Vec3 p; //Defining point
    public Vec3 normal;
    public Vec3 o1; //Orthogonal vectors
    public Vec3 o2;

    //Methods:
    public override float Intersect(Ray ray)
    {//Returns t such that ray.o+t*ray.dir is in the plane, returns PositiveInfinity if such a t does not exist.
        if (!DoesHit(ray)) { return float.PositiveInfinity; }

        float denom = Vec3.Dot(normal, ray.dir);
        return Vec3.Dot(normal, p - ray.o) / denom;
    }

    public override bool DoesHit(Ray ray)
    {//Checks if a hit is possible by checking if the vector is parallel to the plane
        float angle = Vec3.Dot(normal, ray.dir);
        return !(angle == 0);
    }

    public override Color OnHit(Ray ray)
    {
        throw new System.NotImplementedException();
    }
}

class Sphere : HittableObject
{
    public Sphere(Point center_in, float radius_in)
    {
        center = center_in;
        radius = radius_in;
    }
    public Point center;
    public float radius;
    public override float Intersect(Ray ray)
    {//Not implemented
        if (!DoesHit(ray)) { return float.PositiveInfinity; }
        return 0;

    }
    public override bool DoesHit(Ray ray)
    {
        float a = Vec3.Dot(ray.dir, ray.dir);
        float h = Vec3.Dot(ray.dir, center - ray.o);
        float c = Vec3.Dot(center - ray.o, center - ray.o) - radius * radius;
        float discriminant = h * h - a * c;
        return discriminant >= 0;
    }

    public override Color OnHit(Ray ray)
    {//Some duplication of calculation here, could be optimized for sure
        float a = Vec3.Dot(ray.dir, ray.dir);
        float h = Vec3.Dot(ray.dir, center - ray.o);
        float c = Vec3.Dot(center - ray.o, center - ray.o) - radius * radius;
        float discriminant = h * h - a * c;

        float t;//Automatically take closest point for now.
        float discrimroot = (float)Sqrt(discriminant);
        if (discrimroot == 0) { t = h / a; }
        else if (discrimroot < h * h) { t = h / a - discrimroot; }
        else { t = h / a + discrimroot; }
        Point intsec = ray.at(t);
        return Vec3.AsColor(SurfaceNormal(intsec));
    }

    public Vec3 SurfaceNormal(Point point)
    {//Returns the normalized surface Normal along the line between the origin and point
        var ray = new Ray(center, point);
        return ray.dir;
    }
}

class VectorMath
{
    public static (Vec3, Vec3, Vec3) GramSchmidt(Vec3 v1, Vec3 v2, Vec3 v3)
    {//Gram-Schmidt procedure to obtain an orthonormal basis from three vectors (unused right now)
        Vec3 o1, o2, o3;
        o1 = Vec3.Normalize(v1);
        o2 = Vec3.Normalize(v1 - OrthogonalProj(v2, o1));
        o3 = Vec3.Normalize(v3 - OrthogonalProj(v3, o1) - OrthogonalProj(v3, o2));
        return (o1, o2, o3);
    }
    public static Vec3 OrthogonalProj(Vec3 v1, Vec3 v2)
    {//Orthogonal projection of v1 on v2
        if (Vec3.Dot(v2, v2) == 0)
        {
            return new Vec3(0, 0, 0);
        }
        return (Vec3.Dot(v1, v2) / Vec3.Dot(v2, v2)) * v2;
    }
    public static (Vec3, Vec3) OrthogonalVecs(Vec3 v)
    {//Gives a pair of normalized orthogonal vectors to v.
        if (v.x == 0 & v.z == 0)
        {
            return (new Vec3(1, 0, 0), new Vec3(0, 0, 1));
        }
        return (Vec3.Normalize(new Vec3(v.z, 0, -v.x)), Vec3.Normalize(new Vec3(v.x * v.y, -v.x * v.x - v.z * v.z, v.y * v.z)));
    }

}