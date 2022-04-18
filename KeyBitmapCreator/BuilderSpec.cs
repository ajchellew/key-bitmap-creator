using KeyBitmapCreator.Elements;
using KeyBitmapCreator.Helper;
using SixLabors.ImageSharp;

namespace KeyBitmapCreator;

internal class BuilderSpec
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

    public List<IBitmapElement> Elements { get; set; } = new();
}