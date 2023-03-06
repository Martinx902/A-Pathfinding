using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChooseSteering : IAction
{
    [SerializeField]
    private List<ActionUseSteeringAgent> behaviours;

    public override void Act()
    {
        foreach (ActionUseSteeringAgent behaviour in behaviours)
        {
            if (behaviour.shouldStopInstead)
            {
                behaviour.behaviour.Stop();
            }
            else
            {
                behaviour.behaviour.Play();
            }
        }
    }
}
