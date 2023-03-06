using UnityEngine;
using System.Collections;

/// <summary>
/// Makes use (play/stop) an steering agent.
/// </summary>
[System.Serializable]
public class ActionUseSteeringAgent
{
    /// <summary>
    /// Steering behaviour that will be stopped/played.
    /// </summary>
    public Steering behaviour;

    public bool shouldStopInstead = false;
}