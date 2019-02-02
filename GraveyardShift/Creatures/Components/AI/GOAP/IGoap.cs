using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    public interface IGoap
    {
        Dictionary<string, object> GetWorldState();
        Dictionary<string, object> GetGoalsState();
        List<GOAP_action> GetActions();
    }
}
