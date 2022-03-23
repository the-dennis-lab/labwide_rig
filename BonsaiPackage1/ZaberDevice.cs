using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion;
using Zaber.Motion.Ascii;
namespace BonsaiPackage1
{
    [Description("Connect to zaber API via COM and return device")]
    [WorkflowElementCategory(ElementCategory.Source)]
          
    public class ZaberDevice : Source<Device>

    {
        [Description("Serial port that Zaber motion controller is connected to.")]
        public string PortName { get; set; } = "COM3";

        public override IObservable<Device> Generate()
        {
            return Observable.Using(
                () => Connection.OpenSerialPort(PortName),
                connection =>
                {
                    Library.EnableDeviceDbStore(); // From Zaber, necessary
                    var device = connection.DetectDevices()[0];
                    return Observable.Return(device);
                }
            );
        }
    }
}