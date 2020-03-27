using System.Linq;
using System.Threading.Tasks;
using System;
using DEOS.BACnet;

namespace bc_demo1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello BACnet!");

            // Hier wird das Netzwerk an eine IP gebunden
            var network = new DEOS.BACnet.Network("172.20.47.22");

            // Anonyme Event Registrierung
            // Geräte werden dem Netwerk hinzugefügt, wenn ein IAM empfangen wurde
            network.OnDeviceAdd += (sender, device) => {
                // GetObectName wird hier Syncron durchgeführt
                // Events werden jedoch generell 'gecacht'
                Console.WriteLine($"BACnet Device found {device} Objectname: {device.GetObjectName()}");
            };

            // Start des Eigentlichen BACnet Netzwerkes
            network.Start();

            while (true)
            {
                await Task.Delay(5000);
                Console.WriteLine($"{DateTime.Now} [INFO] MainLoop run");


                var meineControllerInstanznummer = 47163;
                // Mal gucken ob 'MEIN' Controller im Netzwerk vorhanden
                if(network.BacnetDevices.Any(d => d.Instance == meineControllerInstanznummer))
                {
                    await DoSomething(network.BacnetDevices.First(d => d.Instance == meineControllerInstanznummer));

                    break;
                }
            }

            Console.WriteLine($"Proram shutdown");
            
            network.Stop();
            
            Console.WriteLine($"by by ...");
        }

        private static async Task DoSomething(Device device)
        {
            var objectList = await device.GetBacnetObjectListAsync();
            foreach (var bacnetObject in objectList)
            {
                string[] onds = bacnetObject.GetObjectNameAndDescription();
                Console.WriteLine($"ObjectType: {bacnetObject.ObjectId.Type} Name: {onds.ElementAtOrDefault(0)} Beschreibung: {onds[1]}");
            }
        }
    }
}
