namespace CORE.APP.Extensions
{
    // String için yardımcı metotlar
    public static class StringExtensions
    {
        // Eğer değer boş/null/whitespace ise defaultValue döndürür, değilse kendisini döndürür
        public static string HasNotAny(this string value, string defaultValue)
        {
            return HasNotAny(value) ? defaultValue : value;
        }

        // Değer boş, null veya sadece boşluklardan mı oluşuyor?
        public static bool HasNotAny(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        // Değer dolu mu (boş veya null değil mi)?
        public static bool HasAny(this string value)
        {
            return !HasNotAny(value);
        }
    }
}