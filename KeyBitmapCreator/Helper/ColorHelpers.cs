using System.Net.Http.Headers;
using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Helper;

public class ColorHelpers
{
    public static Color GetForegroundColor(Color? backgroundColor)
    {
        if (backgroundColor == null)
            return Constants.DefaultForegroundColor;

        var hex = "#" + backgroundColor.Value.ToHex().Substring(0, 6); // rrggbbaa
        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hex);
        return color.GetBrightness() > 0.5 ? Color.Black : Color.White;
    }
}