using KeyBitmapCreator.Elements;
using KeyBitmapCreator.Helper;
using SixLabors.ImageSharp;

namespace KeyBitmapCreator;

public class KeyBitmapSpec
{
    private Color? _foregroundColor;

    /// <summary>
    /// Foreground color used if not specified for a individual element
    /// </summary>
    public Color ForegroundColor
    {
        get => _foregroundColor ?? ColorHelpers.GetForegroundColor(BackgroundColor);
        set => _foregroundColor = value;
    }

    /// <summary>
    /// Key background fill - a key is black when off
    /// </summary>
    public Color BackgroundColor { get; set; } = Color.Black;

    public List<KeyBitmapText> TextElements { get; set; } = new();

    public List<KeyBitmapImage> ImageElements { get; set; } = new();
}