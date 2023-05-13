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
1 - C2D
2 - Display Connected Devices (0 - to exit)
3 - Device Twin
0 - Exit
");
        }

        public static async Task Execute(int feature,Industrial_IoT.Lib.IoTHubManager manager)
        {
            switch (feature)
            {
                case 1:
                    {
                        System.Console.WriteLine("\n Type your message (enter to confirm)");
                        string messageText = System.Console.ReadLine() ?? string.Empty;

                        System.Console.WriteLine("\n Type your device ID (enter to confirm)");
                        string deviceId = System.Console.ReadLine() ?? string.Empty;

                        await manager.SendMessage(messageText, deviceId);
                    }
                    break;
                case 2:
                    {
                       new OpcSimConnector(manager).connectAndDisplay();
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
