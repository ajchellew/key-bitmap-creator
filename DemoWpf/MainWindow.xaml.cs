using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KeyBitmapCreator;
using KeyBitmapCreator.Elements;
using KeyBitmapCreator.Helper;
using OpenMacroBoard.SDK;
using SixLabors.ImageSharp;
using StreamDeckSharp;
using KeyEventArgs = OpenMacroBoard.SDK.KeyEventArgs;

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
                    // top left
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

                    // middle
                    case 7:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("Box")))
                            .AddText((keyId + 1).ToString(), new TextElementOptions() { FontSize = KeyBitmapCreator.FontSize.Large, ForegroundColor = Color.Black })
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
                        break;

                    case 12:
                        keyBitmap = new KeyBitmapBuilder(_deck.Keys.KeySize)
                            .AddText("Github", null, ElementLayoutOptions.BottomCenter)
                            .AddImage(WpfKeyBitmapHelper.ConvertCanvasToImage((Canvas)FindResource("Github")), new ElementLayoutOptions() { VerticalAlignment = KeyBitmapCreator.VerticalAlignment.Top, PaddingTop = 10 })
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
    }
}
