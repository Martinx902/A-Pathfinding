using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSetState : IAction
{
    [SerializeField]
    private States newState;

    private StateController stateController;

    private void Awake()
    {
        stateController = GetComponentInParent<StateController>();
    }

    public override void Act()
    {
        if (stateController == null)
        {
            Debug.Log("No state controller encontrado en el Action Set State Script");
            return;
        }

        stateController.SetState(newState);
    }
}