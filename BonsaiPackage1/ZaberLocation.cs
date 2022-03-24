using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion;
using Zaber.Motion.Ascii;
using TSource = Zaber.Motion.Ascii.Device;
using TResult = System.Tuple<double, double>;


namespace BonsaiPackage1
{
    [Description("attempting to use a zaber device as input and return the location of the stage in x and y")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ZaberLocation : Transform<TSource, TResult>
    {
        public override IObservable<TResult> Process(TSource source)
        {
            // fucking around with this - the variables here are useful
            // but the implementation is obviously wrong - had to stop mid-destroying it :) 
            // 
            var lockstep = source.GetLockstep(1);
            var lockstepoffset = lockstep.GetOffsets();
            var axis = source.GetAxis(3);
            var axisposition = axis.GetPosition();
            var tuptoreturn = Tuple<T1,T2>(double lockstepoffset, double axisposition);

            return Observable.Return(tuptoreturn);
        }

    }
}
