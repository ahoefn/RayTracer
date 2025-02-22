using System;
using static System.Math;
namespace RayTracer.TracerCode;

using Point = Vec3;

class ImageProcessor
{
    public static Func<int, int, int> CreatePixelindexFunc(uint width, uint height)
    {
        int PixelIndex(int x, int z)
        {//int Makes sure x and z are within bounds, then returns the index
            int _x, _z;
            if (x < 0) _x = 0;
            else if (x > width - 1) _x = (int)width - 1;
            else _x = x;

            if (z < 0) _z = 0;
            else if (z > height - 1) _z = (int)height - 1;
            else _z = z;

            return _x + (int)width * _z;
        }

        return PixelIndex;
    }
}
class Blurs : ImageProcessor
{
    public static Color[] Linear(Color[] img_in, uint width, uint height, float blur_strength)
    {//Blurs a pixel by taking a weighted average in the square around it, the average is taken such that the original pixel is scaled by 1-blur_strength
        var img_out = new Color[width * height];
        Func<int, int, int> PixelIndex = CreatePixelindexFunc(width, height);
        //Define the masking parameters:
        float self = 1 - blur_strength;
        float diag = blur_strength / (4 * (1 + (float)Sqrt(2)));
        float straight = diag * (float)Sqrt(2);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                img_out[PixelIndex(x, z)] = self * img_in[PixelIndex(x, z)]
                                            + straight * img_in[PixelIndex(x - 1, z)] + straight * img_in[PixelIndex(x + 1, z)]
                                            + straight * img_in[PixelIndex(x, z - 1)] + straight * img_in[PixelIndex(x, z + 1)]
                                            + diag * img_in[PixelIndex(x - 1, z - 1)] + diag * img_in[PixelIndex(x + 1, z - 1)]
                                            + diag * img_in[PixelIndex(x + 1, z - 1)] + diag * img_in[PixelIndex(x + 1, z + 1)];

            }
        }

        return img_out;
    }
}
