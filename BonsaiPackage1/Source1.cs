using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Zaber.Motion;
using Zaber.Motion.Ascii;
using System.Reactive.Disposables;
using System.Threading.Tasks;


// transform inputs and outputs by type


namespace BonsaiPackage1
{
    // Attribute metadata. Description attributes specify strings that will be included as
    // documentation in the editor
    [Description("connect to an RTSP")]
    [WorkflowElementCategory(ElementCategory.Source)]
          
    public class Source1 : Source<Device>

    {
        // Only one exclusive connection can be made to a hardware camera object. However, many
        // observers may want to connect to the camera. Because of this, the strategy for the
        // node is to create and share the single connection between all observers.
        // When the first observer subscribes to the source, the connection is created. When the
        // last observer unsubscribes, the connection is destroyed.

        private Device[] deviceList;
        readonly IObservable<Device> zaber;

        // This lock is only necessary because of asynchronous creation and destruction of the
        // camera connection. In case we are rapidly creating and destroying camera connections
        // we want to wait until the last connection is destroyed before we create a new one.
        // This lock ensures that.

        readonly object captureLock = new object();


        public Source1()
        {
            // Here we define our observable source. An observable is just a (possibly asynchronous)
            // sequence of events that gets pushed to an arbitrary number of downstream observers.
            //
            // The Observable.Create method makes it easy to create one of these sequences, by simply
            // specifying a function that gets called for each observer. Inside this function, you can
            // send notifications to the observer by calling observer.OnNext(). Also, you need to specify
            // what happens if the observer cancels the subscription at any time (e.g. the workflow is
            // stopped). There are many possible overloads to Observable.Create, but in this case, we
            // are defining the sequence in terms of a Task that starts and stops tied to a particular
            // observer subscription. The cancellationToken variable allows us to be notified when the
            // observer cancelled the subscription.

            zaber = Observable.Create<Device>((observer, cancellationToken) =>
            {
                // Here we simply start the task that will emit the notifications to the observer
                return Task.Factory.StartNew(() =>
                {
                    // We wait until any previous connections are completely disposed.
                    lock (captureLock)
                    {

                        // We create the connection
                        using (var connection = Connection.OpenSerialPort("COM3"))
                        {

                            Library.EnableDeviceDbStore(); //from Zaber, necessary
                            deviceList = connection.DetectDevices();
                            var device = deviceList[0];

                            // TODO want this to be in a sink node
                            Console.WriteLine($"Found {deviceList.Length} devices and address is {device.DeviceAddress}.");
                            var lockstep = device.GetLockstep(1);
                            lockstep.MoveRelative(-1000);
                            /*
                                // Loop until the observer has cancelled the subscription
                                while (!cancellationToken.IsCancellationRequested)
                                {
                                    // Read one image
                                    var image = captureProperties.Capture.QueryFrame();
                                    if (image == null)
                                    {
                                        // If the next image is null, the camera was somehow stopped,
                                        // so we signal the observer that the sequence has ended.
                                        // This mostly never happens, but just to be sure.
                                        observer.OnCompleted();
                                        break;
                                    }
                                    // Otherwise, send a copy of the image to the observer. The reason we
                                    // send a copy is that acquisition of the next frame will overwrite the
                                    // original image; this is a problem if the observer cached the image
                                    // somewhere for future use.
                                    else observer.OnNext(image.Clone());
                                }
                            */

                            // Make sure we reset the capture property to null at the end
                            //finally { captureProperties.Capture = null; }
                        }
                    }
                },
                // These next parameters specify the operation of the Task. We give it the token, so that
                // the task is cancelled in case the observer unsubscribes. We also indicate the Task is long
                // running so that the framework allocates a dedicated thread to it, rather than a worker thread
                // from the thread pool. The last parameter just assigns it the default scheduler.
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            })
            // The next two methods make this source a shared (hot) source. This ensures there is only one observer
            // subscribed to the camera and all notifications are distributed among all other observers.
            // RefCount ensures that the connection is only made when there are actually observers.
            .PublishReconnectable()
            .RefCount();
        }

        // Properties for the property window xxx need to put COM port here later
        //[Description("The index of the camera from which to acquire images.")]
        //public int Index { get; set; }

        // Properties for the property window
       // [Description("Specifies the set of capture properties assigned to the camera.")]
       // public CapturePropertyCollection CaptureProperties
       // {
       //     get { return captureProperties; }
       // }

        // Since we have defined our observable source in the constructor (because we are sharing it), here we
        // just need to return that source.

        public override IObservable<Device> Generate()
        {
            Console.WriteLine("in source1");
            return (IObservable<Device>)Observable.Return(zaber);
        }
    }
}