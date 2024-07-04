using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : MonoBehaviour
{
    [SerializeField] public float cadence, damage1, damage2, damage3;
    [SerializeField] Collider shotArea;

    private float cadenceTime;

    public bool canShoot = false;

    EnemyBasic enemyScript;

    private void Start()
    {
        cadenceTime = Time.time - cadenceTime;
    }

    private void Update()
    {
        if (Time.time - cadenceTime >= cadence && canShoot)
        {
            Shoot();
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
}
