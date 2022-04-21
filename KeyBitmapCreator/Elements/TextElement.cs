using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Elements;

internal class TextElement : BaseElement
{
    public string Text { get; }

    public TextElementOptions TextOptions { get; }

    public BuilderSpec ParentSpec { get; }
    
    public Color ForegroundColor => TextOptions.ForegroundColor ?? ParentSpec.ForegroundColor;

    public TextElement(string text, BuilderSpec parentSpec, TextElementOptions? textElementOptions = null, ElementLayoutOptions? layoutOptions = null) : base(layoutOptions)
    {
        Text = text;
        ParentSpec = parentSpec;
        TextOptions = textElementOptions ?? new TextElementOptions();
    }

    public TextOptions BuildTextOptions(int keySize)
    {
        int x;
        int y;

        var textHorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Center;
        var textVerticalAlignment = SixLabors.Fonts.VerticalAlignment.Center;
        var textAlignment = TextAlignment.Center;

        switch (LayoutOptions.HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
                x = LayoutOptions.PaddingLeftRight;
                textHorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left;
                textAlignment = TextAlignment.Start;
                break;
            default:
            case HorizontalAlignment.Center:
                x = keySize / 2;
                break;
            case HorizontalAlignment.Right:
                x = keySize - LayoutOptions.PaddingLeftRight;
                textHorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Right;
                textAlignment = TextAlignment.End;
                break;
        }

        switch (LayoutOptions.VerticalAlignment)
        {
            case VerticalAlignment.Top:
                y = LayoutOptions.PaddingTopBottom;
                textVerticalAlignment = SixLabors.Fonts.VerticalAlignment.Top;
                break;
            default:
            case VerticalAlignment.Center:
                y = keySize / 2;
                break;
            case VerticalAlignment.Bottom:
                y = keySize - LayoutOptions.PaddingTopBottom;
                textVerticalAlignment = SixLabors.Fonts.VerticalAlignment.Bottom;
                break;
        }

        TextOptions textOptions = new TextOptions(GetFont(TextOptions.FontSize))
        {
            HorizontalAlignment = textHorizontalAlignment,
            VerticalAlignment = textVerticalAlignment,
            TextAlignment = textAlignment,
            Origin = new Vector2(x, y),
            WrappingLength = keySize,
            LineSpacing = 1.3f
        };
        return textOptions;
    }

    private Font GetFont(FontSize fontSize)
    {
        return fontSize switch
        {
            FontSize.Small => Constants.SmallFont,
            FontSize.Large => Constants.LargeFont,
            _ => Constants.NormalFont
        };
    }
}

public class TextElementOptions
{
    public static TextElementOptions Large = new() { FontSize = FontSize.Large };

    public static TextElementOptions Small = new() { FontSize = FontSize.Small };

    public FontSize FontSize { get; set; } = FontSize.Normal;

    public Color? ForegroundColor { get; set; }
}