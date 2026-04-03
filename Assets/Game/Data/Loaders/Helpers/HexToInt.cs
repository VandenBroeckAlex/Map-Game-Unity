public static class ColorUtilities
{
    public static int HexToInt(string color)
    {
        string hex = color.Replace("#", "");
        uint argb = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        return (int)argb;
    }
}