﻿using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion.Ascii;

namespace ZaberMovement
{
    [Description("Take a Zaber device as input and then move that device's lockstep and independent axis")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public class ZaberMoveVelocity : Sink<Tuple<Device, double, double>>
    {
        public override IObservable<Tuple<Device, double, double>> Process(IObservable<Tuple<Device, double, double>> source)
        {
            return source.Do(
                s =>
                {
                    var lockstep = s.Item1.GetLockstep(1);
                    lockstep.MoveVelocity(s.Item2);
                    var axis = s.Item1.GetAxis(3);
                    axis.MoveVelocity(s.Item3);
                }
            );
        }
    }
}