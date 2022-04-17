using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace DemoWpf;

public class WpfKeyBitmapHelper
{
    public static Image ConvertCanvasToImage(Canvas canvas)
    {
        // if you don't do this it doesn't load.
        var size = new Size();
        canvas.Measure(size);
        canvas.Arrange(new Rect(size));

        RenderTargetBitmap bmp = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        bmp.Render(canvas);

        PngBitmapEncoder encoder = new PngBitmapEncoder();
        using MemoryStream memoryStream = new MemoryStream();
        encoder.Frames.Add(BitmapFrame.Create(bmp));
        encoder.Save(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return Image.Load(memoryStream);
    }
}