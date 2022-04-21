using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DemoWpf.Utils;
using KeyBitmapCreator;
using KeyBitmapCreator.Elements;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;

namespace DemoWpf.ExampleDecks
{
    // links to some github things

    public class GithubDeck : IDeckHandler
    {
        private readonly IMacroBoard _deck;

        public GithubDeck(IMacroBoard deck)
        {
            _deck = deck;
            InitialiseDeck();
        }

        private void InitialiseDeck()
        {
            for (int keyId = 0; keyId < _deck.Keys.Count; keyId++)
            {
                KeyBitmap keyBitmap;

                switch (keyId)
                {
                    /*// top left
                    case 0:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText((keyId + 1).ToString(), TextElementOptions.Large)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("ArrowTopLeft")), ElementLayoutOptions.TopLeft)
                            .Build();
                        break;
                    // top right
                    case 4:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText((keyId + 1).ToString(), TextElementOptions.Large)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("ArrowTopRight")), ElementLayoutOptions.TopRight)
                            .Build();
                        break;
                    // bottom left
                    case 10:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText((keyId + 1).ToString(), TextElementOptions.Large)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("ArrowBottomLeft")), ElementLayoutOptions.BottomLeft)
                            .Build();
                        break;
                    // bottom right
                    case 14:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText((keyId + 1).ToString(), TextElementOptions.Large)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("ArrowBottomRight")), ElementLayoutOptions.BottomRight)
                            .Build();
                        break;*/

                    case 6:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromTop(33), Color.Red))
                            .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.HorizontalRectangleFromCenter(35), Color.White))
                            .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromBottom(33), Color.Red))
                            .Build();
                        break;

                    case 7:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .SetForegroundColor(Color.Black)
                            .AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromBottom(30), Color.White))
                            .AddText("Github", null, ElementLayoutOptions.BottomCenter)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)Application.Current.FindResource("Github")), null, new ElementLayoutOptions() { VerticalAlignment = KeyBitmapCreator.VerticalAlignment.Top, PaddingTopBottom = 5 })
                            .Build();
                        break;

                    case 8:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            //.AddPoly(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromLeft()))
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)Application.Current.FindResource("Uk")), ImageElementOptions.FillKey())
                            //.AddFilter(FilterDefinition.Invert())
                            .Build();
                        break;

                    // any other
                    default:
                        var builder = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText((keyId + 1).ToString(), TextElementOptions.Large);
                        //.AddFilter(FilterDefinition.Opacity())

                        foreach (var polyDefinition in BuildOutlinePolys(keyId))
                            builder.AddPoly(polyDefinition, FilterDefinition.Opacity(0.3f));

                        keyBitmap = builder.Build();
                        break;
                }

                _deck.SetKeyBitmap(keyId, keyBitmap);
            }

            //_deck.KeyStateChanged += DeckOnKeyStateChanged;
        }

        private List<IPolyDefinition> BuildOutlinePolys(int keyId)
        {
            var polys = new List<IPolyDefinition>();
            switch (keyId)
            {
                case >= 0 and <= 4:
                    polys.Add(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromTop(5), Color.White));
                    break;
                case >= 10 and <= 14:
                    polys.Add(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromBottom(5), Color.White));
                    break;
            }
            switch (keyId)
            {
                case 0 or 5 or 10:
                    polys.Add(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromLeft(5), Color.White));
                    break;
                case 4 or 9 or 14:
                    polys.Add(PolygonDefinition.CreateRelative(RelativePointsHelper.RectangleFromRight(5), Color.White));
                    break;
            }
            return polys;
        }

        public void OnKeyStateChanged(KeyEventArgs e)
        {
            var keyId = e.Key;

            if (e.IsDown)
            {
                //KeyEventText.Dispatcher.Invoke(() => { KeyEventText.Text = "Pressed - " + (keyId + 1); });
            }
            else
            {
                //KeyEventText.Dispatcher.Invoke(() => { KeyEventText.Text = "Released - " + (keyId + 1); });

                switch (keyId)
                {
                    case 6:
                        SystemUtils.OpenUrl("https://github.com/OpenMacroBoard");
                        break;
                    case 7:
                        SystemUtils.OpenUrl("https://github.com");
                        break;
                    case 8:
                        SystemUtils.OpenUrl("https://github.com/ajchellew/key-bitmap-creator");
                        break;
                }
            }
        }
    }
}
