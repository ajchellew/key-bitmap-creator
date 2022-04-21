using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DemoWpf.ExampleDecks;
using KeyBitmapCreator.Helper;
using OpenMacroBoard.SDK;
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
        private IDeckHandler _handler;

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
            BrightnessSlider.Value = 50;
            //_deck.SetBrightness(100);

            _handler = new FlagsDeck(_deck);
            _deck.KeyStateChanged += DeckOnKeyStateChanged;
        }

        private void DeckOnKeyStateChanged(object? sender, KeyEventArgs e)
        {
            _handler.OnKeyStateChanged(e);
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

        private void ButtonDeckGithub_OnClick(object sender, RoutedEventArgs e)
        {
            _handler = new GithubDeck(_deck);
        }

        private void ButtonDeckFlags_OnClick(object sender, RoutedEventArgs e)
        {
            _handler = new FlagsDeck(_deck);
        }

        private void ButtonDeckScrolling_OnClick(object sender, RoutedEventArgs e)
        {
            _handler = new ScrollingDeck(_deck);
        }
    }
}
