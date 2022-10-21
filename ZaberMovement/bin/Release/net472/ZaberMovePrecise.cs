using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion.Ascii;

namespace ZaberMovement
{
    [Description("Take a Zaber device and two doubles as input, and move to that location in Zaber so-called Native units")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public class ZaberMovePrecise : Sink<Tuple<Device, double, double>>
    {
        public override IObservable<Tuple<Device, double, double>> Process(IObservable<Tuple<Device, double, double>> source)
        {
            return source.Do(
                s =>
                {
                    var lockstep = s.Item1.GetLockstep(1);
                    try
                    {
                        lockstep.MoveAbsolute(s.Item2);
                    }
                    catch { }
                    
                    var axis = s.Item1.GetAxis(3);
                    try
                    {
                        axis.MoveAbsolute(s.Item3);
                    }
                    catch { }
                }
            );
        }
    }
}