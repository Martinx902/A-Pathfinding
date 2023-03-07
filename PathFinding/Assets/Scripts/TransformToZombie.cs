using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformToZombie : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;

    public UnityEvent onTransform;

    //Instaciar un nuevo zombie cuando se consiga pillar a un humano
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            Destroy(collision.gameObject);

            onTransform.Invoke();

            Instantiate(zombiePrefab, collision.transform);
        }
    }
}