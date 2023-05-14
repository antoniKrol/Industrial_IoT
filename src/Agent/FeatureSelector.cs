using Microsoft.Azure.Devices.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class FeatureSelector
    {
        public static void PrintMenu()
        {
            System.Console.WriteLine(@"
1 - Start App
2 - Invoke Direct Method
0 - Exit
");
        }

        public static async Task Execute(int feature,Industrial_IoT.Lib.IoTHubManager manager)
        {
            switch (feature)
            {
                case 1:
                    {
                        new OpcSimConnector(manager).connectAndDisplay();
                    }
                    break;
                case 2:
                    {                    
                        System.Console.WriteLine("\n Type device Id");
                        string deviceId = System.Console.ReadLine() ?? string.Empty;

                        System.Console.WriteLine("\n Type method 0 - Emergency Stop 1 - Reset Error Status");
                       string methodName = System.Console.ReadLine() ?? string.Empty;

                       OpcClientManager opc = new OpcClientManager();
                        opc.InvokeDirectMethod(deviceId, methodName);
                    }
                    break;
                default:
                    { break; }

            }
        }

        internal static int ReadInput()
        {
            var keyPressed = System.Console.ReadKey();
            var isParsed = int.TryParse(keyPressed.KeyChar.ToString(), out var value);
            
            return isParsed ? value : -1;
        }
    }
}
