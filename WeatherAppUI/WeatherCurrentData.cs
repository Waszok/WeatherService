using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using ExtensionMethods;
using System;
using System.Collections.ObjectModel;

namespace WeatherAppUI
{
    public class WeatherCurrentData
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string LocalDate { get; set; }
        public string WeekDay { get; set; }
        public string LocalTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Temperature { get; set; }
        public string TemperatureF { get; set; }
        public BitmapImage WeatherIcon { get; set; }
        public string WeatherDescription { get; set; }
        public string FeelslikeTemperature { get; set; }
        public string FeelslikeTemperatureF { get; set; }
        public string Wind { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string Precipitation { get; set; }

        public List<string> DateWeekDays { get; set; }
        public ObservableCollection<double> TempWeekDays { get; set; }
        public ObservableCollection<double> PrecipitationWeekDays { get; set; }
    }/*
        private string _city = null;
        private string _url = null;

        public WeatherCurrentData(string city)
        {
            _city = city;
            DateWeekDays = new List<string>();
            TempWeekDays = new ObservableCollection<double>();
            PrecipitationWeekDays = new ObservableCollection<double>();

            DownloadData();
        }

        private async Task GetDataAsynchronously()
        {
            await Task.Run(() => { DownloadData(); });
        }

        private void SetApiAddress()
        {
            //_city = "Unislaw";
            if(!string.IsNullOrEmpty(_city))
                _url = $"https://api-cdn.apixu.com/v1/forecast.xml?key=e39bb7cee08c4064806165158191708&q={_city}&days=7";
        }

        private void DownloadData()
        {
            SetApiAddress();
            if (!string.IsNullOrEmpty(_url))
            {
                XDocument doc = XDocument.Load(_url);
                
                
                #region Get weather icon image

                string iconUrl = (string)doc.Descendants("icon").FirstOrDefault();

                WebClient client = new WebClient();
                byte[] image = client.DownloadData($"https:{iconUrl}");

                BitmapImage bitMap = ToImage(image);

                WeatherIcon = bitMap;

                #endregion
                
                #region Get weather data for current day 

                Country = (string)doc.Descendants("country").FirstOrDefault();
                Latitude = (string)doc.Descendants("lat").FirstOrDefault();
                Longitude = (string)doc.Descendants("lon").FirstOrDefault();
                Temperature = (string)doc.Descendants("temp_c").FirstOrDefault();
                TemperatureF = (string)doc.Descendants("temp_f").FirstOrDefault();
                WeatherDescription = (string)doc.Descendants("text").FirstOrDefault();
                FeelslikeTemperature = (string)doc.Descendants("feelslike_c").FirstOrDefault();
                FeelslikeTemperatureF = (string)doc.Descendants("feelslike_f").FirstOrDefault();
                Wind = (string)doc.Descendants("wind_kph").FirstOrDefault();
                Humidity = (string)doc.Descendants("humidity").FirstOrDefault();
                Pressure = (string)doc.Descendants("pressure_mb").FirstOrDefault();
                Precipitation = (string)doc.Descendants("precip_mm").FirstOrDefault();

                string localTime = (string)doc.Descendants("localtime").FirstOrDefault();
                string[] dateTimeElements = localTime.Split();

                if(dateTimeElements.Length > 0)
                {
                    string localDate = dateTimeElements[0];
                    var date = localDate.ToDateTime(format: "yyyy-MM-dd");

                    WeekDay = $"{date.DayOfWeek.ToString()} ";
                    LocalDate = $"{date.ToString("dd.MM.yyyy")}, ";

                    if (dateTimeElements.Length > 1)
                        LocalTime = dateTimeElements[1];
                    
                }

                #endregion
                
                #region Get weather data for week

                foreach(var docd in doc.Descendants("forecastday"))
                {
                    //var result = Task.Run(async () => { return await GetData(docd); }).Result;
                    string dayDateNonFormat = (string)docd.Descendants("date").FirstOrDefault();
                    var dayDate = dayDateNonFormat.ToDateTime(format: "yyyy-MM-dd");
                    DateWeekDays.Add(dayDate.ToString("dd.MM.yy"));

                    TempWeekDays.Add(double.Parse((string)docd.Descendants("avgtemp_c").FirstOrDefault()));
                    PrecipitationWeekDays.Add(double.Parse((string)docd.Descendants("totalprecip_mm").FirstOrDefault()));
                }

                #endregion

            }
        }

        private async Task<string> GetData(XElement docd)
        {
            return (string)docd.Descendants("date").FirstOrDefault();
        }

        private BitmapImage ToImage(byte[] array)
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
       
    }*/
}
