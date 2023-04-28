using Microsoft.Azure.Devices.Client;

namespace DeviceSdkAgent.Device
{
    public class VirtualDevice
    {
        private readonly DeviceClient deviceClient;

        public VirtualDevice(DeviceClient deviceClient)
        {
            this.deviceClient = deviceClient;
        }
    }
}