using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Elements;

internal class ImageElement : BaseElement
{
    public Image Image { get; }
    public ImageElementOptions ImageOptions { get; }

    public ImageElement(Image image, ImageElementOptions? imageOptions = null, ElementLayoutOptions? layoutOptions = null) : base(layoutOptions)
    {
        Image = image;
        ImageOptions = imageOptions ?? new ImageElementOptions();
    }

    public Point CalculateLocation(int keySize)
    {
        var x = LayoutOptions.HorizontalAlignment switch
        {
            HorizontalAlignment.Left => Image.Width < keySize - LayoutOptions.PaddingLeft ? LayoutOptions.PaddingLeft : 0,
            HorizontalAlignment.Right => keySize - (Image.Width + LayoutOptions.PaddingRight),
            _ => keySize / 2 - Image.Width / 2
        };
        var y = LayoutOptions.VerticalAlignment switch
        {
            VerticalAlignment.Top => Image.Height < keySize - LayoutOptions.PaddingTop ? LayoutOptions.PaddingTop : 0,
            VerticalAlignment.Bottom => keySize - (Image.Height + LayoutOptions.PaddingBottom),
            _ => keySize / 2 - Image.Height / 2
        };
        return new Point(x, y);
    }
}

public class ImageElementOptions
{
    public static ImageElementOptions FillKey()
    {
        return new ImageElementOptions() { SizeMode = ImageSizeMode.FillKey };
    }

    public static ImageElementOptions Create(ImageSizeMode sizeMode)
    {
        return new ImageElementOptions() { SizeMode = sizeMode };
    }

    public ImageSizeMode SizeMode { get; set; } = ImageSizeMode.None;
}