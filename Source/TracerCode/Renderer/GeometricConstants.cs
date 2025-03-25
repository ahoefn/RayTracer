namespace RayTracer.TracerCode.Renderer;
using Point = Vec3;
static class Basis
{
    public static Point origin { get => new Point(0, 0, 0); }
    public static Point xVec { get => new Point(1, 0, 0); }
    public static Point yVec { get => new Point(0, 1, 0); }
    public static Point zVec { get => new Point(0, 0, 1); }
}