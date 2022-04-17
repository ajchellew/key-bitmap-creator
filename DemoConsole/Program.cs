// See https://aka.ms/new-console-template for more information

using System.Reflection;
using KeyBitmapCreator;
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
        .AddText("1", FontSize.Normal, null, HorizontalAlignment.Left, VerticalAlignment.Top)
        .Build();
    deck.SetKeyBitmap(0, key0);

    var key1 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("2", FontSize.Large, null, HorizontalAlignment.Center, VerticalAlignment.Top)
        .Build();
    deck.SetKeyBitmap(1, key1);

    var key2 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.Blue)
        .AddText("3", FontSize.Normal, Color.White, HorizontalAlignment.Center, VerticalAlignment.Top)
        .Build();
    deck.SetKeyBitmap(2, key2);

    var key3 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("4", FontSize.Large, null, HorizontalAlignment.Center, VerticalAlignment.Top)
        .Build();
    deck.SetKeyBitmap(3, key3);

    var key4 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.White)
        .AddText("5", FontSize.Normal, Color.Blue, HorizontalAlignment.Right, VerticalAlignment.Top)
        .Build();
    deck.SetKeyBitmap(4, key4);

    // Row 2

    var key5 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("6", FontSize.Small, null, HorizontalAlignment.Left, VerticalAlignment.Center)
        .Build();
    deck.SetKeyBitmap(5, key5);


    using var bottomLeftStream = assembly.GetManifestResourceStream("DemoConsole.Images.bottomleft.png");
    using var bottomRightStream = assembly.GetManifestResourceStream("DemoConsole.Images.bottomright.png");
    using var topLeftStream = assembly.GetManifestResourceStream("DemoConsole.Images.topleft.png");
    using var topRightStream = assembly.GetManifestResourceStream("DemoConsole.Images.topright.png");

    var key6 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddImage(Image.Load(bottomLeftStream), HorizontalAlignment.Left, VerticalAlignment.Bottom)
        .AddImage(Image.Load(bottomRightStream), HorizontalAlignment.Right, VerticalAlignment.Bottom)
        .AddImage(Image.Load(topLeftStream), HorizontalAlignment.Left, VerticalAlignment.Top)
        .AddImage(Image.Load(topRightStream), HorizontalAlignment.Right, VerticalAlignment.Top)
        .Build();
    deck.SetKeyBitmap(6, key6);

    var key7 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.Red)
        .SetForegroundColor(Color.BurlyWood)
        .AddText("8", FontSize.Large)
        .AddText("1", FontSize.Normal, Color.Blue, HorizontalAlignment.Left, VerticalAlignment.Top)
        .AddText("2", FontSize.Normal, null, HorizontalAlignment.Right, VerticalAlignment.Top)
        .AddText("3", FontSize.Normal, Color.Blue, HorizontalAlignment.Left, VerticalAlignment.Bottom)
        .AddText("4", FontSize.Normal, null, HorizontalAlignment.Right, VerticalAlignment.Bottom)
        .Build();
    deck.SetKeyBitmap(7, key7);

    using var topStream = assembly.GetManifestResourceStream("DemoConsole.Images.image.png");

    var key8 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.Red)
        .AddImage(Image.Load(topStream), HorizontalAlignment.Center, VerticalAlignment.Top)
        .AddText("Text", FontSize.Normal, Color.Black, HorizontalAlignment.Center, VerticalAlignment.Bottom)
        .Build();
    deck.SetKeyBitmap(8, key8);

    var key9 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("10", FontSize.Small, null, HorizontalAlignment.Right, VerticalAlignment.Center)
        .Build();
    deck.SetKeyBitmap(9, key9);

    // Row 3

    var key10 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .SetBackgroundColor(Color.White)
        .SetForegroundColor(Color.Red)
        .AddText("11", FontSize.Normal, null, HorizontalAlignment.Left, VerticalAlignment.Bottom)
        .Build();
    deck.SetKeyBitmap(10, key10);

    var key14 = new KeyBitmapBuilder(deck.Keys.KeySize)
        .AddText("15", FontSize.Normal, null, HorizontalAlignment.Right, VerticalAlignment.Bottom)
        .Build();
    deck.SetKeyBitmap(14, key14);
}