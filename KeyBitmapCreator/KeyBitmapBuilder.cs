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
    private readonly BuilderSpec _builderSpec = new();

    public KeyBitmapBuilder(int keySize)
    {
        _keySize = keySize;
    }

    public KeyBitmap Build()
    {
        var image = new Image<Bgr24>(_keySize, _keySize);

        if (_builderSpec.BackgroundColor != Color.Black)
            image.Mutate(x => x.BackgroundColor(_builderSpec.BackgroundColor));
        
        if (_builderSpec.Elements.Count > 0)
        {
            foreach (var keyBitmapElement in _builderSpec.Elements)
            {
                switch (keyBitmapElement)
                {
                    case ImageElement imageElement:
                        image.Mutate(x => x.DrawImage(imageElement.Image, imageElement.CalculateLocation(_keySize), new GraphicsOptions()));
                        break;
                    case TextElement textElement:
                        image.Mutate(x => x.DrawText(textElement.BuildTextOptions(_keySize), textElement.Text, textElement.ForegroundColor));
                        break;
                }
            }
        }

        if (Constants.DebugColorBlindnessMode != null)
            image.Mutate(x => x.ColorBlindness((ColorBlindnessMode)Constants.DebugColorBlindnessMode));

        return KeyBitmap.Create.FromImageSharpImage(image);
    }

    public KeyBitmapBuilder SetForegroundColor(Color color)
    {
        _builderSpec.ForegroundColor = color;
        return this;
    }

    public KeyBitmapBuilder SetBackgroundColor(Color color)
    {
        _builderSpec.BackgroundColor = color;
        return this;
    }

    public KeyBitmapBuilder AddImage(Image image, ElementLayoutOptions? layoutOptions = null)
    {
        _builderSpec.Elements.Add(new ImageElement(image, layoutOptions));
        return this;
    }

    public KeyBitmapBuilder AddText(string text, TextElementOptions? textOptions = null, ElementLayoutOptions? layoutOptions = null)
    {
        _builderSpec.Elements.Add(new TextElement(text, _builderSpec, textOptions, layoutOptions));
        return this;
    }
}