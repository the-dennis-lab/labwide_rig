using System;
using Zaber.Motion;
using Zaber.Motion.Ascii;

// namespace name may differ in your code
namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            Library.EnableDeviceDbStore();

            using (var connection = Connection.OpenSerialPort("COM3")) {
            var deviceList = connection.DetectDevices();
            Console.WriteLine($"Found {deviceList.Length} devices.");
            var device = deviceList[0];

            var axis = device.GetAxis(1);
            axis.Home();
    // The rest of your program goes here
}
        }
    }
}