using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> humansAlive = new List<GameObject>();

    [SerializeField]
    private GameObject zombiePrefab;

    private float raycastRange = 1000f;

    private bool firstZombie = true;

    public UnityEvent<float> onGameEnd;

    public UnityEvent<int> onHumanKilled;

    private int humans = 20;

    private float simulationTime = 0f;

    private void Start()
    {
        Time.timeScale = 0;

        simulationTime = 0f;

        humans = humansAlive.Count;
    }

    private void Update()
    {
        if (firstZombie && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                if (hit.transform.CompareTag("Human"))
                {
                    InstantiateFirstZombie(hit.collider.gameObject);
                }
            }
        }

        if (!firstZombie)
        {
            simulationTime += Time.deltaTime;
        }

        if (humansAlive.Count <= 0)
        {
            Debug.Log("Game ended");
            Time.timeScale = 0;
            onGameEnd.Invoke(simulationTime);
            //simulationTime = 0f;
        }
    }

    public void HumanKilled(GameObject human)
    {
        if (human == null)
        {
            Debug.Log("Human not found on list gm");
            return;
        }
        else if (humansAlive.Contains(human))
            humansAlive.Remove(human);

        humans = humansAlive.Count;

        onHumanKilled.Invoke(humans);
    }

    private void InstantiateFirstZombie(GameObject go)
    {
        GameObject newZombie = Instantiate(zombiePrefab);

        newZombie.transform.parent = go.transform.parent;

        newZombie.transform.position = go.transform.position;

        HumanKilled(go);

        Destroy(go.gameObject);

        firstZombie = false;

        Time.timeScale = 1;
    }

    public void ResetSimulation()
    {
        SceneManager.LoadScene("Infectonator");
    }
}