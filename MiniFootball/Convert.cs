namespace MiniFootball
{
    using System.Globalization;

    public static class Convert
    {
        public static string ToTitleCase(string input)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            
            var result =  ti.ToTitleCase(input);
            
            return result;
        }
    }
}
