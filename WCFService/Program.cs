using System;
using System.ServiceModel;

namespace WCFService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(WCFService),
                                      new Uri[]{ new Uri("net.pipe://localhost")}))
            {

                host.AddServiceEndpoint(typeof(IWCFService),
                  new NetNamedPipeBinding(),
                  "PipeWeatherService");

                host.Open();

                Console.WriteLine("Service is available. " +
                  "Press <ENTER> to exit.");
                Console.ReadLine();

                //host.Close();
            }
        }
    }
}
