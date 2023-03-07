using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionIsZombieOrNot : ICondition
{
    public override bool Test()
    {
        if (gameObject.tag == "Zombie")
            return true;
        else
            return false;
    }
}
