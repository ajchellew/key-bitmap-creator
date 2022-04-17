﻿using SixLabors.ImageSharp;

namespace KeyBitmapCreator.Helper;

public class ColorHelpers
{
    public static Color GetForegroundColor(Color? backgroundColor)
    {
        if (backgroundColor == null)
            return KeyBitmapConstants.DefaultForegroundColor;
        return backgroundColor == Color.White || backgroundColor == Color.LightGray ? Color.Black : Color.White;
    }
}