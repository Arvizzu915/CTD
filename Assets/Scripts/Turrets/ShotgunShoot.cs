using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : MonoBehaviour
{
    //checar que se llame la funcion de bajar vida en los enemigos

    [SerializeField] public float cadence, damage1, damage2, damage3;

    private float cadenceTime, damage = 0;

    private bool shoot = false, canShoot = false;

    EnemyBasic enemyScript;
    TurretsBasicBehavior turretScript;

    private void Start()
    {
        damage = damage1;
        cadenceTime = Time.time - cadenceTime;
        turretScript = gameObject.GetComponentInParent<TurretsBasicBehavior>();
    }

    private void Update()
    {
        canShoot = turretScript.canShoot;

        if (Time.time - cadenceTime >= cadence && canShoot)
        {
            shoot = true;

            cadence = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyScript = other.gameObject.GetComponent<EnemyBasic>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && gameObject.CompareTag("ShotgunArea") && shoot)
        {
            enemyScript.TakeDamage(damage);
        }
    }
}
