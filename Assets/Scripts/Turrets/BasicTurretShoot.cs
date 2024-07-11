using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretShoot : MonoBehaviour
{
    [SerializeField] public float cadence, damage1, damage2, damage3;
    [SerializeField] GameObject projectile;

    private float cadenceTime;
    public float damage;

    private EnemyBasic enemyScript;
    private BasicTurretBullet bulletScript;
    private TurretsBasicBehavior turretScript;
    public bool canShoot = false;

    private void Start()
    {
        turretScript = GetComponent<TurretsBasicBehavior>();
        enemyScript = gameObject.GetComponent<EnemyBasic>();
        damage = damage1;
        cadenceTime = Time.time - cadence;
    }

    private void Update()
    {
        //cuando mejore solo le asigna el damage que sigue al damage de la bala

        canShoot = turretScript.canShoot;

        if (Time.time - cadenceTime >= cadence && canShoot)
        {
            Shoot();
            cadenceTime = Time.time;
        }
    }

    private void Shoot()
    {
        Instantiate(projectile, transform.position, transform.rotation, transform);
    }
}
