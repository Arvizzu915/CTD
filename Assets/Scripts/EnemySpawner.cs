using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn = new List<GameObject>();
    [SerializeField] private float spawnTimer;
    private float spawnTimerReference = 0;

    private void Start()
    {
        spawnTimerReference = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTimerReference >= spawnTimer) 
        {
            foreach (GameObject item in enemiesToSpawn)
            {
                item.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
