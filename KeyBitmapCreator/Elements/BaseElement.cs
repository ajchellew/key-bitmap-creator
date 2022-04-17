namespace KeyBitmapCreator.Elements;

internal class BaseElement
{
    public ElementLayoutOptions LayoutOptions { get; set; }

    public BaseElement(ElementLayoutOptions? layoutOptions)
    {
        LayoutOptions = layoutOptions ?? new ElementLayoutOptions();
    }
}