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

        public static string ToSentenceCase(string input)
        {
            return input[0].ToString().ToUpper()
                   + input.Substring(1, input.Length - 1).ToLower();
        }
    }
}
