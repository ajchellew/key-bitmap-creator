using System;
using DemoWpf.Utils;
using KeyBitmapCreator;
using KeyBitmapCreator.Elements;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;

namespace DemoWpf.ExampleDecks
{
    // press any key to cycle flags

    internal class FlagsDeck : IDeckHandler
    {
        private readonly IMacroBoard _deck;
        private int _flagNo;

        public FlagsDeck(IMacroBoard deck)
        {
            _deck = deck;

            if (_deck.Keys.CountX != 5)
                throw new ArgumentException("This demo is only setup for the 5x3 Stream Deck");
            
            DrawFlag();
        }

        private void DrawFlag()
        {
            switch (_flagNo)
            {
                case 0:
                    AustriaFlag();
                    break;
                case 1:
                    EnglandFlag();
                    break;
                case 2:
                    UkraineFlag();
                    break;
            }
        }

        private void AustriaFlag()
        {
            // the original creators austria flag. this is still the best way to make a bitmap that is one color
            var red = KeyBitmap.Create.FromRgb(237, 41, 57);
            var white = KeyBitmap.Create.FromRgb(255, 255, 255);
            var rowColors = new[] { red, white, red };
            for (int i = 0; i < _deck.Keys.Count; i++)
                _deck.SetKeyBitmap(i, rowColors[i / _deck.Keys.CountX]);
        }

        private void UkraineFlag()
        {
            var blueHex = "#005BBB";
            var blue = KeyBitmap.Create.FromHex(blueHex);
            for (int i = 0; i < _deck.Keys.CountX; i++)
                _deck.SetKeyBitmap(i, blue);

            var yellowHex = "#FFD500";
            var yellow = KeyBitmap.Create.FromHex(yellowHex);
            for (int i = _deck.Keys.Count; i >= _deck.Keys.Count - _deck.Keys.CountX; i--)
                _deck.SetKeyBitmap(i, yellow);

            var blueYellow = new KeyBitmapBuilder(_deck)
                .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromTop(),
                    Color.ParseHex(blueHex)))
                .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromBottom(),
                    Color.ParseHex(yellowHex)))
                .Build();
            for (int i = _deck.Keys.CountX; i < _deck.Keys.CountX * 2; i++)
                _deck.SetKeyBitmap(i, blueYellow);
        }

        public void EnglandFlag()
        {
            // too lazy to do UK without using the SVG
            var red = KeyBitmap.Create.FromRgb(237, 41, 57);
            var white = KeyBitmap.Create.FromRgb(255, 255, 255);

            var redWhite = new KeyBitmapBuilder(_deck)
                .SetBackgroundColor(Color.White)
                .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.HorizontalRectangleFromCenter(70),
                    Color.FromRgb(237, 41, 57)))
                .Build();

            _deck.SetKeyBitmap(0, white);
            _deck.SetKeyBitmap(1, white);

            _deck.SetKeyBitmap(3, white);
            _deck.SetKeyBitmap(4, white);

            _deck.SetKeyBitmap(5, redWhite);
            _deck.SetKeyBitmap(6, redWhite);

            _deck.SetKeyBitmap(2, red);
            _deck.SetKeyBitmap(7, red);
            _deck.SetKeyBitmap(12, red);

            _deck.SetKeyBitmap(8, redWhite);
            _deck.SetKeyBitmap(9, redWhite);

            _deck.SetKeyBitmap(10, white);
            _deck.SetKeyBitmap(11, white);

            _deck.SetKeyBitmap(13, white);
            _deck.SetKeyBitmap(14, white);
        }

        public void OnKeyStateChanged(KeyEventArgs e)
        {
            if (e.IsDown) return;

            if (_flagNo < 3) _flagNo++; else _flagNo = 0;
            DrawFlag();
        }
    }
}
