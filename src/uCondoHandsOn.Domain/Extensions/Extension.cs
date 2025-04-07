using System.Globalization;

namespace uCondoHandsOn.Domain.Extensions
{
    public static class Extension
    {
        public static bool ContainsNormalized(string source , string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
                return false;

            return CultureInfo.InvariantCulture.CompareInfo
                .IndexOf(source, target, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) >= 0;

           
        }
    }
}