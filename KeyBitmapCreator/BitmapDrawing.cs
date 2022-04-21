using KeyBitmapCreator.Elements;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace KeyBitmapCreator
{
    internal class BitmapDrawing
    {
        public static KeyBitmap Draw(int keySize, BuilderSpec builderSpec)
        {
            var image = new Image<Bgr24>(keySize, keySize);

            if (builderSpec.BackgroundColor != Color.Black)
                image.Mutate(x => x.Fill(builderSpec.BackgroundColor));

            if (builderSpec.Elements.Count > 0)
                foreach (var keyBitmapElement in builderSpec.Elements)
                    DrawElement(image, keyBitmapElement, keySize);

            if (Constants.DebugColorBlindnessMode != null)
                image.Mutate(x => x.ColorBlindness((ColorBlindnessMode)Constants.DebugColorBlindnessMode));

            return KeyBitmap.Create.FromImageSharpImage(image);
        }

        private static void DrawElement(Image<Bgr24> image, IBitmapElement keyBitmapElement, int keySize)
        {
            switch (keyBitmapElement)
            {
                case ImageElement imageElement:

                    ResizeImageElement(imageElement, keySize);

                    if (imageElement.FilterDefinitions.Count > 0)
                        foreach (var filterDefinition in imageElement.FilterDefinitions)
                            ApplyFilter(imageElement.Image, filterDefinition);

                    image.Mutate(x =>
                        x.DrawImage(imageElement.Image, imageElement.CalculateLocation(keySize), new GraphicsOptions()));
                    break;
                case TextElement textElement:
                    image.Mutate(x =>
                        x.DrawText(textElement.BuildTextOptions(keySize), textElement.Text, textElement.ForegroundColor));
                    break;
                case PolyElement polyElement:
                    if (polyElement.FilterDefinitions.Count > 0)
                        DrawFilteredPoly(image, keySize, polyElement);
                    else
                        DrawPoly(image, polyElement, keySize);
                    break;
                case FilterElement filterElement:
                    ApplyFilter(image, filterElement.FilterDefinition);
                    break;
            }
        }

        private static void ResizeImageElement(ImageElement imageElement, int keySize)
        {
            if (imageElement.ImageOptions.SizeMode == ImageSizeMode.FillKey)
                imageElement.Image.Mutate(x => x.Resize(new Size(keySize, keySize)));
        }

        private static void DrawPoly(Image image, PolyElement polyElement, int keySize)
        {
            image.Mutate(x =>
            {
                switch (polyElement.Definition)
                {
                    case PolygonDefinition:
                        x.FillPolygon(new DrawingOptions(), polyElement.Color, polyElement.GetRenderPoints(keySize));
                        break;
                    case PolylineDefinition polylineDefinition:
                        x.DrawPolygon(new DrawingOptions(), polyElement.Color, polylineDefinition.Width, polyElement.GetRenderPoints(keySize));
                        break;
                }
            });
        }

        private static void DrawFilteredPoly(Image<Bgr24> image, int keySize, PolyElement polyElement)
        {
            var polyImage = new Image<Bgra32>(keySize, keySize);
            DrawPoly(polyImage, polyElement, keySize);
            foreach (var filterDefinition in polyElement.FilterDefinitions)
                ApplyFilter(polyImage, filterDefinition);
            image.Mutate(x => x.DrawImage(polyImage, new Point(0, 0), new GraphicsOptions()));
        }

        private static void ApplyFilter(Image image, FilterDefinition filterDefinition)
        {
            image.Mutate(x =>
            {
                switch (filterDefinition.FilterType)
                {
                    case FilterType.Brightness:
                        var brightnessDefinition = (BrightnessFilterParams)filterDefinition.Parameters!;
                        x.Brightness(brightnessDefinition.Brightness);
                        break;
                    case FilterType.Greyscale:
                        var greyscaleDefinition = (GreyscaleFilterParams)filterDefinition.Parameters!;
                        x.Grayscale(greyscaleDefinition.Mode, greyscaleDefinition.Amount);
                        break;
                    case FilterType.Opacity:
                        var opacityDefinition = (OpacityFilterParams)filterDefinition.Parameters!;
                        x.Opacity(opacityDefinition.Amount);
                        break;
                    case FilterType.Invert:
                        x.Invert();
                        break;
                }
            });
        }
    }
}
