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
                host.Closing += Host_Closing;
                host.Faulted += Host_Faulted;
                host.UnknownMessageReceived += Host_UnknownMessageReceived;                
                host.Open();
                Console.WriteLine("Host Started @ " + DateTime.Now);
                Console.ReadLine();
            }
        }

        private static void Host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            Console.WriteLine("Host Unknown Message Received  @ " + DateTime.Now);
        }

        private static void Host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Host Faulted @ " + DateTime.Now);
        }

        private static void Host_Closing(object sender, EventArgs e)
        {
            Console.WriteLine("Host Closing @ " + DateTime.Now);
        }

        private static void RaptorService_CustomEvent(object sender, RaptorService.CustomEventArgs e)
        {            
                Console.WriteLine(e.CallingMethod + "\t" + e.Message);            
        }
    }
}
