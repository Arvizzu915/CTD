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
        cadenceTime = Time.time - cadence;
        turretScript = gameObject.GetComponentInParent<TurretsBasicBehavior>();
    }

    private void Update()
    {
        canShoot = turretScript.canShoot;

        if (Time.time - cadenceTime >= cadence && canShoot)
        {
            Debug.Log("change");
            shoot = true;
            cadenceTime = Time.time;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && shoot)
        {
            Debug.Log("shotgun");
            other.gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            shoot = false;
        }
    }
}
