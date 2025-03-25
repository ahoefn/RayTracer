namespace RayTracer.TracerCode;
using Point = Vec3;
using static System.Math;
struct Color //u32 with blocks of 8 bytes corresponding to r, g, b and a respectively. 
// Comes with getter functions to convert bits to uints for each channel.
{//Note that if a this function receives an int over 255, 
 // it will store the int%255 as all the bits above 255 are lost.
    public Color(uint r_in, uint g_in, uint b_in, uint a_in)
    {
        //Note that the order in memory is reversed as uints are little-endian while RGBA8888 is big-endian
        rgba = a_in << 3 * 8 | (b_in << 3 * 8) >> 8 | (g_in << 3 * 8) >> 2 * 8 | (r_in << 3 * 8) >> 3 * 8;
    }
    //Data: 
    public uint rgba { get; set; }

    //Methods, used to obtain uint versions of rgba values, needs various bitmasks to get values.:
    public uint a { get { return rgba >> 3 * 8; } set { rgba = ((rgba << 8) >> 8) | (value << 3 * 8); } }
    public uint b { get { return (rgba << 8) >> 3 * 8; } set { rgba = (rgba << 2 * 8) >> 2 * 8 | (rgba >> 3 * 8) << 3 * 8 | (value << 3 * 8) >> 8; } }
    public uint g { get { return (rgba << 8 * 2) >> 3 * 8; } set { rgba = (rgba << 3 * 8) >> 3 * 8 | (rgba >> 2 * 8) << 2 * 8 | (value << 3 * 8) >> 2 * 8; } }
    public uint r { get { return (rgba << 8 * 3) >> 3 * 8; } set { rgba = (rgba >> 8) << 8 | (value << 3 * 8) >> 3 * 8; } }

    public static uint ToColor(float f)
    {//Makes sure that colors do not overflow
        if (f > 255) return 255;
        if (f < 0) return 0;
        return (uint)f;
    }
    public static Color operator *(float factor, Color color)
    {
        return new Color(ToColor(color.r * factor), ToColor(color.g * factor), ToColor(color.b * factor), ToColor(color.a * factor));
    }
    public static Color operator *(Color color, float factor)
    {
        return new Color(ToColor(color.r * factor), ToColor(color.g * factor), ToColor(color.b * factor), ToColor(color.a * factor));
    }
    public static Color operator *(Color color1, Color color2)
    {
        return new Color(ToColor(color1.r * color2.r / 255), ToColor(color1.g * color2.g / 255), ToColor(color1.b * color2.b / 255), ToColor(color1.a * color2.a / 255));
    }
    public static Color operator +(Color color1, Color color2)
    {
        return new Color(ToColor(color1.r + color2.r), ToColor(color1.g + color2.g), ToColor(color1.b + color2.b), ToColor(color1.a + color2.a));
    }
    public static Color MultiplyNotAlpha(float factor, Color color)
    {
        return new Color(ToColor(color.r * factor), ToColor(color.g * factor), ToColor(color.b * factor), color.a);
    }
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
        return new Vec3(v1.y * v2.z - v1.z * v2.y, v1.z * v1.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
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
    public static float Distancesqrd(Vec3 v1, Vec3 v2)
    {
        return Dot(v1 - v2, v1 - v2);
    }
    public static float Distance(Vec3 v1, Vec3 v2)
    {
        return (float)Sqrt(Vec3.Distancesqrd(v1, v2));
    }
    public static Color AsColor(Vec3 v)
    {//Takes in a normalized vector and outputs the associated color, with r=x, g=y and b=z
        return new Color((uint)(255 * v.x), (uint)(255 * v.y), (uint)(255 * v.z), 255);
    }
    public static Color AsColorUnnormalized(Vec3 v)
    {//Same as above but first normalizes
        return AsColor(Vec3.Normalize(v));
    }

    // public static bool Equals(Vec3 v1, Vec3 v2)
    // {
    //     return (v1.x == v2.x) & (v1.y == v2.y) & (v1.z == v2.z);
    // }


    // public override bool Equals(object v1)
    // {
    //     return (v1.x == x) & (v1.y == y) & (v1.z == z);
    // }
    // public static bool operator ==(Vec3 v1, Vec3 v2)
    // {
    //     return (v1.x == v2.x) & (v1.y == v2.y) & (v1.z == v2.z);
    // }
    // public static bool operator !=(Vec3 v1, Vec3 v2)
    // {
    //     return (v1.x != v2.x) | (v1.y != v2.y) | (v1.z != v2.z);
    // }


    //Methods, these allow us to work with either x,y,z or r,g,b to obtain the values;


}
struct Vec2
{
    public Vec2(float x_in, float y_in)
    {
        x = x_in; y = y_in;
    }
    //Data:
    public float x { get; set; }
    public float y { get; set; }

    // Make it possible to acces data also using v.r v.g and v.b to use Vec2 as colors:
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

    // Mathematical operations:
    public static Vec2 operator +(Vec2 v1, Vec2 v2)
    {
        return new Vec2(v1.x + v2.x, v1.y + v2.y);
    }
    public static Vec2 operator -(Vec2 v1, Vec2 v2)
    {
        return new Vec2(v1.x - v2.x, v1.y - v2.y);
    }
    public static Vec2 operator *(Vec2 v1, float c)
    {
        return new Vec2(v1.x * c, v1.y * c);
    }

    public static Vec2 operator *(float c, Vec2 v1)
    {
        return new Vec2(v1.x * c, v1.y * c);
    }

    public static Vec2 operator -(Vec2 v1)
    {
        return new Vec2(v1.x * (-1), v1.y * (-1));
    }
    public static Vec2 operator /(Vec2 v, float c)
    {
        return new Vec2(v.x / c, v.y / c);
    }
    public static float Dot(Vec2 v1, Vec2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }
    public static Vec2 Normalize(Vec2 v1)
    {
        return v1 / (float)Sqrt(Dot(v1, v1));
    }
    public static Vec2 operator *(Vec2 v1, uint c)
    {
        return new Vec2(v1.x * c, v1.y * c);
    }
    public static Vec2 operator *(uint c, Vec2 v1)
    {
        return new Vec2(v1.x * c, v1.y * c);
    }
    public static Vec2 operator /(Vec2 v, uint c)
    {
        return new Vec2(v.x / c, v.y / c);
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
