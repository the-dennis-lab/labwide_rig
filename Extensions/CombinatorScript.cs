using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Combinator)]
public class CombinatorScript
{
    public IObservable<int> Process()
    {
        return source;
    }
}
