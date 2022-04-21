using System;
using System.Windows;
using System.Windows.Controls;
using DemoWpf.Utils;
using KeyBitmapCreator;
using KeyBitmapCreator.Elements;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace DemoWpf.ExampleDecks
{
    // deck of keys that scroll to show more

    public class ScrollingDeck : IDeckHandler
    {
        private readonly IMacroBoard _deck;

        private KeyBitmap _arrowUp;
        private KeyBitmap _arrowUpDisabled;
        private KeyBitmap _arrowDown;
        private KeyBitmap _arrowDownDisabled;

        private const int UpKeyId = 4;
        private const int DownKeyId = 14;

        private const int ItemsPerPage = 12;
        private const int ItemCount = ItemsPerPage * 4;

        private int _page;
        private int _pagesTotal = ItemCount / ItemsPerPage - 1;

        private readonly bool[] _selectedState = new bool[ItemCount];

        public ScrollingDeck(IMacroBoard deck)
        {
            _deck = deck;

            if (_deck.Keys.CountX != 5)
                throw new ArgumentException("This demo is only setup for the 5x3 Stream Deck");

            _deck.ClearKey(9);
            BuildFixedIcons();
            UpdateArrowKeys();
            BuildPage();
        }

        private void BuildPage()
        {
            var number = 1 + _page * ItemsPerPage;

            // 0 - 3
            for (int keyId = 0; keyId <= 3; keyId++)
            {
                SetNumberKeyBitmap(keyId, number);
                number++;
            }

            // 5 - 8
            for (int keyId = 5; keyId <= 8; keyId++)
            {
                SetNumberKeyBitmap(keyId, number);
                number++;
            }

            // 10 - 13
            for (int keyId = 10; keyId <= 13; keyId++)
            {
                SetNumberKeyBitmap(keyId, number);
                number++;
            }
        }

        private void SetNumberKeyBitmap(int keyId, int number)
        {
            _deck.SetKeyBitmap(keyId, GenerateFilledKeyBitmap(number, _selectedState[number-1] ? Color.Coral : null));
        }

        #region Arrow Keys

        private void BuildFixedIcons()
        {
            _arrowUp = GenerateImageKeyBitmap("ArrowUp");
            _arrowUpDisabled = GenerateImageKeyBitmap("ArrowUp", true);
            _arrowDown = GenerateImageKeyBitmap("ArrowDown");
            _arrowDownDisabled = GenerateImageKeyBitmap("ArrowDown", true);
        }

        private void UpdateArrowKeys()
        {
            _deck.SetKeyBitmap(UpKeyId, _page == 0 ? _arrowUpDisabled : _arrowUp);
            _deck.SetKeyBitmap(DownKeyId, _page == _pagesTotal ? _arrowDownDisabled : _arrowDown);
        }

        #endregion

        public void OnKeyStateChanged(KeyEventArgs e)
        {
            // on key up
            if (e.IsDown == false)
            {
                switch (e.Key)
                {
                    case UpKeyId:
                        if (_page > 0)
                        {
                            _page--;
                            UpdateArrowKeys();
                            BuildPage();
                        }
                        break;
                    case DownKeyId:
                        if (_page < _pagesTotal)
                        {
                            _page++;
                            UpdateArrowKeys();
                            BuildPage();
                        }
                        break;
                    case 9: // unassigned key
                        break;
                    default:
                        var number = FindNumberForKey(e.Key, _page);
                        var currentState = _selectedState[number-1];
                        _selectedState[number-1] = !currentState;
                        SetNumberKeyBitmap(e.Key, number);
                        break;
                }
            }
        }

        private int FindNumberForKey(int keyId, int page)
        {
            var result = keyId switch
            {
                >= 0 and <= 3 => keyId + 1,
                >= 5 and <= 8 => keyId,
                >= 10 and <= 13 => keyId - 1,
                _ => 1
            };

            return result + (page*ItemsPerPage);
        }


        public KeyBitmap GenerateFilledKeyBitmap(int keyNo, Color? backgroundColor = null, Image? subImage = null)
        {
            var builder = new KeyBitmapBuilder(_deck)
                .AddText(keyNo.ToString(), TextElementOptions.Large);
            if (backgroundColor != null)
                builder.SetBackgroundColor((Color)backgroundColor);
            if (subImage != null)
                builder.AddImage(subImage, null, ElementLayoutOptions.BottomRight);
            return builder.Build();
        }

        public KeyBitmap GenerateImageKeyBitmap(string canvasKey, bool isDisabled = false)
        {
            return GenerateImageKeyBitmap(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)Application.Current.FindResource(canvasKey)), isDisabled);
        }

        public KeyBitmap GenerateImageKeyBitmap(Image image, bool isDisabled = false)
        {
            var builder = new KeyBitmapBuilder(_deck)
                .AddImage(image);
            if (isDisabled)
                builder.AddFilter(FilterDefinition.Brightness(0.2f));
            return builder.Build();
        }
    }
}
