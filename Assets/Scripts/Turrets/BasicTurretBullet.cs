using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretBullet : MonoBehaviour
{
    [SerializeField] float speed;

    private float damage, timer;

    Rigidbody rb;
    EnemyBasic enemyScript;
    TurretsBasicBehavior turretsBasicBehaviorScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = Time.time;
        damage = gameObject.GetComponentInParent<BasicTurretShoot>().damage;
        transform.parent = null;
    }

    private void Update()
    {
        
        rb.velocity = transform.forward * speed * Time.deltaTime;

        if (Time.time - timer >= 15)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
