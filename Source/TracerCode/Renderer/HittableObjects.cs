using static System.Math;
namespace RayTracer.TracerCode.Renderer;
using Point = Vec3;
using static Basis;

interface HittableObject
{
    public HitRecord GetHit(Ray ray);
}
struct Material
{
    public Material(Color color_in)
    {
        color = color_in;
    }
    public Color color;
}
struct HitRecord
{
    public HitRecord(float t_in, Material material_in, Vec3 normal_in)
    {
        t = t_in;
        material = material_in;
        normal = normal_in;
    }
    public Material material;
    public float t;
    public Vec3 normal;
}

class ShapeList : HittableObject
{
    public ShapeList(Shape[] shapes_in)
    {
        shapes = shapes_in;
    }

    public Shape[] shapes { get; set; }


    public HitRecord GetHit(Ray ray)
    {
        var record = new HitRecord(float.PositiveInfinity, new Material(new Color(0, 0, 0, 0)), new Vec3(0, 0, 0));
        float t_temp;
        for (int i = 0; i < shapes.Length; i++)
        {
            t_temp = shapes[i].Intersect(ray);
            if (t_temp < record.t)
            {
                record.t = t_temp;
                record.material = shapes[i].material;
                record.normal = shapes[i].SurfaceNormal(ray.at(t_temp));
            }
        }
        return record;
    }

}