using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using TSource = System.Tuple<Zaber.Motion.Ascii.Device, int>;
using TResult = System.Tuple<double, double>;


namespace ZaberMovement
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
                        var lockstepaxis = s.Item1.GetAxis(1);
                        Console.WriteLine($"axis of lockstep is {s.Item1.GetAxis(1).GetPosition()}");
                        Zaber.Motion.Ascii.Axis axis = s.Item1.GetAxis(3);
                        double axisposition = axis.GetPosition();
                        Console.WriteLine($"axisposition is {axisposition}");
                        return Tuple.Create(lockstepaxis.GetPosition(), axisposition);
                    });

        } 
    }
}
    