using System.Globalization;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace KeyBitmapCreator;

internal class Constants
{
    public static readonly Color DefaultForegroundColor = Color.White;
    public const int DefaultPadding = 3;

    private const string FontFamily = "Arial";
    public static readonly Font LargeFont = SystemFonts.Get(FontFamily, CultureInfo.InvariantCulture).CreateFont(32);
    public static readonly Font NormalFont = SystemFonts.Get(FontFamily, CultureInfo.InvariantCulture).CreateFont(16);
    public static readonly Font SmallFont = SystemFonts.Get(FontFamily, CultureInfo.InvariantCulture).CreateFont(10);

    // For testing what a colored layout might look like to someone who is colorblind
    // Red-green color blindness is the most common - Deuteranomaly is the most common.
    public static readonly ColorBlindnessMode? DebugColorBlindnessMode = null;
}