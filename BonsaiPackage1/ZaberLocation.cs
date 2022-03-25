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
                        var lockstepaxis = s.GetAxis(1);
                        Console.WriteLine($"axis of lockstep is {s.GetAxis(1).GetPosition()}");
                        var locksteplocation = lockstepaxis.GetPosition();
                        var axis = s.GetAxis(3);
                        double axisposition = axis.GetPosition();
                        Console.WriteLine($"axisposition is {axisposition}");
                        return Tuple.Create(locksteplocation, axisposition);
                    });

        } 
    }
}
    