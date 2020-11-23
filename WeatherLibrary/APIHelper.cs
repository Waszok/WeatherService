using ExtensionMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace WeatherLibrary
{
    public static class APIHelper
    {

        public static string GetAddress(string coordinate)
        {
            string api_key = "KSyPETMoEbvOGD2aknukZI7v9Knb8MLX";

            string city = null;

            string url = $"https://api.tomtom.com/search/2/reverseGeocode/{coordinate}.json?key={api_key}";
            using (var webClient = new System.Net.WebClient() { Encoding = System.Text.Encoding.UTF8 })
            {
                string json = webClient.DownloadString(url);

                dynamic dObject = JsonConvert.DeserializeObject<dynamic>(json);
                string address = dObject["addresses"][0]["address"]["localName"].ToString();
                city = Normalize(address.ToLower());
            }

            return city;
        }

        public static char NormalizeChar(char c)
        {
            switch (c)
            {
                case 'ą':
                    return 'a';
                case 'ć':
                    return 'c';
                case 'ę':
                    return 'e';
                case 'ł':
                    return 'l';
                case 'ń':
                    return 'n';
                case 'ó':
                    return 'o';
                case 'ś':
                    return 's';
                case 'ż':
                case 'ź':
                    return 'z';
                default:
                    return c;
            }
        }

        public static string Normalize(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }
            char[] charArray = word.ToCharArray();
            char[] normalizedArray = new char[charArray.Length];
            for (int i = 0; i < normalizedArray.Length; i++)
            {
                normalizedArray[i] = NormalizeChar(charArray[i]);
            }
            return new string(normalizedArray);
        }


        public static List<List<string>> DownloadData(string _city)
        {
            List<List<string>> tmpList = new List<List<string>>();
            List<string> a = new List<string>();
            List<string> DateWeekDays = new List<string>();
            List<string> TempWeekDays = new List<string>();
            List<string> PrecipitationWeekDays = new List<string>();

            string _url = null;
            if (!string.IsNullOrEmpty(_city))
                _url = $"https://api-cdn.apixu.com/v1/forecast.xml?key=e39bb7cee08c4064806165158191708&q={_city}&days=7";
            if (!string.IsNullOrEmpty(_url))
            {
                XDocument doc = XDocument.Load(_url);


                #region Get weather icon image

                string iconUrl = (string)doc.Descendants("icon").FirstOrDefault();

                //WebClient client = new WebClient();
                //byte[] image = client.DownloadData($"https:{iconUrl}");

                //BitmapImage bitMap = ToImage(image);

                //WeatherIcon = bitMap;
                a.Add(iconUrl);
                #endregion

                #region Get weather data for current day 

                a.Add((string)doc.Descendants("country").FirstOrDefault());
                a.Add((string)doc.Descendants("lat").FirstOrDefault());
                a.Add((string)doc.Descendants("lon").FirstOrDefault());
                a.Add((string)doc.Descendants("temp_c").FirstOrDefault());
                a.Add((string)doc.Descendants("temp_f").FirstOrDefault());
                a.Add((string)doc.Descendants("text").FirstOrDefault());
                a.Add((string)doc.Descendants("feelslike_c").FirstOrDefault());
                a.Add((string)doc.Descendants("feelslike_f").FirstOrDefault());
                a.Add((string)doc.Descendants("wind_kph").FirstOrDefault());
                a.Add((string)doc.Descendants("humidity").FirstOrDefault());
                a.Add((string)doc.Descendants("pressure_mb").FirstOrDefault());
                a.Add((string)doc.Descendants("precip_mm").FirstOrDefault());

                string localTime = (string)doc.Descendants("localtime").FirstOrDefault();
                string[] dateTimeElements = localTime.Split();

                if (dateTimeElements.Length > 0)
                {
                    string localDate = dateTimeElements[0];
                    var date = localDate.ToDateTime(format: "yyyy-MM-dd");

                    a.Add($"{date.DayOfWeek.ToString()} ");
                    a.Add($"{date.ToString("dd.MM.yyyy")}, ");

                    if (dateTimeElements.Length > 1)
                          a.Add(dateTimeElements[1]);

                }

                #endregion

                #region Get weather data for week

                foreach (var docd in doc.Descendants("forecastday"))
                {
                    string dayDateNonFormat = (string)docd.Descendants("date").FirstOrDefault();
                    var dayDate = dayDateNonFormat.ToDateTime(format: "yyyy-MM-dd");
                    DateWeekDays.Add(dayDate.ToString("dd.MM.yy"));

                    TempWeekDays.Add((string)docd.Descendants("avgtemp_c").FirstOrDefault());
                    PrecipitationWeekDays.Add((string)docd.Descendants("totalprecip_mm").FirstOrDefault());
                }

                #endregion

                tmpList.Add(a);
                tmpList.Add(DateWeekDays);
                tmpList.Add(TempWeekDays);
                tmpList.Add(PrecipitationWeekDays);
                return tmpList;

            }
            return tmpList;
        }


        private static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        

    }
}
