namespace KeyBitmapCreator.Elements;

interface IBitmapElement
{
    
}

internal class BaseElement : IBitmapElement
{
    public ElementLayoutOptions LayoutOptions { get; set; }

    public BaseElement(ElementLayoutOptions? layoutOptions)
    {
        LayoutOptions = layoutOptions ?? new ElementLayoutOptions();
    }
}