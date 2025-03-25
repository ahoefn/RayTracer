using Avalonia;
using Avalonia.Platform;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using RayTracer.TracerCode;
using System;
using Avalonia.Layout;
using Avalonia.Media;
using Tmds.DBus.Protocol;
using System.Collections.Generic;
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

        modifyButtons = [];
        deleteButtons = [];
        RayTracer.Tests.InterfaceTests.RenderColorSquare(Rendered_Image, width, height);
    }
    //Data:
    public uint width;
    public uint height;
    public List<Button> deleteButtons;
    public List<Button> modifyButtons;

    //Methods:
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
    private void Add_Object(object sender, RoutedEventArgs args)
    {//Adds an object to the render engine and updates the UI to reflect this.

        int index = ObjectPanel.Children.Count;
        var stackPanel = new StackPanel();
        if (index != 0)
        {//Don't add border for first child.
            stackPanel.Children.Add(new Border() { BorderThickness = new Thickness(0, 0, 0, 2), HorizontalAlignment = HorizontalAlignment.Stretch });
        }

        //GridPanel setup:
        var gridPanelSub = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch };
        gridPanelSub.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
        gridPanelSub.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        gridPanelSub.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

        //Constructing the elements contained in gridPanelSub:
        var nameBlock = new TextBlock() { Text = "test" + index, VerticalAlignment = VerticalAlignment.Center };
        gridPanelSub.Children.Add(nameBlock);
        Grid.SetColumn(nameBlock, 0);

        Brush _secondaryBrush = GetBrushFromDict("SecondaryBrush");
        Brush _backgroundBrush = GetBrushFromDict("BackgroundBrush");
        var modifyButton = new Button()
        {
            Content = "m",
            Foreground = _secondaryBrush,
            Background = _backgroundBrush,
            HorizontalAlignment = HorizontalAlignment.Right,
            Tag = index
        };
        gridPanelSub.Children.Add(modifyButton);
        modifyButtons.Add(modifyButton);
        Grid.SetColumn(modifyButton, 1);

        var deleteButton = new Button()
        {
            Content = "x",
            Foreground = _secondaryBrush,
            Background = _backgroundBrush,
            HorizontalAlignment = HorizontalAlignment.Right,
            Tag = index
        };
        deleteButton.AddHandler(Button.ClickEvent, Delete_Object);
        gridPanelSub.Children.Add(deleteButton);
        deleteButtons.Add(deleteButton);
        Grid.SetColumn(deleteButton, 2);

        //Add everything to the ObjectPanel:
        stackPanel.Children.Add(gridPanelSub);
        ObjectPanel.Children.Add(stackPanel);
    }
    void Delete_Object(object sender, RoutedEventArgs args)
    {//Called by deleteButton contained in ObjectPanel
        var deleteButton = (Button)sender;
        if (deleteButton.Tag is not int) { throw new InvalidOperationException(); } //Should check which exception is fitting here.
        int index = (int)deleteButton.Tag;

        modifyButtons.RemoveAt(index);
        deleteButtons.RemoveAt(index);
        ObjectPanel.Children.RemoveAt(index);

        for (int i = index; i < ObjectPanel.Children.Count; i++)
        {
            modifyButtons[i].Tag = i;
            deleteButtons[i].Tag = i;
        }
    }
    Brush GetBrushFromDict(string key)
    {
        bool _brushFound = this.TryFindResource(key, out var _brush);
        if ((_brush is not Brush) || (!_brushFound)) { throw new KeyNotFoundException(); }
        return (Brush)_brush;
    }
}