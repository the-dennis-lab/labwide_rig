using Bonsai;
using Bonsai.Vision;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using RtspClientSharp;
using System.Threading;
using CommandLine;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using RtspClientSharp.RawFrames.Video;


namespace RTSPConnect
{
    [Description("Connect to FLIR dh-390 camera via RTSP protocol and receive image stream")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public class RTSPSource : Source<IplImage>
    {
        //have user set some variables that we will use

        [Description("URI of the stream (can test this in VLC, should show a video stream)")]
        public string URI { get; set; } = "rtsp://10.123.1.42/mjpeg/stream3";

        [Description("username for the URI")]
        public string Username { get; set; } = "admin";

        [Description("password for the URI")]
        public string Password { get; set; } = "Musmusculus.";

        [Description("frame rate in fps")]
        public int FrameRate { get; set; } = 10;

        [Description("folder where images should be saved")]
        public string SavePath { get; set; } = @"\Users\labadmin\Desktop\";

        IObservable<IplImage> source;
        readonly object captureLock = new object();
        public RTSPSource()
        // define the source - push notifications to each observer but only
        // when there's a subscribed observer

        {
            source = Observable.Create<IplImage>((observer, cancellationToken) =>
            { //start the task
                return Task.Factory.StartNew(() =>
                {
                    // wait and make sure previous connections are completed
                    lock (captureLock)
                    {
                        // set some variables for making the connection
                        var serverUri = new Uri(URI);
                        var login = new System.Net.NetworkCredential(Username, Password);
                        var connectionParameters = new ConnectionParameters(serverUri, login);

                        Console.WriteLine(FrameRate);
                        int frameIntervalinSec = FrameRate / 60;
                        int frameInterval = frameIntervalinSec * 1000;
                        int lastTimeSnapshotSaved = Environment.TickCount - frameInterval;

                        if (frameInterval == 0)
                            frameInterval = 166;
                        Console.WriteLine("frame interval is");
                        Console.WriteLine(frameInterval);

                        // in camera capture, this is 
                        // using (var capture = Capture.CreateCameracapture(Index))
                        using (var rtspClient = new RtspClient(connectionParameters))
                        {
                            Console.WriteLine("rtspClient was made");
                            rtspClient.FrameReceived += (sender, frame) =>
                            {
                                if (!(frame is RawJpegFrame))
                                    Console.WriteLine("frame is not rawjpegframe");
                                    return;
                                int ticksNow = Environment.TickCount;

                                if (Math.Abs(ticksNow - lastTimeSnapshotSaved) < frameInterval)
                                    Console.WriteLine("frame interval if statement");
                                    return;

                                lastTimeSnapshotSaved = ticksNow;
                                ArraySegment<byte> frameSegment = frame.FrameSegment;
                              

                                observer.OnNext();
                            };

                        }
                    }
                        /*
                            // loop until the observer cancels the subscription
                            while (!cancellationToken.IsCancellationRequested)
                            {
                                Console.WriteLine("in while loop, cancellation is not requested");
                                try
                                {
                                    rtspClient.ConnectAsync(cancellationToken);
                                }
                                catch (OperationCanceledException)
                                {
                                    return;
                                }
                                catch (RtspClientException e)
                                {
                                    Task.Delay(10, cancellationToken);
                                    continue;
                                }

                                Console.WriteLine("Connected");

                                try
                                {
                                    rtspClient.ReceiveAsync(cancellationToken);
                                }
                                catch (OperationCanceledException)
                                { return; }
                                catch (RtspClientException e)
                                {
                                    Task.Delay(10, cancellationToken);
                                }

                                //read an image
                                // var image = queryframe
                                //observer.OnNext(image.Clone());


                            }

                        }
                    }

                },
                // from camera capture: task should be cancelled (cancellation Token) if the obserer unsubscribes
                // long running means the framework allocates a dedicated thread from the threadpool
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            })
            // make this a shared/hot source
            // and only make the connection when there are observers
            .PublishReconnectable()
            .RefCount();
        }

        public override IObservable<IplImage> Generate()
        { return source;
        }
    }
}

