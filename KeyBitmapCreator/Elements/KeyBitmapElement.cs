namespace KeyBitmapCreator.Elements;

public class KeyBitmapElement
{
    public int Padding { get; set; } = KeyBitmapConstants.DefaultPadding;

    public HorizontalAlignment HorizontalAlignment { get; }

    public VerticalAlignment VerticalAlignment { get; }

    public KeyBitmapElement(HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center)
    {
        VerticalAlignment = verticalAlignment;
        HorizontalAlignment = horizontalAlignment;
    }
}