using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateController : MonoBehaviour
{
    public List<EntityState> states = new List<EntityState>();

    private States actualState;

    [SerializeField]
    private bool DebugEnable = false;

    public States GetActualState() => actualState;

    public void SetState(States newState)
    {
        if (actualState == newState)
            return;

        actualState = newState;

        if (DebugEnable)
        {
            Debug.Log("New State recieved: " + newState + " on: " + this.gameObject.name);
        }

        PlaySteerings();
    }

    private void PlaySteerings()
    {
        EntityState entity = states.FirstOrDefault(x => x.entityState == actualState);

        entity.SetSteerings();
    }
}