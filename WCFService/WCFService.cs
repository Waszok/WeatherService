using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WCFService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WCFService : IWCFService
    {
        private Func<List<List<string>>> callback;

        public WCFService(Func<List<List<string>>> callback) //Constructor
        {
            this.callback = callback;
        }

        public List<List<string>> GetDataFromService()
        {
            List<List<string>> output = this.callback();
            return output;
        }
    }
}
