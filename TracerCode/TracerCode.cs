using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
namespace RayTracer.TracerCode;

class Tracer //Interface class, main interface function is Create_Image
{
    public static WriteableBitmap Create_Image(uint width, uint height)
    {
        Color base_color = new Color(255, 165, 0, 255);
        Color[] image_raw = new Color[width * height];
        for (int i = 0; i < width * height; i++)
        {
            image_raw[i] = base_color;
        }
        return Bitmap_Builder(image_raw, width, height);

    }

    private static WriteableBitmap Bitmap_Builder(Color[] im, uint width, uint height)
    {
        WriteableBitmap output_image = new WriteableBitmap(new PixelSize((int)width, (int)height), new Vector(96, 96), PixelFormat.Rgba8888);

        using (var framebuffer = output_image.Lock())  //Makes sure that the framebuffer is automatically unlocked when we're done (something with IDisoposable?)
        {
            unsafe
            {// Change pixels in memory one at a time, seems like we can only do it using the pointer in output_image.
                uint* pixels = (uint*)framebuffer.Address;
                int i;
                for (i = 0; i < width * height; i++)
                {
                    pixels[i] = im[i].rgba;
                }
            }
        }
        output_image.Save("test.png");
        return output_image;
    }

}

class Engine
{

}