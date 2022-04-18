namespace KeyBitmapCreator;

public enum VerticalAlignment
{
    Top,
    Center,
    Bottom
}

public enum HorizontalAlignment
{
    Left,
    Center,
    Right
}

public enum FontSize
{
    Small,
    Normal,
    Large
}

public enum ImageSizeMode
{
    None,
    FillKey
}

public class ElementLayoutOptions
{
    public static ElementLayoutOptions TopLeft = new() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };

    public static ElementLayoutOptions TopCenter = new() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };

    public static ElementLayoutOptions TopRight = new() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top };

    public static ElementLayoutOptions BottomLeft = new() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom };
    
    public static ElementLayoutOptions BottomCenter = new() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom };

    public static ElementLayoutOptions BottomRight = new() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };

    public static ElementLayoutOptions CenterRight = new() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };

    public static ElementLayoutOptions CenterLeft = new() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center };

    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

    public int PaddingTop { get; set; } = Constants.DefaultPadding;
    public int PaddingBottom { get; set; } = Constants.DefaultPadding;
    public int PaddingLeft { get; set; } = Constants.DefaultPadding;
    public int PaddingRight { get; set; } = Constants.DefaultPadding;
}