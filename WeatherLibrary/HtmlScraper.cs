using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherLibrary
{
    public static class HtmlScraper
    {
        /// <summary>
        /// Return list of Html Documents according to the given websites.
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <param name="url3"></param>
        /// <returns></returns>
        public async static Task<List<HtmlDocument>> GetHtmlDocs(string url1, string url2, string url3)
        {
            List<HtmlDocument> htmldocs = new List<HtmlDocument>();
            await Task.Factory.StartNew(() =>
            {
                htmldocs.Add(GetHtml(url1));
                htmldocs.Add(GetHtml(url2));
                htmldocs.Add(GetHtml(url3));

            });
            return htmldocs;
        }

        /// <summary>
        /// Get a HTML Document for a given website.
        /// </summary>
        /// <param name="websiteAdrress"></param>
        /// <returns></returns>
        public static HtmlDocument GetHtml(string websiteAdrress)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            try
            {
                HtmlDocument htmlDoc = htmlWeb.Load(websiteAdrress);
                return htmlDoc;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
                return null;
            }
        }


        #region Interia website

        private static double? GetTemperatureInteria(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string tempStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value.Split().Contains("weather-currently-temp-strict")).SingleOrDefault().InnerText;

                    tempStr = tempStr.Replace(",", ".");
                    tempStr = Regex.Split(tempStr, @"[^0-9\.]+")
                                              .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(tempStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetWindInteria(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string windStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "weather-currently-details-item wind").FirstOrDefault()
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "weather-currently-details-value").FirstOrDefault().InnerText;

                    windStr = windStr.Replace(",", ".");
                    windStr = Regex.Split(windStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(windStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetHumidityInteria(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string humidityStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "entry-humidity-wrap").FirstOrDefault().InnerText;

                    humidityStr = humidityStr.Replace(",", ".");
                    humidityStr = Regex.Split(humidityStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(humidityStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetPressureInteria(HtmlDocument htmlDoc)
        {
            string pressureStr = null;
            if (htmlDoc != null)
            {
                try
                {
                    pressureStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && (x.Attributes["class"].Value == "weather-currently-details-value steady"
                                                                        || x.Attributes["class"].Value == "weather-currently-details-value rising"
                                                                        || x.Attributes["class"].Value == "weather-currently-details-value decreasing")).FirstOrDefault().InnerText;

                    pressureStr = pressureStr.Replace(",", ".");
                    pressureStr = Regex.Split(pressureStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();

                    return double.Parse(pressureStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        #endregion


        #region GISMETEO website

        private static double? GetTemperatureGM(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string tempStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "unit unit_temperature_c").FirstOrDefault().InnerText;
                    tempStr = tempStr.Replace(",", ".");
                    tempStr = Regex.Split(tempStr, @"[^0-9\.]+")
                                              .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(tempStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetWindGM(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string windStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "unit unit_wind_km_h").FirstOrDefault()
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "nowinfo__value").FirstOrDefault().InnerText;

                    string[] windStrArray = windStr.Split('-');
                    windStr = windStrArray[0];

                    windStr = windStr.Replace(",", ".");
                    windStr = Regex.Split(windStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(windStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetHumidityGM(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string humidityStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "nowinfo__item nowinfo__item_humidity").FirstOrDefault()
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "nowinfo__block").FirstOrDefault()
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "nowinfo__value").FirstOrDefault().InnerText;

                    humidityStr = humidityStr.Replace(",", ".");
                    humidityStr = Regex.Split(humidityStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(humidityStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetPressureGM(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string pressureStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "unit unit_pressure_h_pa").FirstOrDefault()
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "nowinfo__value").FirstOrDefault().InnerText;
                    pressureStr = pressureStr.Replace(",", ".");
                    pressureStr = Regex.Split(pressureStr, @"[^0-9\.]+")
                                              .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(pressureStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }


        #endregion


        #region ONET website
        private static double? GetTemperatureOnet(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string tempStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "temp").FirstOrDefault().InnerText;
                    tempStr = tempStr.Replace(",", ".");
                    tempStr = Regex.Split(tempStr, @"[^0-9\.]+")
                                              .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(tempStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetWindOnet(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string windStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "weatherParams").FirstOrDefault()
                                                                        .Descendants("li").ElementAt(1)
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "restParamValue").FirstOrDefault().InnerText;

                    string[] windStrArray = windStr.Split('-');
                    windStr = windStrArray[0];

                    windStr = windStr.Replace(",", ".");
                    windStr = Regex.Split(windStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(windStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetHumidityOnet(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {
                try
                {
                    string humidityStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "weatherParams").FirstOrDefault()
                                                                        .Descendants("li").ElementAt(4)
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "restParamValue").FirstOrDefault().InnerText;


                    humidityStr = humidityStr.Replace(",", ".");
                    humidityStr = Regex.Split(humidityStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(humidityStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        private static double? GetPressureOnet(HtmlDocument htmlDoc)
        {
            if (htmlDoc != null)
            {

                try
                {
                    string humidityStr = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "weatherParams").FirstOrDefault()
                                                                        .Descendants("li").ElementAt(2)
                                                                        .ChildNodes.Where(x => x.Attributes.Contains("class")
                                                                        && x.Attributes["class"].Value == "restParamValue").FirstOrDefault().InnerText;


                    humidityStr = humidityStr.Replace(",", ".");
                    humidityStr = Regex.Split(humidityStr, @"[^0-9\.]+")
                                          .Where(c => c != "." && c.Trim() != "").SingleOrDefault();
                    return double.Parse(humidityStr);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Error message {e}");
                    return null;
                }
            }
            return null;
        }

        #endregion


        /// <summary>
        /// Get average temperature.
        /// </summary>
        /// <param name="htmlDoc1"></param>
        /// <param name="htmlDoc2"></param>
        /// <param name="htmlDoc3"></param>
        /// <returns></returns>
        public static string AverageTemp(HtmlDocument htmlDoc1, HtmlDocument htmlDoc2, HtmlDocument htmlDoc3)
        {
            int counter = 0;
            double sum = 0;

            double? temp1 = GetTemperatureInteria(htmlDoc1);
            double? temp2 = GetTemperatureGM(htmlDoc2);
            double? temp3 = GetTemperatureOnet(htmlDoc3);

            if (temp1 != null)
            {
                double temp = (double)temp1;
                sum += temp;
                counter++;
            }
            if (temp2 != null)
            {
                double temp = (double)temp2;
                sum += temp;
                counter++;
            }
            if (temp3 != null)
            {
                double temp = (double)temp3;
                sum += temp;
                counter++;
                sum = Math.Round(sum / counter, 1);
            }
            if (counter > 0) return sum.ToString();
            else return "-";
        }

        /// <summary>
        /// Get average wind.
        /// </summary>
        /// <param name="htmlDoc1"></param>
        /// <param name="htmlDoc2"></param>
        /// <param name="htmlDoc3"></param>
        /// <returns></returns>
        public static string AverageWind(HtmlDocument htmlDoc1, HtmlDocument htmlDoc2, HtmlDocument htmlDoc3)
        {
            int counter = 0;
            double sum = 0;

            double? wind1 = GetWindInteria(htmlDoc1);
            double? wind2 = GetWindGM(htmlDoc2);
            double? wind3 = GetWindOnet(htmlDoc3);

            if (wind1 != null)
            {
                double wind = (double)wind1;
                sum += wind;
                counter++;
            }
            if (wind2 != null)
            {
                double wind = (double)wind2;
                sum += wind;
                counter++;
            }
            if (wind3 != null)
            {
                double wind = (double)wind3;
                sum += wind;
                counter++;
                sum = Math.Round(sum / counter, 1);
            }
            if (counter > 0) return sum.ToString();
            else return "-";
        }

        /// <summary>
        /// Get average humidity.
        /// </summary>
        /// <param name="htmlDoc1"></param>
        /// <param name="htmlDoc2"></param>
        /// <param name="htmlDoc3"></param>
        /// <returns></returns>
        public static string AverageHumidity(HtmlDocument htmlDoc1, HtmlDocument htmlDoc2, HtmlDocument htmlDoc3)
        {
            int counter = 0;
            double sum = 0;

            double? humidity1 = GetHumidityInteria(htmlDoc1);
            double? humidity2 = GetHumidityGM(htmlDoc2);
            double? humidity3 = GetHumidityOnet(htmlDoc3);

            if (humidity1 != null)
            {
                double humidity = (double)humidity1;
                sum += humidity;
                counter++;
            }
            if (humidity2 != null)
            {
                double humidity = (double)humidity2;
                sum += humidity;
                counter++;
            }
            if (humidity3 != null)
            {
                double humidity = (double)humidity3;
                sum += humidity;
                counter++;
                sum = Math.Round(sum / counter, 1);
            }
            if (counter > 0) return sum.ToString();
            else return "-";
        }

        /// <summary>
        /// Get average pressure.
        /// </summary>
        /// <param name="htmlDoc1"></param>
        /// <param name="htmlDoc2"></param>
        /// <param name="htmlDoc3"></param>
        /// <returns></returns>
        public static string AveragePressure(HtmlDocument htmlDoc1, HtmlDocument htmlDoc2, HtmlDocument htmlDoc3)
        {
            int counter = 0;
            double sum = 0;

            double? pressure1 = GetPressureInteria(htmlDoc1);
            double? pressure2 = GetPressureGM(htmlDoc2);
            double? pressure3 = GetPressureOnet(htmlDoc3);

            if (pressure1 != null)
            {
                double pressure = (double)pressure1;
                sum += pressure;
                counter++;
            }
            if (pressure2 != null)
            {
                double pressure = (double)pressure2;
                sum += pressure;
                counter++;
            }
            if (pressure3 != null)
            {
                double pressure = (double)pressure3;
                sum += pressure;
                counter++;
                sum = Math.Round(sum / counter, 1);
            }
            if (counter > 0) return sum.ToString();
            else return "-";
        }

    }
}
