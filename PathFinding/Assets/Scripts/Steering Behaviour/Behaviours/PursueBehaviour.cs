using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueBehaviour : Steering
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float maxPrediction = 2f;

    public override SteeringData GetSteering(SteeringBehaviourBase steeringbase)
    {
        SteeringData steering = new SteeringData();

        Vector3 direction = target.position - transform.position;

        float distance = direction.magnitude;

        float speed = GetComponent<Rigidbody>().velocity.magnitude;

        //Prediction es una variable para medir la cercanía y cuanto se ha de mover prediciópn la posición del jugador,
        // en base a su cercanía con él. Cuando más lejos esté más puede ahuevarla, cuanto más cerca, más pequeño se hace
        // el número dejando menos cabida a los errores

        float prediction;

        if (speed <= (distance / maxPrediction))
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        Vector3 predictedTarget = target.position + (target.GetComponent<Rigidbody>().velocity * prediction);

        steering.linear = predictedTarget - transform.position;
        steering.linear.Normalize();
        steering.linear *= steeringbase.maxAcceleration;
        //steering.angular = 0;

        return steering;
    }
}