using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEventResult
{
    public int EventAmount {  get; private set; }
    public int Misses { get; private set; }
    public int Regulars { get; private set; }
    public int Perfects { get; private set; }

    public QuickTimeEventResult(List<QuickTimeEventElementController.QuickTimeEventResultType> results)
    {
        EventAmount = results.Count;

        Misses = Regulars = Perfects = 0;

        foreach (var result in results)
        {
            switch(result)
            {
                case QuickTimeEventElementController.QuickTimeEventResultType.Miss: Misses++; break;
                case QuickTimeEventElementController.QuickTimeEventResultType.Regular: Regulars++; break;
                case QuickTimeEventElementController.QuickTimeEventResultType.Perfect: Perfects++; break;
            }
        }
    }

    public QuickTimeEventResult()
    {
        EventAmount = Misses = Regulars = Perfects = 0;
    }
}
