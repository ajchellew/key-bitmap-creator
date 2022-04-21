using KeyBitmapCreator.Elements;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;

namespace KeyBitmapCreator;

public class KeyBitmapBuilder
{
    private readonly int _keySize;
    private readonly BuilderSpec _builderSpec = new();

    public KeyBitmapBuilder(IMacroBoard deck)
    {
        _keySize = deck.Keys.KeySize;
    }

    public KeyBitmapBuilder(int keySize)
    {
        _keySize = keySize;
    }

    public KeyBitmap Build()
    {
        return BitmapDrawing.Draw(_keySize, _builderSpec);
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

    public KeyBitmapBuilder AddImage(Image image, ImageElementOptions? imageOptions = null, ElementLayoutOptions? layoutOptions = null, FilterDefinition? filter = null)
    {
        _builderSpec.Elements.Add(new ImageElement(image, imageOptions, layoutOptions, filter));
        return this;
    }

    public KeyBitmapBuilder AddText(string text, TextElementOptions? textOptions = null, ElementLayoutOptions? layoutOptions = null)
    {
        _builderSpec.Elements.Add(new TextElement(text, _builderSpec, textOptions, layoutOptions));
        return this;
    }

    public KeyBitmapBuilder AddPoly(IPolyDefinition polyDefinition, FilterDefinition? filter = null)
    {
        _builderSpec.Elements.Add(new PolyElement(polyDefinition, _builderSpec, filter));
        return this;
    }

    public KeyBitmapBuilder AddFilter(FilterDefinition filterDefinition)
    {
        _builderSpec.Elements.Add(new FilterElement(filterDefinition));
        return this;
    }
}