/*using System;
using System.Globalization;

namespace ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string s,
                  string format = "ddMMyyyy", string cultureString = "en-US")
        {
            try
            {
                var r = DateTime.ParseExact(
                    s: s,
                    format: format,
                    provider: CultureInfo.GetCultureInfo(cultureString));
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; 
            }
        }

        public static DateTime ToDateTime(this string s,
                    string format, CultureInfo culture)
        {
            try
            {
                var r = DateTime.ParseExact(s: s, format: format,
                                        provider: culture);
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw;
            }

        }

    }
}
*/