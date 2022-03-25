using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Zaber.Motion.Ascii;
using TSource = Zaber.Motion.Ascii.Device;
using TResult = System.Tuple<double, double>;


namespace BonsaiPackage1
{
    [Description("attempting to use a zaber device as input and return the location of the stage in x and y")]
    [WorkflowElementCategory(ElementCategory.Transform)]

    public class ZaberLocation : Transform<TSource, TResult>
    {
        
        public override IObservable<TResult> Process(IObservable<TSource> source)
        {

            return source.Select(
                s =>
                    {
                        var lockstep = s.GetLockstep(1);
                        double lockstepoffset = lockstep.GetOffsets()[0];
                        Console.WriteLine($"lockstepoffset is {lockstepoffset}");
                        var axis = s.GetAxis(3);
                        double axisposition = axis.GetPosition();
                        Console.WriteLine($"axisposition is {axisposition}");
                        return Tuple.Create(lockstepoffset, axisposition);
                    });

        } 
    }
}
    