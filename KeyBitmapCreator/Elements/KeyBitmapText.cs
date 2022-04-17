using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Elements;

public class KeyBitmapText : KeyBitmapElement
{
    public string Text { get; }
    public FontSize FontSize { get; }
    public KeyBitmapSpec ParentSpec { get; }
    
    private readonly Color? _foregroundColor;
    public Color ForegroundColor => _foregroundColor ?? ParentSpec.ForegroundColor;

    public KeyBitmapText(string text, KeyBitmapSpec parentSpec, FontSize fontSize = FontSize.Normal, Color? foregroundColor = null)
    {
        Text = text;
        ParentSpec = parentSpec;
        FontSize = fontSize;
        _foregroundColor = foregroundColor;
    }

    public KeyBitmapText(string text, KeyBitmapSpec parentSpec, FontSize fontSize = FontSize.Normal, Color? foregroundColor = null, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center) : base(horizontalAlignment, verticalAlignment)
    {
        Text = text;
        ParentSpec = parentSpec;
        FontSize = fontSize;
        _foregroundColor = foregroundColor;
    }

    public TextOptions BuildTextOptions(int keySize)
    {
        int x;
        int y;

        var textHorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Center;
        var textVerticalAlignment = SixLabors.Fonts.VerticalAlignment.Center;
        var textAlignment = TextAlignment.Center;

        switch (HorizontalAlignment)
        {
            case HorizontalAlignment.Left:
                x = Padding;
                textHorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left;
                textAlignment = TextAlignment.Start;
                break;
            default:
            case HorizontalAlignment.Center:
                x = keySize / 2;
                break;
            case HorizontalAlignment.Right:
                x = keySize - Padding;
                textHorizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Right;
                textAlignment = TextAlignment.End;
                break;
        }

        switch (VerticalAlignment)
        {
            case VerticalAlignment.Top:
                y = Padding;
                textVerticalAlignment = SixLabors.Fonts.VerticalAlignment.Top;
                break;
            default:
            case VerticalAlignment.Center:
                y = keySize / 2;
                break;
            case VerticalAlignment.Bottom:
                y = keySize - Padding;
                textVerticalAlignment = SixLabors.Fonts.VerticalAlignment.Bottom;
                break;
        }

        TextOptions textOptions = new TextOptions(GetFont(FontSize))
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
            FontSize.Small => KeyBitmapConstants.SmallFont,
            FontSize.Large => KeyBitmapConstants.LargeFont,
            _ => KeyBitmapConstants.NormalFont
        };
    }
}