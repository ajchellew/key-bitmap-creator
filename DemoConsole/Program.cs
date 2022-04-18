// See https://aka.ms/new-console-template for more information

using System.Reflection;
using KeyBitmapCreator;
using KeyBitmapCreator.Elements;
using KeyBitmapCreator.Helper;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using StreamDeckSharp;

// not really much help in a console app
KeyBitmapHelper.RunImageSharpWarmup();

//Open the Stream Deck device
using (var deck = StreamDeck.OpenDevice())
{
    deck.SetBrightness(100);

    //AustrianFlag(deck);
    //Console.ReadKey();

    DrawingDemo(deck);
    Console.ReadKey();
}

void AustrianFlag(IMacroBoard macroBoard)
{
    //Create some color we use later to draw the flag of austria
    var red = new KeyBitmapBuilder(macroBoard.Keys.KeySize)
        .SetBackgroundColor(Color.FromRgb(237, 41, 57))
        .Build();
    var white = new KeyBitmapBuilder(macroBoard.Keys.KeySize)
        .SetBackgroundColor(Color.FromRgb(255, 255, 255))
        .Build();
    var rowColors = new[] { red, white, red };

    for (int i = 0; i < macroBoard.Keys.Count; i++)
        macroBoard.SetKeyBitmap(i, rowColors[i / 5]);
}

void DrawingDemo(IMacroBoard deck)
{
    var assembly = Assembly.GetExecutingAssembly();
    //var resources = assembly.GetManifestResourceNames();

    // Row 1

    var key0 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.White)
        .SetForegroundColor(Color.Red)
        .AddText("1", TextElementOptions.Large, ElementLayoutOptions.TopLeft)
        .Build();
    deck.SetKeyBitmap(0, key0);

    var key1 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("2", TextElementOptions.Large, ElementLayoutOptions.TopCenter)
        .Build();
    deck.SetKeyBitmap(1, key1);

    var key2 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.Blue)
        .AddText("3", new TextElementOptions() { ForegroundColor = Color.White }, ElementLayoutOptions.TopCenter)
        .Build();
    deck.SetKeyBitmap(2, key2);

    var key3 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("4", TextElementOptions.Large, ElementLayoutOptions.TopCenter)
        .Build();
    deck.SetKeyBitmap(3, key3);

    var key4 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.White)
        .AddText("5", new TextElementOptions() { ForegroundColor = Color.Blue }, ElementLayoutOptions.TopRight)
        .Build();
    deck.SetKeyBitmap(4, key4);

    // Row 2

    var key5 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("6", null, ElementLayoutOptions.CenterLeft)
        .Build();
    deck.SetKeyBitmap(5, key5);


    using var bottomLeftStream = assembly.GetManifestResourceStream("DemoConsole.Images.bottomleft.png");
    using var bottomRightStream = assembly.GetManifestResourceStream("DemoConsole.Images.bottomright.png");
    using var topLeftStream = assembly.GetManifestResourceStream("DemoConsole.Images.topleft.png");
    using var topRightStream = assembly.GetManifestResourceStream("DemoConsole.Images.topright.png");

    var key6 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddImage(Image.Load(bottomLeftStream), null, ElementLayoutOptions.BottomLeft)
        .AddImage(Image.Load(bottomRightStream), null, ElementLayoutOptions.BottomRight)
        .AddImage(Image.Load(topLeftStream), null, ElementLayoutOptions.TopLeft)
        .AddImage(Image.Load(topRightStream), null, new ElementLayoutOptions() { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, PaddingTop = 0, PaddingRight = 0 })
        .AddText("Test Images", TextElementOptions.Small)
        .Build();
    deck.SetKeyBitmap(6, key6);

    var key7 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.Red)
        .SetForegroundColor(Color.BurlyWood)
        .AddText("8", TextElementOptions.Large)
        .AddText("1", new TextElementOptions() { ForegroundColor = Color.Blue }, ElementLayoutOptions.TopLeft)
        .AddText("2", null, ElementLayoutOptions.TopRight)
        .AddText("3", new TextElementOptions() { ForegroundColor = Color.Blue }, ElementLayoutOptions.BottomLeft)
        .AddText("4", null, ElementLayoutOptions.BottomRight)
        .Build();
    deck.SetKeyBitmap(7, key7);

    using var topStream = assembly.GetManifestResourceStream("DemoConsole.Images.image.png");

    var key8 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.Red)
        .AddImage(Image.Load(topStream), null, new ElementLayoutOptions() { VerticalAlignment = VerticalAlignment.Top, PaddingTop = 0 })
        .AddText("Text", null, new ElementLayoutOptions() { VerticalAlignment = VerticalAlignment.Bottom, PaddingBottom = 6 })
        .Build();
    deck.SetKeyBitmap(8, key8);

    var key9 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("10", TextElementOptions.Small, ElementLayoutOptions.CenterRight)
        .Build();
    deck.SetKeyBitmap(9, key9);

    // Row 3

    var key10 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.White)
        .SetForegroundColor(Color.Red)
        .AddText("11", null, ElementLayoutOptions.BottomLeft)
        .Build();
    deck.SetKeyBitmap(10, key10);

    var key14 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromLeft(50), Color.Cyan))
        .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromTop(50), Color.DarkCyan))
        .AddText("15", null, ElementLayoutOptions.BottomRight)
        .Build();
    deck.SetKeyBitmap(14, key14);
}