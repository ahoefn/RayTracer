using Avalonia;
using Avalonia.Platform;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using RayTracer.TracerCode;
using System;
namespace RayTracer;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        DataContext = this;

        // Initialize widths and heights:
        width = 200;
        height = 200;
        width_box.Text = width.ToString();
        height_box.Text = height.ToString();

        RayTracer.Tests.InterfaceTests.RenderColorSquare(Rendered_Image, width, height);
    }
    public uint width;
    public uint height;

    private void Show_Love(object sender, RoutedEventArgs args)
    {
        Button_Text.Text = "I love you ";
        Render_Image(sender, args);
    }


    private void Render_Image(object sender, RoutedEventArgs args)
    {
        UInt32.TryParse(width_box.Text, out width);
        UInt32.TryParse(height_box.Text, out height);
        Rendered_Image.Width = width;
        Rendered_Image.Height = height;
        Rendered_Image.Source = Tracer.Create_Image(width, height);
    }
}