using System;
using OpenMacroBoard.SDK;

namespace DemoWpf.Utils;

public static class KeyBitmapBasicFactoryExtraExtensions
{
    public static KeyBitmap FromHex(this IKeyBitmapFactory keyFactory, string hex)
    {
        byte r = (byte)Convert.ToInt32(hex.Substring(1, 2), 16);
        byte g = (byte)Convert.ToInt32(hex.Substring(3, 2), 16);
        byte b = (byte)Convert.ToInt32(hex.Substring(5, 2), 16);
        return keyFactory.FromRgb(r, g, b);
    }
}