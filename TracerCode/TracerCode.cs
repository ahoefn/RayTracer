using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
namespace RayTracer.TracerCode;

class Tracer //Interface class, main interface function is Create_Image
{
    public static WriteableBitmap Create_Image(uint width, uint height)
    {
        Color[] image_raw = new Color[width * height];
        Engine engine = new Engine(width, height);
        image_raw = engine.Render();
        return Bitmap_Builder(image_raw, width, height);

    }

    public static WriteableBitmap Bitmap_Builder(Color[] im, uint width, uint height)
    {
        WriteableBitmap output_image = new WriteableBitmap(new PixelSize((int)width, (int)height), new Vector(96, 96), PixelFormat.Rgba8888);

        //Makes sure that the framebuffer is automatically unlocked when we're done (something with IDisoposable?)
        using (var framebuffer = output_image.Lock())
        {
            unsafe
            {// Change pixels directly in memory
                uint* pixels = (uint*)framebuffer.Address;
                for (int i = 0; i < width * height; i++)
                {
                    pixels[i] = im[i].rgba;
                }
            }
        }
        output_image.Save("test.png");
        return output_image;
    }

}

