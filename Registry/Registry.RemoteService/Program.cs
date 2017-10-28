using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
using Registry.Data;
using Registry.Data.Services;

namespace Registry.RemoteService
{
  class Program
  {

    static void Main(string[] args)
    {
      RegistryCommon.Container = new UnityContainer();
      RegistryDataRegistration.Register(RegistryCommon.Container);

      using (ServiceHost host = new ServiceHost(typeof(CommunicationService)))
      {
        host.Open();
        Console.WriteLine("The service is ready");
        Console.WriteLine("Press <Enter> to stop the service.");
        Console.ReadLine();
      }
    }
  }
}
