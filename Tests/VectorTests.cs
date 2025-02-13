using RayTracer.TracerCode;
using System;
namespace RayTracer.Tests;
class MathTests
{
    public MathTests()
    {
        bool[] results = new bool[10];
        results[0] = vectorEquality((new Vec3(1, 0, 0) + new Vec3(0, 1, 0)), new Vec3(1, 1, 0));
        results[1] = vectorEquality((new Vec3(1, 0, 0) - new Vec3(0, 1, 0)), new Vec3(1, -1, 0));
        results[2] = vectorEquality((new Vec3((float)1.4, (float)0.3, (float)0.2)) + new Vec3(-3, 1, 0), new Vec3((float)(-1.6), (float)1.3, (float)0.2));
        results[3] = vectorEquality(3 * (new Vec3(1, (float)4.5, 0)), new Vec3(3, (float)13.5, 0));
        results[4] = vectorEquality((new Vec3(1, (float)4.5, 0)) * 3, new Vec3(3, (float)13.5, 0));
        results[5] = vectorEquality(new Vec3(1, 3, 0) / 2, new Vec3((float)1 / 2, (float)3 / 2, 0));
        results[6] = vectorEquality(new Vec3(1, 3, 0) / (float)0.2, new Vec3((float)1 * 5, (float)3 * 5, 0));
    }

    public bool vectorEquality(Vec3 v1, Vec3 v2)
    {
        if (v1.x == v2.x & v1.y == v2.y & v1.z == v2.z) { return true; }
        return false;
    }
}