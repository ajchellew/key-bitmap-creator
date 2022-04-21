using SixLabors.ImageSharp.Processing;

namespace KeyBitmapCreator.Elements;

public class FilterElement : IBitmapElement
{
    public FilterDefinition FilterDefinition { get; }

    public FilterElement(FilterDefinition definition)
    {
        FilterDefinition = definition;
    }
}

public class FilterDefinition
{
    public FilterType FilterType { get; }

    public IFilterParams? Parameters { get; }

    private FilterDefinition(FilterType filterType, IFilterParams? parameters = null)
    {
        FilterType = filterType;
        Parameters = parameters;
    }

    public static FilterDefinition Brightness(float brightness = 0.5f)
    {
        return new FilterDefinition(FilterType.Brightness, new BrightnessFilterParams { Brightness = brightness });
    }

    public static FilterDefinition Greyscale(GrayscaleMode mode = GrayscaleMode.Bt601, float amount = 1)
    {
        return new FilterDefinition(FilterType.Greyscale, new GreyscaleFilterParams { Mode = mode, Amount = amount });
    }

    public static FilterDefinition Opacity(float amount = 0.5f)
    {
        return new FilterDefinition(FilterType.Opacity, new OpacityFilterParams { Amount = amount });
    }

    public static FilterDefinition Invert()
    {
        return new FilterDefinition(FilterType.Invert);
    }
}

public enum FilterType
{
    Brightness,
    Greyscale,
    Opacity,
    Invert,
    Glow
}

public interface IFilterParams
{

}

public class GreyscaleFilterParams : IFilterParams
{
    public GrayscaleMode Mode { get; set; } = GrayscaleMode.Bt601;

    public float Amount { get; set; } = 1f;
}

public class BrightnessFilterParams : IFilterParams
{
    public float Brightness { get; set; } = 0.5f;
}

public class OpacityFilterParams : IFilterParams
{
    public float Amount { get; set; } = 0.5f;
}