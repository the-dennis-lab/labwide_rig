using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion.Ascii;

namespace BonsaiPackage1
{
    [Description("Take a Zaber device as input and then move that device's lockstep")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public class ZaberMove : Sink<Tuple<Device, int, int>>
    {
        public override IObservable<Tuple<Device, int, int>> Process(IObservable<Tuple<Device, int, int>> source)
        {
            return source.Do(
                s =>
                {
                    var lockstep = s.Item1.GetLockstep(1);
                    lockstep.MoveRelative(s.Item2);
                    var axis = s.Item1.GetAxis(3);
                    axis.MoveRelative(s.Item3);
                }
            );
        }
    }
}