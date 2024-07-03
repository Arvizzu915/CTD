using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretsBasicBehavior : MonoBehaviour
{

    private GameObject enemyObjective;

    private bool shootingEnemy = false;

    public int turretsInRange = 0;
    public List<GameObject> turrets = new List<GameObject>();

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!shootingEnemy)
            {
                if (turretsInRange <= 0)
                {
                    enemyObjective = other.gameObject;
                }
                else
                {
                    for (int i = 0; i < turrets.Count; i++)
                    {


                    }
                }
            }

            turrets.Add(other.gameObject);
            turretsInRange++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemyObjective = other.gameObject)
            {
                shootingEnemy = false;
            }
            turrets.Remove(other.gameObject);
            turretsInRange--;
        }
    }
}
