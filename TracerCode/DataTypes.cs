namespace RayTracer.TracerCode;

struct Color //u32 with blocks of 8 bytes corresponding to r, g, b and a respectively. Comes with getter functions to convert bits to uints for each channel.
{
    public Color(int r_in, int g_in, int b_in, int a_in)
    {
        rgba = (uint)r_in << 3 * 8 | (uint)g_in << 2 * 8 | (uint)b_in << 8 | (uint)a_in;
    }
    //Data: 
    public uint rgba { get; set; }

    //Methods, used to obtain uint versions of rgba values:
    public uint r() { return rgba >> 3 * 8; }

    public uint g() { return (rgba << 8) >> 3 * 8; }
    public uint b() { return (rgba << 8 * 2) >> 3 * 8; }
    public uint a() { return (rgba << 8 * 3) >> 3 * 8; }
}
