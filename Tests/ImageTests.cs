using RayTracer.TracerCode;
using Avalonia.Media.Imaging;
using Avalonia.Controls;
namespace RayTracer.Tests;

class InterfaceTests
{
    public static void RenderColorSquare(Image image, uint width, uint height)
    {
        var image_raw = new Color[width * height];
        Color base_color = new Color(50, 255, 0, 255);
        for (int i = 0; i < width * height; i++)
        {
            int x_pos = i % (int)width;
            int z_pos = (int)(i / (int)width);

            if (x_pos < width / 2 & z_pos < height / 2)
            {
                image_raw[i] = new Color(255, 0, 0, 255);
            }
            else if (x_pos >= width / 2 & z_pos < height / 2)
            {
                image_raw[i] = new Color(0, 255, 0, 255);
            }
            else if (x_pos < width / 2 & z_pos >= height / 2)
            {
                image_raw[i] = new Color(0, 0, 255, 255);
            }
            else
            {
                image_raw[i] = new Color(0, 0, 0, 255);
            }
        }
        image.Width = width;
        image.Height = height;
        image.Source = Tracer.Bitmap_Builder(image_raw, width, height);
    }

    public static void GradientUpDown(Image image, uint width, uint height)
    {
        var image_raw = new Color[width * height];
        var base_color = new Color(255, 0, 0, 255);
        for (int i = 0; i < width * height; i++)
        {
            double x_pos = (i % (int)width) / width;
            double z_pos = i / (int)width;
            // image_raw[i] = new Color((int)base_color.r * x_pos, base_color.g, base_color.r, base_color.a);
        }
        image.Width = width;
        image.Height = height;
        image.Source = Tracer.Bitmap_Builder(image_raw, width, height);

    }
}
