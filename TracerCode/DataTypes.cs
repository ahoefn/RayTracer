namespace RayTracer.TracerCode;
using Point = Vec3;
using static System.Math;
struct Color //u32 with blocks of 8 bytes corresponding to r, g, b and a respectively. 
// Comes with getter functions to convert bits to uints for each channel.
{//Note that if a this function receives an int over 255, 
 // it will store the int%255 as all the bits above 255 are lost.
    public Color(int r_in, int g_in, int b_in, int a_in)
    {
        //Note that the order in memory is reversed as uints are little-endian while RGBA8888 is big-endian
        rgba = (uint)a_in << 3 * 8 | ((uint)b_in << 3 * 8) >> 8 | ((uint)g_in << 3 * 8) >> 2 * 8 | ((uint)r_in << 3 * 8) >> 3 * 8;
    }
    //Data: 
    public uint rgba { get; set; }

    //Methods, used to obtain uint versions of rgba values, needs various bitmasks to get values.:
    public uint a { get { return rgba >> 3 * 8; } set { rgba = ((rgba << 8) >> 8) | (value << 3 * 8); } }
    public uint b { get { return (rgba << 8) >> 3 * 8; } set { rgba = (rgba << 2 * 8) >> 2 * 8 | (rgba >> 3 * 8) << 3 * 8 | (value << 3 * 8) >> 8; } }
    public uint g { get { return (rgba << 8 * 2) >> 3 * 8; } set { rgba = (rgba << 3 * 8) >> 3 * 8 | (rgba >> 2 * 8) << 2 * 8 | (value << 3 * 8) >> 2 * 8; } }
    public uint r { get { return (rgba << 8 * 3) >> 3 * 8; } set { rgba = (rgba >> 8) << 8 | (value << 3 * 8) >> 3 * 8; } }
}

struct Vec3
{
    public Vec3(float x_in, float y_in, float z_in)
    {
        x = x_in; y = y_in; z = z_in;
    }
    //Data:
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    // Make it possible to acces data also using v.r v.g and v.b to use Vec3 as colors:
    public float r
    {
        get { return x; }
        set { x = value; }
    }
    public float g
    {
        get { return y; }
        set { y = value; }
    }
    public float b
    {
        get { return z; }
        set { z = value; }
    }

    // Mathematical operations:
    public static Vec3 operator +(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }
    public static Vec3 operator -(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }
    public static Vec3 operator *(Vec3 v1, float c)
    {
        return new Vec3(v1.x * c, v1.y * c, v1.z * c);
    }

    public static Vec3 operator *(float c, Vec3 v1)
    {
        return new Vec3(v1.x * c, v1.y * c, v1.z * c);
    }

    public static Vec3 operator -(Vec3 v1)
    {
        return new Vec3(v1.x * (-1), v1.y * (-1), v1.z * (-1));
    }
    public static Vec3 operator /(Vec3 v, float c)
    {
        return new Vec3(v.x / c, v.y / c, v.z / c);
    }
    public static float Dot(Vec3 v1, Vec3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }
    public static Vec3 Cross(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }
    public static Vec3 Normalize(Vec3 v1)
    {
        return v1 / (float)Sqrt(Dot(v1, v1));
    }
    public static Vec3 operator *(Vec3 v1, uint c)
    {
        return new Vec3(v1.x * c, v1.y * c, v1.z * c);
    }
    public static Vec3 operator *(uint c, Vec3 v1)
    {
        return new Vec3(v1.x * c, v1.y * c, v1.z * c);
    }
    public static Vec3 operator /(Vec3 v, uint c)
    {
        return new Vec3(v.x / c, v.y / c, v.z / c);
    }



    //Methods, these allow us to work with either x,y,z or r,g,b to obtain the values;


}
struct Ray
{
    public Ray(Point base_in, Point head_in)
    {
        o = base_in; //o for Origin
        dir = Point.Normalize(head_in - base_in); //dir for Direction
    }

    //Data:
    public Point o { get; set; }
    public Point dir { get; set; }
    public Point head { get { return o + dir; } }

    // Methods:
    public Point at(float t)
    {
        return o + t * dir;
    }

}
