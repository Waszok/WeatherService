using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WeatherLibrary;
using System.Runtime.CompilerServices;
using System.Device.Location;
using WCFService;
using System.ServiceModel;
using System.Threading;
using HtmlAgilityPack;
using System.Collections.ObjectModel;

using Brushes = System.Windows.Media.Brushes;
using System.Net;
using System.Windows.Media.Imaging;

namespace WeatherAppUI
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private ChannelFactory<IWCFService> _pipeFactory;
        private IWCFService _pipeProxy;
        private bool _isRunning;

        private string _napis;

        private Dictionary<string, string> _webInteriaDict = new Dictionary<string, string>() {
            { "Warszawa", "https://pogoda.interia.pl/prognoza-szczegolowa-warszawa,cId,36917" },
            { "Wrocław", "https://pogoda.interia.pl/prognoza-szczegolowa-wroclaw,cId,39240"},
            { "Szczecin", "https://pogoda.interia.pl/prognoza-szczegolowa-szczecin,cId,34668" },
            { "Rzeszów", "https://pogoda.interia.pl/prognoza-szczegolowa-rzeszow,cId,30389" },
            { "Poznań", "https://pogoda.interia.pl/prognoza-szczegolowa-poznan,cId,27457" },
            { "Olsztyn", "https://pogoda.interia.pl/prognoza-szczegolowa-olsztyn,cId,24210" },
            { "Opole", "https://pogoda.interia.pl/prognoza-szczegolowa-opole,cId,24308" },
            { "Łódź", "https://pogoda.interia.pl/prognoza-szczegolowa-lodz,cId,19059" },
            { "Lublin", "https://pogoda.interia.pl/prognoza-szczegolowa-lublin,cId,19393" },
            { "Kielce", "https://pogoda.interia.pl/prognoza-szczegolowa-kielce,cId,13378" },
            { "Kraków", "https://pogoda.interia.pl/prognoza-szczegolowa-krakow,cId,4970" },
            { "Gdańsk", "https://pogoda.interia.pl/prognoza-szczegolowa-gdansk,cId,8048" },
            { "Katowice", "https://pogoda.interia.pl/prognoza-szczegolowa-katowice,cId,13095" },
            { "Gorzów Wielkopolski", "https://pogoda.interia.pl/prognoza-szczegolowa-gorzow-wielkopolski,cId,9148" },
            { "Zielona Góra", "https://pogoda.interia.pl/prognoza-szczegolowa-zielona-gora,cId,41517" },
            { "Bydgoszcz", "https://pogoda.interia.pl/prognoza-szczegolowa-bydgoszcz,cId,3696" },
            { "Toruń", "https://pogoda.interia.pl/prognoza-szczegolowa-torun,cId,35707" },
            { "Białystok", "https://pogoda.interia.pl/prognoza-szczegolowa-bialystok,cId,1197" } };

        private Dictionary<string, string> _webGisDict = new Dictionary<string, string>() {
            { "Warszawa", "https://www.gismeteo.pl/weather-warsaw-3196/now/" },
            { "Wrocław", "https://www.gismeteo.pl/weather-wroclaw-3200/now/"},
            { "Szczecin", "https://www.gismeteo.pl/weather-szczecin-3101/now/" },
            { "Rzeszów", "https://www.gismeteo.pl/weather-rzeszow-3215/now/" },
            { "Poznań", "https://www.gismeteo.pl/weather-poznan-3194/now/" },
            { "Olsztyn", "https://www.gismeteo.pl/weather-olsztyn-3165/now/" },
            { "Opole", "https://www.gismeteo.pl/weather-opole-3207/now/" },
            { "Łódź", "https://www.gismeteo.pl/weather-lodz-3203/now/" },
            { "Lublin", "https://www.gismeteo.pl/weather-lublin-3205/now/" },
            { "Kielce", "https://www.gismeteo.pl/weather-kielce-3213/now/" },
            { "Kraków", "https://www.gismeteo.pl/weather-krakow-3212/now/" },
            { "Gdańsk", "https://www.gismeteo.pl/weather-gdansk-3046/now/" },
            { "Katowice", "https://www.gismeteo.pl/weather-katowice-3211/now/" },
            { "Gorzów Wielkopolski", "https://www.gismeteo.pl/weather-gorzow-wielkopolski-3192/now/" },
            { "Zielona Góra", "https://www.gismeteo.pl/weather-zielona-gora-3197/now/" },
            { "Bydgoszcz", "https://www.gismeteo.pl/weather-bydgoszcz-3092/now/" },
            { "Toruń", "https://www.gismeteo.pl/weather-torun-3143/now/" },
            { "Białystok", "https://www.gismeteo.pl/weather-bialystok-3187/now/" } };

        private Dictionary<string, string> _webOnetDict = new Dictionary<string, string>() {
            { "Warszawa", "https://pogoda.onet.pl/prognoza-pogody/warszawa-357732" },
            { "Wrocław", "https://pogoda.onet.pl/prognoza-pogody/wroclaw-362450"},
            { "Szczecin", "https://pogoda.onet.pl/prognoza-pogody/szczecin-351892" },
            { "Rzeszów", "https://pogoda.onet.pl/prognoza-pogody/rzeszow-342624" },
            { "Poznań", "https://pogoda.onet.pl/prognoza-pogody/poznan-335979" },
            { "Olsztyn", "https://pogoda.onet.pl/prognoza-pogody/olsztyn-325715" },
            { "Opole", "https://pogoda.onet.pl/prognoza-pogody/opole-325985" },
            { "Łódź", "https://pogoda.onet.pl/prognoza-pogody/lodz-313660" },
            { "Lublin", "https://pogoda.onet.pl/prognoza-pogody/lublin-311624" },
            { "Kielce", "https://pogoda.onet.pl/prognoza-pogody/kielce-300882" },
            { "Kraków", "https://pogoda.onet.pl/prognoza-pogody/krakow-306020" },
            { "Gdańsk", "https://pogoda.onet.pl/prognoza-pogody/gdansk-287788" },
            { "Katowice", "https://pogoda.onet.pl/prognoza-pogody/katowice-299998" },
            { "Gorzów Wielkopolski", "https://pogoda.onet.pl/prognoza-pogody/gorzow-wielkopolski-289581" },
            { "Zielona Góra", "https://pogoda.onet.pl/prognoza-pogody/zielona-gora-368720" },
            { "Bydgoszcz", "https://pogoda.onet.pl/prognoza-pogody/bydgoszcz-276560" },
            { "Toruń", "https://pogoda.onet.pl/prognoza-pogody/torun-355171" },
            { "Białystok", "https://pogoda.onet.pl/prognoza-pogody/bialystok-270085" } };


        private List<double> _xPoints = new List<double>() { 110, 170, 230, 290, 350, 410, 470 };
        private List<double> _yPointsTemp = new List<double>();
        private List<double> _yPointsRain = new List<double>();

        private string _location = null;
        private string _currentTime;
        private List<string> _jsonPaths;
        private AppStaticText _appTextObj;
        private WeatherCurrentData _weatherCurrentData;
        private bool _isPlLang; //0 - ENG, 1 - PL
        private bool _isFahrenheit; //0 - Celsius, 1 - Fahrenheit
        private bool _isFahrenheitFeel;
        public List<string> MyList { get; set; } = new List<string>(){"06.08.19", "06.10.19", "06.01.19",
                                                           "05.08.19", "16.09.19", "30.08.19", "09.12.19" };

        public ObservableCollection<string> WebsiteAddresses { get; set; } = new ObservableCollection<string>();

        public List<string> ProvincialCities { get; set; } = new List<string>() { "Warszawa", "Wrocław", "Szczecin", "Rzeszów", "Poznań", "Olsztyn",
                                                                      "Opole", "Łódź", "Lublin", "Kielce", "Kraków", "Gdańsk", "Katowice",
                                                                      "Gorzów Wielkopolski", "Zielona Góra", "Bydgoszcz", "Toruń", "Białystok"};


        private List<HtmlDocument> _returnedHtmlDocs = new List<HtmlDocument>();

        private ProvincialCityDataModel _provCityDataObj;

        public ProvincialCityDataModel ProvCityDataObj
        {
            get { return _provCityDataObj; }
            private set
            {
                if (_provCityDataObj == value)
                    return;
                _provCityDataObj = value;
                OnPropertyChanged();
            }
        }


        private string _selectedCity;

        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity == value)
                    return;
                _selectedCity = value;
                Task.Factory.StartNew(() =>
                {
                    ProvCityDataObj = GetProvincialCityData().Result;
                });
                InitilizeWebsiteList();
                OnPropertyChanged();
            }
        }

        public string CurrentTime
        {
            get { return _currentTime; }
            private set
            {
                if (_currentTime == value)
                    return;
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsFahrenheit
        {
            get { return _isFahrenheit; }
            private set
            {
                if (_isFahrenheit == value)
                    return;
                _isFahrenheit = value;
                OnPropertyChanged();
            }
        }

        public bool IsFahrenheitFeel
        {
            get { return _isFahrenheitFeel; }
            private set
            {
                if (_isFahrenheitFeel == value)
                    return;
                _isFahrenheitFeel = value;
                OnPropertyChanged();
            }
        }

        public string Napis
        {
            get { return _napis; }
            private set
            {
                if (_napis == value)
                    return;
                _napis = value;
                OnPropertyChanged();
            }
        }


        public AppStaticText AppTextObj
        {
            get { return _appTextObj;  }
            private set
            {
                if (_appTextObj == value)
                    return;
                _appTextObj = value;
                OnPropertyChanged();
            }
        }


        public WeatherCurrentData WeatherCurrentData
        {
            get { return _weatherCurrentData; }
            private set
            {
                if (_weatherCurrentData == value)
                    return;
                _weatherCurrentData = value;
                OnPropertyChanged();
            }
        }


        public string Location
        {
            get { return _location; }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    //WeatherCurrentData = new WeatherCurrentData(Location);
                    OnPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            _pipeFactory = new ChannelFactory<IWCFService>(
                new NetNamedPipeBinding(),
                new EndpointAddress(
            "net.pipe://localhost/PipeWeatherService"));

            _pipeProxy = _pipeFactory.CreateChannel();

            _jsonPaths = new List<string>() { @"C:\Users\Kamil\source\repos\WeatherService\textEN.json",
                                              @"C:\Users\Kamil\source\repos\WeatherService\textPL.json"};

            _isPlLang = false;
            _isRunning = true;
            IsFahrenheit = false;
            IsFahrenheitFeel = false;
            SelectedCity = "Toruń";

            DataContext = this;

            InitializeComponent();

            AppTextObj = Utilities.JsonReader<AppStaticText>(_jsonPaths[0]);

            

            //GetLocationEvent();
            //Location = "Unislaw";
            //WeatherCurrentData = new WeatherCurrentData("Unislaw");

            GetData();
            
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.IsEnabled = true;
            timer.Tick += new EventHandler(TimerTick);


            //-----------------------------------------------------------------------------------

            DrawingOnCanvas.DrawGraphBase(tempGraph, 36, 3);
            DrawingOnCanvas.DrawGraphBase(rainGraph, 34, 2);

            DrawGraphs();


        }

        private async Task<ProvincialCityDataModel> GetProvincialCityData()
        {
            ProvincialCityDataModel provCityDataObj = new ProvincialCityDataModel();

            _returnedHtmlDocs = HtmlScraper.GetHtmlDocs(_webInteriaDict[SelectedCity], _webGisDict[SelectedCity], _webOnetDict[SelectedCity]).Result;

            await Task.Factory.StartNew(() =>
            {
                provCityDataObj.AverageTemperature = HtmlScraper.AverageTemp(_returnedHtmlDocs[0], _returnedHtmlDocs[1], _returnedHtmlDocs[2]);

                provCityDataObj.AverageWind = HtmlScraper.AverageWind(_returnedHtmlDocs[0], _returnedHtmlDocs[1], _returnedHtmlDocs[2]);

                provCityDataObj.AverageHumidity = HtmlScraper.AverageHumidity(_returnedHtmlDocs[0], _returnedHtmlDocs[1], _returnedHtmlDocs[2]);

                provCityDataObj.AveragePressure = HtmlScraper.AveragePressure(_returnedHtmlDocs[0], _returnedHtmlDocs[1], _returnedHtmlDocs[2]);

            });

            return provCityDataObj;
        }

        public void InitilizeWebsiteList()
        {
            WebsiteAddresses.Clear();
            WebsiteAddresses.Add(_webInteriaDict[SelectedCity]);
            WebsiteAddresses.Add(_webGisDict[SelectedCity]);
            WebsiteAddresses.Add(_webOnetDict[SelectedCity]);
        }

        ////////// Normalize word /////////
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

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
            string location = GetAddress(e.Position.Location.Latitude.ToString() + "," + e.Position.Location.Longitude.ToString());
            Location = location;     
        }

        void PrintPosition(double Latitude, double Longitude)
        {
            Console.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
        }


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







        private void DrawGraphs()
        {
            //while (WeatherCurrentData.TempWeekDays.Count == 0)
            //{
                //Draw temperature chart
                if (WeatherCurrentData.TempWeekDays.Count > 0)
                {
                    _yPointsTemp = DrawingOnCanvas.ScaleYCoordinates(WeatherCurrentData.TempWeekDays);
                    DrawingOnCanvas.DrawLineGraph(_xPoints, _yPointsTemp, tempGraph, Brushes.Blue);
                }

                //Draw precipitation chart
                if (WeatherCurrentData.PrecipitationWeekDays.Count > 0)
                {
                    _yPointsRain = DrawingOnCanvas.ScaleRainYCoordinates(WeatherCurrentData.PrecipitationWeekDays);
                    DrawingOnCanvas.DrawBarGraph(_xPoints, _yPointsRain, rainGraph, Brushes.Aquamarine);
                }
            //}
        }


        void TimerTick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        }

        private async Task GetData()
        {
            await Task.Factory.StartNew(() =>
            {
                while (_isRunning)
                {
                    try
                    {
                        List<List<string>> _weatherData = new List<List<string>>();
                        _weatherData = _pipeProxy.GetDataFromService();

                        WeatherCurrentData = new WeatherAppUI.WeatherCurrentData();
                        WeatherCurrentData.TempWeekDays = new ObservableCollection<double>();
                        WeatherCurrentData.PrecipitationWeekDays = new ObservableCollection<double>();

                        string iconUrl = _weatherData[0][0];

                        //WebClient client = new WebClient();
                        //byte[] image = client.DownloadData($"https:{iconUrl}");

                        //BitmapImage bitMap = ToImage(image);

                        //WeatherCurrentData.WeatherIcon = bitMap;

                        WeatherCurrentData.Country = _weatherData[0][1];
                        WeatherCurrentData.Latitude = _weatherData[0][2];
                        WeatherCurrentData.Longitude = _weatherData[0][3];
                        WeatherCurrentData.Temperature = _weatherData[0][4];
                        WeatherCurrentData.TemperatureF = _weatherData[0][5];
                        WeatherCurrentData.WeatherDescription = _weatherData[0][6];
                        WeatherCurrentData.FeelslikeTemperature = _weatherData[0][7];
                        WeatherCurrentData.FeelslikeTemperatureF = _weatherData[0][8];
                        WeatherCurrentData.Wind = _weatherData[0][9];

                        WeatherCurrentData.Humidity = _weatherData[0][10];
                        WeatherCurrentData.Pressure = _weatherData[0][11];
                        WeatherCurrentData.Precipitation = _weatherData[0][12];
                        WeatherCurrentData.WeekDay = _weatherData[0][13];
                        WeatherCurrentData.LocalDate = _weatherData[0][14];
                        WeatherCurrentData.LocalTime = _weatherData[0][15];

                        WeatherCurrentData.DateWeekDays = _weatherData[1];

                        List<string> tempWeek = new List<string>();
                        tempWeek = _weatherData[2];

                        foreach(string el in tempWeek)
                        {
                            WeatherCurrentData.TempWeekDays.Add(double.Parse(el));
                        }

                        List<string> precipitationWeek = new List<string>();
                        precipitationWeek = _weatherData[3];

                        foreach (string el in precipitationWeek)
                        {
                            WeatherCurrentData.PrecipitationWeekDays.Add(double.Parse(el));
                        }
                        Console.WriteLine(WeatherCurrentData.TempWeekDays.Count);

                        DrawGraphs();

                    }
                    catch (TimeoutException timeProblem)
                    {
                        Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                    }
                    catch (FaultException<WCFService.WCFService> fault)
                    {
                        Console.WriteLine("SampleFault fault occurred: {0}", fault.Detail);
                    }
                    catch (CommunicationException commProblem)
                    {
                        Console.WriteLine("There was a communication problem. " + commProblem.Message);
                    }
                    Thread.Sleep(5000);
                }
            });
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

        #region User interface staff

        /// <summary>
        /// Drag application window using left mouse button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception msg)
            {
                Console.WriteLine("{0} DragMove exception caught.", msg.Message);
            }
        }
        
        /// <summary>
        /// Close Application by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _isRunning = false;
                Close();
            }
            catch (Exception msg)
            {
                Console.WriteLine("{0} CloseApp exception caught.", msg.Message);
            }
        }
        
        /// <summary>
        /// Minimize Application by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception msg)
            {
                Console.WriteLine("{0} MinimizeApp exception caught.", msg.Message);
            }
        }
        
        /// <summary>
        /// Restore/maximize application by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestoreApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    restoreButton.ToolTip = "Restore Down";
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    restoreButton.ToolTip = "Maximize";
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine("{0} RestoreApp exception caught.", msg.Message);
            }
        }

        /// <summary>
        /// Change app language into polish language.
        /// Downloading polish text data from Json, assign DataContext
        /// and set proper visibility for borders.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeOnPolishLanguage(object sender, MouseButtonEventArgs e)
        {
            if (!_isPlLang)
            {
                AppTextObj = Utilities.JsonReader<AppStaticText>(_jsonPaths[1]);
                borderEnFlag.Visibility = Visibility.Hidden;
                borderPlFlag.Visibility = Visibility.Visible;

                SetMarginsPlVersion();

                _isPlLang = true;
                languageText.Text = "PL";
            }   
        }
      
        /// <summary>
        /// Change app language into english language. 
        /// Downloading english text data from Json, assign DataContext
        /// and set proper visibility for borders.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeOnEnglishLanguage(object sender, MouseButtonEventArgs e)
        {
            if (_isPlLang)
            {
                AppTextObj = Utilities.JsonReader<AppStaticText>(_jsonPaths[0]);
                //DataContext = new { window = this, AppText = _appTextObj };
                borderPlFlag.Visibility = Visibility.Hidden;
                borderEnFlag.Visibility = Visibility.Visible;

                SetMarginsEnVersion();

                _isPlLang = false;
                languageText.Text = "EN";
            }
        }

        /// <summary>
        /// Set proper margins in polish language version.
        /// </summary>
        private void SetMarginsPlVersion()
        {
            var margin1 = TopStackPanel.Margin;
            margin1.Left = 130;
            TopStackPanel.Margin = margin1;

            var margin2 = rainGraphDescription.Margin;
            margin2.Top = 165;
            rainGraphDescription.Margin = margin2;
        }

        /// <summary>
        /// Set proper margins in english language version.
        /// </summary>
        private void SetMarginsEnVersion()
        {
            var margin1 = TopStackPanel.Margin;
            margin1.Left = 100;
            TopStackPanel.Margin = margin1;

            var margin2 = rainGraphDescription.Margin;
            margin2.Top = 190;
            rainGraphDescription.Margin = margin2;
        }

        /// <summary>
        /// Highlight border around the polish flag. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HighlightPolishFlagEvent(object sender, MouseEventArgs e)
        {
            if (!_isPlLang) borderPlFlag.Visibility = Visibility.Visible;  
        }
        
        /// <summary>
        /// Unhighlight border around the polish flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnhighlightPolishFlagEvent(object sender, MouseEventArgs e)
        {
            if (!_isPlLang) borderPlFlag.Visibility = Visibility.Hidden;
        }
       
        /// <summary>
        /// Highlight border around the english flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HighlightEnglishFlagEvent(object sender, MouseEventArgs e)
        {
            if (_isPlLang) borderEnFlag.Visibility = Visibility.Visible;
        }
       
        /// <summary>
        /// Unhighlight border around the english flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnhighlightEnglishFlagEvent(object sender, MouseEventArgs e)
        {
            if (_isPlLang) borderEnFlag.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Change Temp to Celsius.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeTempUnitCelsius(object sender, RoutedEventArgs e)
        {
            IsFahrenheit = false;
        }

        /// <summary>
        /// Change Temp to Fahrenheit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeTempUnitFahrenheit(object sender, RoutedEventArgs e)
        {
            IsFahrenheit = true;
        }

        /// <summary>
        /// Change Feelslike Temp to Celsius.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeFeelTempUnitCelsius(object sender, RoutedEventArgs e)
        {
            IsFahrenheitFeel = false;
        }

        /// <summary>
        /// Change Feelslike Temp to Fahrenheit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeFeelTempUnitFahrenheit(object sender, RoutedEventArgs e)
        {
            IsFahrenheitFeel = true;
        }

        #endregion
    }
}
