using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformToZombie : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;

    public UnityEvent onTransform;

    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    //Instaciar un nuevo zombie cuando se consiga pillar a un humano
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            GameObject newZombie = Instantiate(zombiePrefab);

            newZombie.transform.parent = collision.transform.parent;

            newZombie.transform.position = collision.transform.position;

            gm.HumanKilled(collision.gameObject);

            onTransform.Invoke();

            Destroy(collision.gameObject);
        }
    }
}