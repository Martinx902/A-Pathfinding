using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SteeringBehaviourBase : MonoBehaviour
{
    private Rigidbody rb;
    private Steering[] steerings;
    public float maxAcceleration = 10f;
    public float maxAngularAcceleration = 3f;
    public float drag = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        steerings = GetComponents<Steering>();
        rb.drag = drag;
    }

    private void FixedUpdate()
    {
        Vector3 acceleration = Vector3.zero;
        float rotation = 0f;

        foreach (Steering behaviour in steerings)
        {
            //Gets the steering data that we are creating on the steering behaviour attached to the object

            SteeringData steering = behaviour.GetSteering(this);
            acceleration += steering.linear * behaviour.GetWeight();
            rotation += steering.angular * behaviour.GetWeight();
        }

        //Checks for any other problems

        //If the acceleration has reached peack then normalize it and make it constant so it can only reach certain speed

        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= maxAcceleration;
        }

        //Move the body

        rb.AddForce(acceleration);

        //Check for the rotation of the body and add it to get the movement direction

        if (rotation != 0)
        {
            rb.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}