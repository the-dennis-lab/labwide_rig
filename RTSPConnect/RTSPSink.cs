using Bonsai;
using OpenCV.Net;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using RtspClientSharp;
using RtspClientSharp.RawFrames.Video;

namespace RTSPConnect
{
    [Description("Save images from a RTSP source node")]
    [WorkflowElementCategory(ElementCategory.Sink)]



    public class RTSPSink : Sink<RtspClient>
    {
        [Description("frame rate in fps")]
        public int FrameRate { get; set; } = 10;

        [Description("folder where images should be saved")]
        public string SavePath { get; set; } = @"\Users\labadmin\Desktop\";



        public override IObservable<RtspClient> Process(IObservable<RtspClient> source)
        {
            Console.WriteLine(FrameRate);
            int frameIntervalinSec = FrameRate / 60;
            int frameInterval = frameIntervalinSec * 1000;
            int lastTimeSnapshotSaved = Environment.TickCount - frameInterval;
            if (frameInterval == 0)
                frameInterval = 166;
            Console.WriteLine("frame interval is");
            Console.WriteLine(frameInterval);
            return source.Do(
                    s =>
                    {
                        s.FrameReceived += (sender, frame) =>
                        {
                            Console.WriteLine("frame is");
                            Console.WriteLine(frame);
                            /*
                            if (!(frame is RawJpegFrame))
                                Console.WriteLine(frame);
                            return;
                            int ticksNow = Environment.TickCount;
                            if (Math.Abs(ticksNow - lastTimeSnapshotSaved) < frameInterval)
                                Console.WriteLine("math");
                            return;
                            lastTimeSnapshotSaved = ticksNow;
                            */
                            string snapshotName = frame.Timestamp.ToString("O").Replace(":", "_") + ".jpg";
                            string path = SavePath + snapshotName;
                            Console.WriteLine(path);
                            ArraySegment<byte> frameSegment = frame.FrameSegment;

                            using (var stream = File.OpenWrite(path))
                                stream.Write(frameSegment.Array, frameSegment.Offset, frameSegment.Count);

                        };
                        Console.WriteLine("end of s");
                    }); ;

        }
    }
}

