using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Elements;

internal class PolyElement : IBitmapElement
{
    public IPolyDefinition Definition { get; }

    public BuilderSpec ParentSpec { get; }

    public PolyElement(IPolyDefinition definition, BuilderSpec parentSpec)
    {
        Definition = definition;
        ParentSpec = parentSpec;
    }

    public Color Color => Definition.Color ?? ParentSpec.ForegroundColor;

    public PointF[] GetRenderPoints(int keySize)
    {
        var points = Definition.Points;
        if (points != null)
        {
            // a filled poly must have the same start and end point
            if (points[0] != points[points.Length - 1])
            {
                var newArray = new PointF[points.Length + 1];
                for (var index = 0; index < points.Length; index++) newArray[index] = points[index];
                newArray[newArray.Length - 1] = points[0];
            }
            return points;
        }

        var relativePoints = Definition.RelativePoints;
        if (relativePoints != null)
        {
            PointF[] pointFs = new PointF[relativePoints.Length];

            float onePc = (float)keySize / 100;

            for (int i = 0; i < relativePoints.Length; i++)
                pointFs[i] = new PointF(onePc * relativePoints[i].X, onePc * relativePoints[i].Y);

            return pointFs;
        }

        return Array.Empty<PointF>();
    }
}

public interface IPolyDefinition
{
    public Color? Color { get; set; }
    public PointF[]? Points { get; set; }

    // i.e. points as a percentage of the total key. Point[] points = { new(0, 70), new(100, 70), new(100, 100), new(0, 100), new(0, 70) };
    public Point[]? RelativePoints { get; set; }
}

public class PolygonDefinition : IPolyDefinition
{
    public static PolygonDefinition Create(PointF[] points, Color? color = null)
    {
        return new PolygonDefinition() { Points = points, Color = color };
    }

    public static PolygonDefinition CreateRelative(Point[] relativePoints, Color? color = null)
    {
        return new PolygonDefinition() { RelativePoints = relativePoints, Color = color };
    }

    public Color? Color { get; set; }
    public PointF[]? Points { get; set; }
    public Point[]? RelativePoints { get; set; }
}

public class PolylineDefinition : IPolyDefinition
{
    public static PolylineDefinition Create(PointF[] points, float width = 3, Color? color = null)
    {
        return new PolylineDefinition() { Points = points, Width = width, Color = color };
    }

    public static PolylineDefinition CreateRelative(Point[] relativePoints, float width = 3, Color? color = null)
    {
        return new PolylineDefinition() { RelativePoints = relativePoints, Width = width, Color = color };
    }

    public float Width { get; set; }
    public Color? Color { get; set; }
    public PointF[]? Points { get; set; }
    public Point[]? RelativePoints { get; set; }
}

public static class RelativePointsHelper
{
    public static Point[] RectangleFromTop(int percentHeight = 50)
    {
        return new Point[] { new(0, 0), new(100, 0), new(100, percentHeight), new(0, percentHeight), new(0, 0) };
    }

    public static Point[] RectangleFromBottom(int percentHeight = 50)
    {
        return new Point[] { new(0, 100 - percentHeight), new(100, 100 - percentHeight), new(100, 100), new(0, 100), new(0, 100 - percentHeight) };
    }

    public static Point[] HorizontalRectangleFromCenter(int percentHeight = 50)
    {
        var heightOffset = percentHeight /2;
        return new Point[] { new(0, 50 - heightOffset), new(100, 50 - heightOffset), new(100, 50 + heightOffset), new(0, 50 + heightOffset), new(0, 50 - heightOffset) };
    }

    public static Point[] RectangleFromLeft(int percentWidth = 50)
    {
        return new Point[] { new(0, 0), new(percentWidth, 0), new(percentWidth, 100), new(0, 100), new(0, 0) };
    }

    public static Point[] RectangleFromRight(int percentWidth = 50)
    {
        return new Point[] { new(100, 0), new(100, 100), new(100 - percentWidth, 100), new(100 - percentWidth, 0), new(100, 0) };
    }

    public static Point[] VerticalRectangleFromCenter(int percentWidth = 50)
    {
        var widthOffset = percentWidth / 2;
        return new Point[] { new(50 - widthOffset, 0), new(50 + widthOffset, 0), new(50 + widthOffset, 100), new(50 - widthOffset, 100), new(50 - widthOffset, 0) };
    }
}
