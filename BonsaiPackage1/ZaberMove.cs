using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion.Ascii;

namespace BonsaiPackage1
{
    [Description("Take a Zaber device as input and then move that device's lockstep")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public class ZaberMove : Sink<Tuple<Device, double>>
    {
        public override IObservable<Tuple<Device, double>> Process(IObservable<Tuple<Device, double>> source)
        {
            return source.Do(
                s =>
                {
                    var lockstep = s.Item1.GetLockstep(1);
                    lockstep.MoveRelative(s.Item2);
                }
            );
        }
    }
}