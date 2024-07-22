using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretsBasicBehavior : MonoBehaviour
{
    public GameObject enemyObjective;

    public float cadence;
    public int enemiesInRange = 0;
    public bool canShoot = false, canSeeInvisible;
    private bool upgraded = false;
    [SerializeField] private bool canUpgrade;
    [SerializeField] private GameObject turretUpgrade;

    public List<GameObject> enemies = new List<GameObject>();

    private EnemyBasic currentEnemyBeingCompared, enemyToCompare, enemyCollision;
    private BasicTurretShoot shootScript;

    private void Start()
    {
        shootScript = GetComponent<BasicTurretShoot>();
    }

    private void FixedUpdate()
    {
        if (enemyObjective != null)
        {
            transform.LookAt(new Vector3(enemyObjective.transform.position.x, transform.position.y, enemyObjective.transform.position.z));
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        FindNewEnemy();

        if (enemiesInRange == 0)
        {
            enemyObjective = null;
            canShoot = false;
        }

        if (upgraded) 
        {
            Destroy(gameObject);
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
                if ((enemyToCompare.distanceToNextPoint < currentEnemyBeingCompared.distanceToNextPoint) && (enemyToCompare.currentPoint >= currentEnemyBeingCompared.currentPoint) || (UnityEngine.Vector3.Distance(enemyObjective.transform.position, transform.position) > 7))
                {
                    enemyObjective = item;
                }
            }
        }

        if(enemies.Count > 0 && enemyObjective == null)
        {
            enemyObjective = enemies[0];
        }
    }

    public GameObject CanUpgradeTurret()
    {
        if (canUpgrade)
        {
            Instantiate(turretUpgrade, transform.position, Quaternion.identity);
            upgraded = true;
            return turretUpgrade;
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCollision =  other.gameObject.GetComponent<EnemyBasic>();

            if (canSeeInvisible || (!canSeeInvisible && !enemyCollision.isInvisible)) 
            {
                if (enemiesInRange <= 0)
                {
                    enemyObjective = other.gameObject;
                }
                enemies.Add(other.gameObject);
                enemiesInRange++;
            }
            
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
