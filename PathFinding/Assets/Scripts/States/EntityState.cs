using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityState
{
    public States entityState;

    public List<ActionUseSteeringAgent> steeringBehaviours = new List<ActionUseSteeringAgent>();

    public void SetSteerings()
    {
        foreach (ActionUseSteeringAgent behaviour in steeringBehaviours)
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

public enum States
{
    Flee,
    Wander,
    AvoidObstacles,
    Pursue,
    LookFor
}