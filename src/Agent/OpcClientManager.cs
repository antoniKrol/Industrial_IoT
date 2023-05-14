using Microsoft.Extensions.Configuration;
using Opc.UaFx.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Console
{
    internal class OpcClientManager
    {
        private readonly string opcClientConnetionString = "";
        private readonly OpcClient client;
        public OpcClientManager() {
            IConfiguration configuration = AppConfiguration.GetConfiguration();
            opcClientConnetionString = configuration["opcClientConnetionString"];
            this.client = new OpcClient(opcClientConnetionString);
            try
            {
                client.Connect();
            }catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public void InvokeDirectMethod(string deviceId,string input)
        {
            int inputInt = 0;
            int.TryParse(input, out inputInt);
           switch (inputInt)
            {
                case 0:
                    {
                        client.CallMethod($"ns=2;s={deviceId}", $"ns=2;s={deviceId}/EmergencyStop");
                    }
                    break;

                case 1:
                    {
                        client.CallMethod($"ns=2;s={deviceId}", $"ns=2;s={deviceId}/ResetErrorStatus");
                    }
                    break;

                default:
                    {
                        System.Console.WriteLine("Error");
                    }
                    break;
            }
        }
    }
}
