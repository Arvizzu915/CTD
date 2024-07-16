using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretShoot : MonoBehaviour
{
    [SerializeField] public float cadence, damage;
    [SerializeField] GameObject projectile;

    private float cadenceTime;

    private EnemyBasic enemyScript;
    private BasicTurretBullet bulletScript;
    private TurretsBasicBehavior turretScript;
    private bool canShoot = false;

    private void Start()
    {
        cadence = gameObject.GetComponent<TurretsBasicBehavior>().cadence;
        turretScript = GetComponent<TurretsBasicBehavior>();
        enemyScript = gameObject.GetComponent<EnemyBasic>();
        cadenceTime = Time.time - cadence;
    }

    private void Update()
    {
        cadence = gameObject.GetComponent<TurretsBasicBehavior>().cadence;

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
