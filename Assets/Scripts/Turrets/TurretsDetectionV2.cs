using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsDetectionV2 : MonoBehaviour
{
    private float turretDistance = 0f;
    private GameObject enemyObjective;

    private bool shootingEnemy = false;

    public int turretsInRange = 0;
    public List<GameObject> turrets = new List<GameObject>();

    private EnemyBasic enemyScript;

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
                        turretDistance = turrets[i].gameObject.GetComponent<EnemyBasic>().distanceToNextPoint;

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
