using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Elements;

public class KeyBitmapImage : KeyBitmapElement
{
    public Image Image { get; }

    public KeyBitmapImage(Image image)
    {
        Image = image;
    }

    public KeyBitmapImage(Image image, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center) : base(horizontalAlignment, verticalAlignment)
    {
        Image = image;
    }

    public Point CalculateLocation(int keySize)
    {
        var x = HorizontalAlignment switch
        {
            HorizontalAlignment.Left => Image.Width < keySize / 2 ? Padding : 0,
            HorizontalAlignment.Right => keySize - (Image.Width + Padding),
            _ => keySize / 2 - Image.Width / 2
        };
        var y = VerticalAlignment switch
        {
            VerticalAlignment.Top => Image.Height < keySize / 2 ? Padding : 0,
            VerticalAlignment.Bottom => keySize - (Image.Height + Padding),
            _ => keySize / 2 - Image.Height / 2
        };
        return new Point(x, y);
    }
}