using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Zaber.Motion;
using Zaber.Motion.Ascii;

// transform inputs and outputs by type
using TSource = Zaber.Motion.Ascii.Device;
using TResult = Zaber.Motion.Ascii.Device;

namespace BonsaiPackage1
{
    [Description("attempting to use a zaber device as input and then move that device's lockstep")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class Transform1 : Transform<TSource, TResult>
    {

        public void Process(TSource source)
        {
            var lockstep = source.GetLockstep(1);
            Console.WriteLine($"in transform! {lockstep.GetOffsets()}");

        }
        public override IObservable<Device> Process(IObservable<Device> source)
        {
            return source.Do(Process);
        }
        }
    }