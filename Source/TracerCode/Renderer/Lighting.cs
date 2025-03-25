namespace RayTracer.TracerCode.Renderer;
using Point = Vec3;
using static System.Math;

class LightSource
{
    public LightSource(Vec3 direction_in, ILogger log_in)
    {
        direction = Vec3.Normalize(direction_in);
        log = log_in;
    }
    public Vec3 direction;
    public ILogger log;

    public Color LambertianRefl(Vec3 normal, Material material)
    {
        float c = -Vec3.Dot(normal, direction);
        if (c < 0) { return new Color(0, 0, 0, 255); }
        return Color.MultiplyNotAlpha(c, material.color);
    }
}