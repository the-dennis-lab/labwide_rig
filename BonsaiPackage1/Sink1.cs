using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Zaber.Motion;
using Zaber.Motion.Ascii;

// transform inputs and outputs by type
using TSource = System.Int32;
using TResult = System.Int32;

namespace BonsaiPackage1
{
    [Description("testing zaber load")]
    //[Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class Sink1 : Sink<TSource>
    {
        public IObservable<int> V { get; private set; }

        private Connection connection;
        private Device[] deviceList;

        // Transform_1() { }

        public override IObservable<TResult> Process(IObservable<TSource> source)
        {
            Library.EnableDeviceDbStore();

            // persistent stuff lives here

            //return Generate()
            connection = Connection.OpenSerialPort("COM3");
            deviceList = connection.DetectDevices();
            var device = deviceList[0];
            Console.WriteLine($"Found {deviceList.Length} devices.");

            return source.Do(input => // source.do is a return 
            {
               
                
                var lockstep = device.GetLockstep(1);

                lockstep.MoveRelative(1000);
                //return lockstep.GetOffsets;

            });
        }
    }

}