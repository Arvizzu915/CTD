using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Collider explosionArea, bulletCollider;

    public float damage, timer;
    private bool exploded = false;
    private List<GameObject> enemies = new List<GameObject>();

    Rigidbody rb;
    EnemyBasic enemyScript;
    TurretsBasicBehavior turretsBasicBehaviorScript;

    private void Start()
    {
        explosionArea.enabled = false;
        rb = GetComponent<Rigidbody>();
        timer = Time.time;
    }

    private void Update()
    {
        if (exploded) 
        {
            Explode();
        }

        damage = gameObject.GetComponentInParent<BasicTurretShoot>().damage;
        rb.velocity = transform.forward * speed * Time.deltaTime;

        if (Time.time - timer >= 15)
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        foreach (var item in enemies)
        {
            item.gameObject.GetComponent<EnemyBasic>().TakeDamage(damage);
            Debug.Log(item.name);
        }

        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            speed = 0;
            bulletCollider.enabled = false;
            explosionArea.enabled = true;
            exploded = true;
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
