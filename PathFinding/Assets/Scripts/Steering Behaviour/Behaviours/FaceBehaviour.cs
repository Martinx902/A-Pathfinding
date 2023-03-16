using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBehaviour : Steering
{
    private Transform target;

    private TargetController controller;

    private void Awake()
    {
        controller = GetComponent<TargetController>();
    }

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        if (controller.GetTarget() != null)
        {
            target = controller.GetTarget();

            Vector3 direction = target.position - transform.position;

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            steering.angular = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, steeringbase.maxAngularAcceleration * Time.deltaTime);

            //steering.linear = Vector3.zero;
        }

        return steering;
    }
}