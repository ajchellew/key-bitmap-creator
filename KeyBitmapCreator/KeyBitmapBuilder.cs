using KeyBitmapCreator.Elements;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace KeyBitmapCreator;

public class KeyBitmapBuilder
{
    private readonly int _keySize;
    private readonly KeyBitmapSpec _keyBitmapSpec = new();

    public KeyBitmapBuilder(int keySize)
    {
        _keySize = keySize;
    }

    public KeyBitmap Build()
    {
        var image = new Image<Bgr24>(_keySize, _keySize);

        if (_keyBitmapSpec.BackgroundColor != Color.Black)
            image.Mutate(x => x.Fill(_keyBitmapSpec.BackgroundColor));
        
        if (_keyBitmapSpec.ImageElements.Count > 0)
        {
            foreach (var keyBitmapImage in _keyBitmapSpec.ImageElements)
                image.Mutate(x => x.DrawImage(keyBitmapImage.Image, keyBitmapImage.CalculateLocation(_keySize), new GraphicsOptions()));
        }

        if (_keyBitmapSpec.TextElements.Count > 0)
        {
            foreach (var keyBitmapText in _keyBitmapSpec.TextElements)
                image.Mutate(x => x.DrawText(keyBitmapText.BuildTextOptions(_keySize), keyBitmapText.Text, keyBitmapText.ForegroundColor));
        }

        return KeyBitmap.Create.FromImageSharpImage(image);
    }

    public KeyBitmapBuilder SetForegroundColor(Color color)
    {
        _keyBitmapSpec.ForegroundColor = color;
        return this;
    }

    public KeyBitmapBuilder SetBackgroundColor(Color color)
    {
        _keyBitmapSpec.BackgroundColor = color;
        return this;
    }

    public KeyBitmapBuilder AddImage(Image image, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center)
    {
        _keyBitmapSpec.ImageElements.Add(new KeyBitmapImage(image, horizontalAlignment, verticalAlignment));
        return this;
    }

    public KeyBitmapBuilder AddText(string text, FontSize fontSize = FontSize.Normal, Color? foregroundColor = null, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center)
    {
        _keyBitmapSpec.TextElements.Add(new KeyBitmapText(text, _keyBitmapSpec, fontSize, foregroundColor, horizontalAlignment, verticalAlignment));
        return this;
    }
}