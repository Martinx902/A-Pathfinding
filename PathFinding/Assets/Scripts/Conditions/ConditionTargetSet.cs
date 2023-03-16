using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTargetSet : ICondition
{
    [SerializeField]
    private TargetController targetController;

    public override bool Test()
    {
        if (targetController.GetTarget() == null)
            return false;
        else
            return true;
    }
}