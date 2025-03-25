using RayTracer.TracerCode;
using Avalonia.Controls;
namespace RayTracer.Tests;

class InterfaceTests
{
    public static void RenderColorSquare(Image image, uint width, uint height)
    {
        var image_raw = new Color[width * height];
        Color base_color = new Color(50, 255, 0, 255);
        int i;
        for (int x_index = 0; x_index < width; x_index++)
        {
            for (int z_index = 0; z_index < height; z_index++)
            {
                i = x_index + (int)width * z_index;

                //Divide into quadrants and color each quadrant differently
                if (x_index < width / 2 & z_index < height / 2)
                {
                    image_raw[i] = new Color(255, 0, 0, 255);
                }
                else if (x_index >= width / 2 & z_index < height / 2)
                {
                    image_raw[i] = new Color(0, 255, 0, 255);
                }
                else if (x_index < width / 2 & z_index >= height / 2)
                {
                    image_raw[i] = new Color(0, 0, 255, 255);
                }
                else
                {
                    image_raw[i] = new Color(0, 0, 0, 255);
                }
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
        var pixel_color = base_color;


        int i;
        for (int x_index = 0; x_index < width; x_index++)
        {
            for (int z_index = 0; z_index < height; z_index++)
            {
                i = x_index + (int)width * z_index;

                pixel_color.r = (uint)(base_color.r * z_index);
                image_raw[i] = pixel_color;
            }
        }
        image.Width = width;
        image.Height = height;
        image.Source = Tracer.Bitmap_Builder(image_raw, width, height);

    }
    public static void GradientLeftRight(Image image, uint width, uint height)
    {
        var image_raw = new Color[width * height];
        var base_color = new Color(255, 0, 0, 255);
        var pixel_color = base_color;

        double x_pos;
        for (int i = 0; i < width * height; i++)
        {
            x_pos = (double)(i % width) / width;
            pixel_color.r = (uint)(base_color.r * x_pos);
            image_raw[i] = pixel_color;
        }
        image.Width = width;
        image.Height = height;
        image.Source = Tracer.Bitmap_Builder(image_raw, width, height);

    }
}
