using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float timer, timerReference = 0;
    private bool canMakeDamage;

    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        canMakeDamage = false;
        timerReference = Time.time - timer;
    }

    private void Update()
    {
        if(Time.time - timerReference >= timer)
        {
            canMakeDamage = true;
            timerReference = Time.time;
        }

        if(canMakeDamage && enemies.Count > 0) 
        {
            DamageEnemies();
        }
    }

    private void DamageEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            }
            
        }

        canMakeDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
    }
}
