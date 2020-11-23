using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAppUI;

namespace WeatherLibrary.Tests
{
    [TestClass()]
    public class UtilitiesTests
    {
        [TestMethod()]
        public void JsonReaderTest()
        {
            AppStaticText appTextObjTest = new AppStaticText();

            appTextObjTest.AppNameText = "WEATHER APPLICATION";
            appTextObjTest.AppSloganText = "YOUR WEATHER IN ONE PLACE ...";
            appTextObjTest.LatitudeText = "Latitude: ";
            appTextObjTest.LongitudeText = "Longitude: ";
            appTextObjTest.FeelslikeText = "Feelslike: ";
            appTextObjTest.WindText = "Wind: ";
            appTextObjTest.HumidityText = "Humidity: ";
            appTextObjTest.PressureText = "Pressure: ";
            appTextObjTest.PrecipitationText = "Precipitation: ";
            appTextObjTest.TitleFirstGraphText = "Weekly temperature chart";
            appTextObjTest.TitleSecondGraphText = "Weekly precipitation chart";
            appTextObjTest.FirstGraphScaleText = "Temperature [°C]";
            appTextObjTest.SecondGraphScaleText = "Total precipitation [mm]";
            appTextObjTest.ProvincialCityText = "Select a provincial \n city";
            appTextObjTest.AverageTemperatureText = "Average temperature: ";
            appTextObjTest.AverageWindText = "Average wind: ";
            appTextObjTest.AverageHumidityText = "Average humidity: ";
            appTextObjTest.AveragePressureText = "Average pressure: ";
            appTextObjTest.WebsitesStatementText = "Above infos come from these websites:";





            AppStaticText appTextObj = WeatherLibrary.Utilities.JsonReader<AppStaticText>(@"C:\Users\Kamil\source\repos\WeatherService\textEN.json");

            Assert.AreEqual(appTextObj, appTextObjTest);
            
        }
    }
}