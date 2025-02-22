namespace RayTracer.TracerCode;
using Point = Vec3;
using static System.Math;

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
class DebuggingInfo
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