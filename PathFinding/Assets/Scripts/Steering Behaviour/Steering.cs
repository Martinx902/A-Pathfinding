using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour
{
    [SerializeField] private float weight = 2f;

    public abstract SteeringData GetSteering(SteeringBehaviourBase steeringbase);

    public float GetWeight() => weight;
}