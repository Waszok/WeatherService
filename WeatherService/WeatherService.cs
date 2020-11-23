using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using WeatherLibrary;
using ExtensionMethods;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace WeatherService
{
    public partial class WeatherService : ServiceBase
    {
        private ServiceHost _host;
        private string _city = null;
        private Timer _timer = new Timer();

        private List<List<string>> a = new List<List<string>>();
        
        public WeatherService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        private List<List<string>> A()
        {
            return a;
        }

        protected override void OnStart(string[] args)
        {
            var instance = new WCFService.WCFService(this.A);
            _host = new ServiceHost(instance, new Uri("net.pipe://localhost/PipeWeatherService"));
            _host.Open();

            WriteToFile("Service is started at " + DateTime.Now);
            _timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            _timer.Interval = 5000; //set the interval at which to raise the Timer.Elapsed event
            _timer.Enabled = true; //raise the Timer.Elapsed event

            GetLocationEvent();

        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            _timer.Enabled = false;

            _host = new ServiceHost(typeof(WCFService.WCFService));
            _host.Close();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now + "City: " + _city);
            a = APIHelper.DownloadData(_city);
        }

        public void WriteToFile(string message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace("/", "_") + ".txt";
            if (!File.Exists(filepath))
            {
                //Create a file to write to
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                // Append to existing file
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public void GetLocationEvent()
        {
            GeoCoordinateWatcher watcher;
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            bool started = watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
            if (!started)
            {
                Console.WriteLine("GeoCoordinateWatcher timed out on start.");
            }
        }

        public void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            string location = WeatherLibrary.APIHelper.GetAddress(e.Position.Location.Latitude.ToString() + "," + e.Position.Location.Longitude.ToString());
            _city = location;
        }

    }
}
