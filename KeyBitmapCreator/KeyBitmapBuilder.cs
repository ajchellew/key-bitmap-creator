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
            image.Mutate(x => x.Fill(_builderSpec.BackgroundColor));
        
        if (_builderSpec.Elements.Count > 0)
        {
            foreach (var keyBitmapElement in _builderSpec.Elements)
            {
                switch (keyBitmapElement)
                {
                    case ImageElement imageElement:

                        // a basic start
                        if (imageElement.ImageOptions.SizeMode == ImageSizeMode.FillKey)
                            imageElement.Image.Mutate(x => x.Resize(new Size(_keySize, _keySize)));

                        image.Mutate(x => x.DrawImage(imageElement.Image, imageElement.CalculateLocation(_keySize), new GraphicsOptions()));
                        break;
                    case TextElement textElement:
                        image.Mutate(x => x.DrawText(textElement.BuildTextOptions(_keySize), textElement.Text, textElement.ForegroundColor));
                        break;
                    case PolyElement polyElement:
                        image.Mutate(x =>
                        {
                            switch (polyElement.Definition)
                            {
                                case PolygonDefinition:
                                    x.FillPolygon(new DrawingOptions(), polyElement.Color, polyElement.GetRenderPoints(_keySize));
                                    break;
                                case PolylineDefinition polylineDefinition:
                                    x.DrawPolygon(new DrawingOptions(), polyElement.Color, polylineDefinition.Width, polyElement.GetRenderPoints(_keySize));
                                    break;
                            }
                        });
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

    public KeyBitmapBuilder AddImage(Image image, ImageElementOptions? imageOptions = null, ElementLayoutOptions? layoutOptions = null)
    {
        _builderSpec.Elements.Add(new ImageElement(image, imageOptions, layoutOptions));
        return this;
    }

    public KeyBitmapBuilder AddText(string text, TextElementOptions? textOptions = null, ElementLayoutOptions? layoutOptions = null)
    {
        _builderSpec.Elements.Add(new TextElement(text, _builderSpec, textOptions, layoutOptions));
        return this;
    }

    public KeyBitmapBuilder AddPoly(IPolyDefinition polyDefinition)
    {
        _builderSpec.Elements.Add(new PolyElement(polyDefinition, _builderSpec));
        return this;
    }

    /*public KeyBitmapBuilder AddPolygonPercentageSized(Point[] points, Color? color = null, bool filled = true)
    {
        PointF[] pointFs = new PointF[points.Length];

        float onePc = (float)_keySize / 100;

        for (int i = 0; i < points.Length; i++)
            pointFs[i] = new PointF(onePc  * points[i].X, onePc * points[i].Y);

        var element = new PolygonElement { Points = pointFs, Filled = filled };
        if (color != null)
            element.Color = (Color)color;
        _builderSpec.Elements.Add(element);
        return this;
    }*/
}