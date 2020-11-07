namespace Api.Extensions
{
    public static class StringExtensions
    {
        public static string AddSpace(this string text)
        {

            return text.Replace("%20", " ");
        }
    }
}