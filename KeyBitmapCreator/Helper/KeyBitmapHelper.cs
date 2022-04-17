using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;

namespace KeyBitmapCreator.Helper;

public class KeyBitmapHelper
{
    public static void RunImageSharpWarmup()
    {
        var image = new Image<Bgr24>(128, 128);
        image.Mutate(x => x.DrawText("Text", KeyBitmapConstants.LargeFont, Color.White, new PointF(0, 0)));
    }
}