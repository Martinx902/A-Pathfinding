using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignBehaviour : Steering
{
    private Transform[] targets;
    [SerializeField] private float alignDistance = 8f;

    // Start is called before the first frame update
    private void Start()
    {
        SteeringBehaviourBase[] agents = FindObjectsOfType<SteeringBehaviourBase>();

        targets = new Transform[agents.Length - 1];

        int count = 0;

        foreach (SteeringBehaviourBase agent in agents)
        {
            if (agent.gameObject != gameObject)
            {
                targets[count] = agent.transform;
                count++;
            }
        }
    }

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        steering.linear = Vector3.zero;

        int count = 0;

        foreach (Transform target in targets)
        {
            Vector3 targetDir = target.position - transform.position;

            if (targetDir.magnitude < alignDistance)
            {
                steering.linear += target.GetComponent<Rigidbody>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            steering.linear = steering.linear / count;

            if (steering.linear.magnitude > steeringbase.maxAcceleration)
            {
                steering.linear = steering.linear.normalized * steeringbase.maxAcceleration;
            }
        }

        return steering;
    }
}