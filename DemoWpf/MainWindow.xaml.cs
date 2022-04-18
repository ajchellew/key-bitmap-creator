using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KeyBitmapCreator;
using KeyBitmapCreator.Elements;
using KeyBitmapCreator.Helper;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using StreamDeckSharp;
using KeyEventArgs = OpenMacroBoard.SDK.KeyEventArgs;
using Point = SixLabors.ImageSharp.Point;

namespace DemoWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IReadOnlyList<IMacroBoard> _streamDecks;
        private bool _started = false;

        private IMacroBoard _deck;

        public MainWindow()
        {
            InitializeComponent();

            KeyBitmapHelper.RunImageSharpWarmup();

            // Initialize the Stream Decks and display information about them.
            var devices = StreamDeck.EnumerateDevices().ToList();
            if (!devices.Any())
            {
                MessageBox.Show("No StreamDeck found", "Error");
                return;
            }

            _streamDecks = devices.Select(device => StreamDeck.OpenDevice(device.DevicePath)).ToList();
            StreamDeckInfo.Text = string.Join("\r\n", devices.Zip(_streamDecks, (device, deck) =>
                $"{device.DeviceName} - Firmware: {deck.GetFirmwareVersion()} " +
                $"Keys: {deck.Keys.CountX}x{deck.Keys.CountY} " +
                $"Key size: {deck.Keys.KeySize}px " +
                $"Key gap: {deck.Keys.GapSize}px"));

            InitialiseDeck();
        }

        private void InitialiseDeck()
        {
            _started = true;

            _deck = _streamDecks[0];
            _deck.SetBrightness(100);

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
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("Github")), null, new ElementLayoutOptions() { VerticalAlignment = KeyBitmapCreator.VerticalAlignment.Top, PaddingTop = 5 })
                            .Build();
                        break;

                    case 8:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                                .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("Uk")), ImageElementOptions.FillKey())
                            .Build();
                        break;

                    // any other
                    default:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText((keyId + 1).ToString(), TextElementOptions.Large)
                            .Build();
                        break;
                }

                _deck.SetKeyBitmap(keyId, keyBitmap);
            }

            _deck.KeyStateChanged += DeckOnKeyStateChanged;
        }

        private void DeckOnKeyStateChanged(object? sender, KeyEventArgs e)
        {
            var keyId = e.Key;

            if (e.IsDown)
            {
                KeyEventText.Dispatcher.Invoke(() => { KeyEventText.Text = "Pressed - " + (keyId + 1); });
            }
            else
            {
                KeyEventText.Dispatcher.Invoke(() => { KeyEventText.Text = "Released - " + (keyId + 1); });

                switch (keyId)
                {
                    case 6:
                        OpenUrl("https://github.com/OpenMacroBoard");
                        break;
                    case 7:
                        OpenUrl("https://github.com");
                        break;
                    case 8:
                        OpenUrl("https://github.com/ajchellew/key-bitmap-creator");
                        break;
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_started)
            {
                _streamDecks[0].SetBrightness(10);
                _streamDecks[0].Dispose();
            }

            base.OnClosing(e);
        }

        private void BrightnessSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _deck?.SetBrightness((byte)e.NewValue);
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
