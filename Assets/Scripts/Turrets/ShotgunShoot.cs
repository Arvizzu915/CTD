using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : MonoBehaviour
{
    //checar que se llame la funcion de bajar vida en los enemigos

    [SerializeField] public float cadence, damage1, damage2, damage3;

    private float cadenceTime, damage = 0;

    private bool shoot = false, canShoot = false;

    public List<GameObject> enemies = new List<GameObject>();

    EnemyBasic enemyScript;
    TurretsBasicBehavior turretScript;

    private void Start()
    {
        cadence = gameObject.GetComponentInParent<TurretsBasicBehavior>().cadence;
        damage = damage1;
        cadenceTime = Time.time - cadence;
        turretScript = gameObject.GetComponentInParent<TurretsBasicBehavior>();
    }

    private void Update()
    {
        cadence = gameObject.GetComponentInParent<TurretsBasicBehavior>().cadence;
        canShoot = turretScript.canShoot;

        if (Time.time - cadenceTime >= cadence && canShoot)
        {
            shoot = true;
            cadenceTime = Time.time;
        }

        if(shoot && enemies.Count > 0) 
        {
            ShootEnemies();
        }
    }

    private void ShootEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                //daño es igual a la distancia máxima más la distancia del enemigo multiplicada por el daño sobre 100 por 2
                //damage = radius + Vector3.Distance(transform.position, enemies[i].transform.position) * .8;
                enemies[i].gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            }
        }

        shoot = false;
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
