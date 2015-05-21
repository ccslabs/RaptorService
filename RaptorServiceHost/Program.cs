using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RaptorServiceHost
{
    class Program
    {
      public static void Main()
        {
            using (ServiceHost host = new ServiceHost(typeof(RaptorService.RaptorService)))
            {
                RaptorService.RaptorService.CustomEvent += RaptorService_CustomEvent;    
                host.Open();                
                Console.WriteLine("Host Started @ " + DateTime.Now);
                Console.ReadLine();                
            }
        }

        private static void RaptorService_CustomEvent(object sender, RaptorService.CustomEventArgs e)
        {
            Console.WriteLine(e.CallingMethod + "\t" + e.Message);
        }
    }
}
