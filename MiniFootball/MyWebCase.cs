namespace MiniFootball
{
    public static class MyWebCase
    {
        public static string FirstLetterUpperThenLower(string input)
        {
            return input[0].ToString().ToUpper()
                   + input.Substring(1, input.Length - 1).ToLower();
        }
    }
}
