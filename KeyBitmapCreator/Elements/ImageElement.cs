using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Elements;

internal class ImageElement : BaseElement
{
    public Image Image { get; }

    public ImageElement(Image image, ElementLayoutOptions? layoutOptions = null) : base(layoutOptions)
    {
        Image = image;
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