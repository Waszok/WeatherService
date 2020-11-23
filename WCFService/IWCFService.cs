using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WCFService
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        List<List<string>> GetDataFromService(); //This method is called in WPF App
    }
}
