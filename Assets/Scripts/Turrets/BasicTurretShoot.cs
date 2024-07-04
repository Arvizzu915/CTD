using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretShoot : MonoBehaviour
{
    [SerializeField] public float cadence, damage1, damage2, damage3;
    [SerializeField] GameObject projectile;

    private float cadenceTime, damage;

    private EnemyBasic enemyScript;
    private BasicTurretBullet bulletScript;
    public bool canShoot = false;

    private void Start()
    {
        enemyScript = gameObject.GetComponent<EnemyBasic>();
        damage = damage1;
        cadenceTime = Time.time - cadence;
    }

    private void Update()
    {
        //cuando mejore solo le asigna el damage que sigue al damage de la bala

        if (Time.time - cadenceTime >= cadence && canShoot)
        {
            Shoot();
            cadenceTime = Time.time;
        }
    }

    private void Shoot()
    {

        projectile.gameObject.GetComponent<BasicTurretBullet>().damage = damage;
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
