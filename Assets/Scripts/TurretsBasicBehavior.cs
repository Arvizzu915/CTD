using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretsBasicBehavior : MonoBehaviour
{
    private float turretDistance = 0f;
    public GameObject enemyObjective;

    public int enemiesInRange = 0;
    public List<GameObject> enemies = new List<GameObject>();

    private EnemyBasic enemyScript, currentEnemyBeingCompared, enemyToCompare;

    private void Update()
    {
        FindNewEnemy();

        if (enemiesInRange == 0)
        {
            enemyObjective = null;
        }
    }

    void FindNewEnemy()
    {
        enemies.RemoveAll(item => item == null);

        if (enemyObjective != null)
        {
            foreach (GameObject item in enemies)
            {
                currentEnemyBeingCompared = enemyObjective.GetComponent<EnemyBasic>();
                enemyToCompare = item.gameObject.GetComponent<EnemyBasic>();
                if ((enemyToCompare.distanceToNextPoint < currentEnemyBeingCompared.distanceToNextPoint) && (enemyToCompare.currentPoint >= currentEnemyBeingCompared.currentPoint) || (UnityEngine.Vector3.Distance(enemyObjective.transform.position, transform.position) > 5))
                {
                    enemyObjective = item;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange <= 0)
            {
                enemyObjective = other.gameObject;
            }
            enemies.Add(other.gameObject);
            enemiesInRange++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
            enemiesInRange--;
        }
    }
}
