using Bonsai;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using RtspClientSharp;
using RtspClientSharp.RawFrames.Video;

namespace RTSPConnect
{
    [Description("Connect to FLIR dh-390 camera via RTSP protocol and receive image stream")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public class RTSPSource : Source<RtspClient>
    {
        [Description("URI of the stream (can test this in VLC, should show a video stream)")]
        public string URI { get; set; } = "rtsp://10.123.1.42/mjpeg/stream3";

        [Description("username for the URI")]
        public string USERNAME { get; set; } = "admin";

        [Description("password for the URI")]
        public string PASSWORD { get; set; } = "Musmusculus.";


        public override IObservable<RtspClient> Generate()
        {

            var serverUri = new Uri(URI);
            var login = new System.Net.NetworkCredential(USERNAME, PASSWORD);
            var connectionParameters = new ConnectionParameters(serverUri, login);
            var rtspClient = new RtspClient(connectionParameters);

            return Observable.Return(rtspClient).Concat(Observable.Never(rtspClient));
        }
    }

}