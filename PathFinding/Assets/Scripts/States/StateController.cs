using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateController : MonoBehaviour
{
    public List<EntityState> states = new List<EntityState>();

    private States actualState;

    public States GetActualState() => actualState;

    public void SetState(States newState)
    {
        actualState = newState;
        Debug.Log("New State recieved: " + newState);
        PlaySteerings();
    }

    private void PlaySteerings()
    {
        EntityState entity = states.FirstOrDefault(x => x.entityState == actualState);

        entity.SetSteerings();
    }
}