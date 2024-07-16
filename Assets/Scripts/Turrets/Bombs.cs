using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    [SerializeField] float speed;

    public float damage, timer;
    public bool exploded = false, isAntiArmour;

    public List<GameObject> enemies = new List<GameObject>();

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
        if (exploded)
        {
            Explode();
        }

        rb.velocity = transform.forward * speed * Time.deltaTime;

        if (Time.time - timer >= 15)
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        foreach (GameObject item in enemies)
        {
            item.gameObject.GetComponent<EnemyBasic>().TakeDamage(damage, isAntiArmour);
            Destroy(gameObject);
        }
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
