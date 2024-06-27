using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField] float health, armour, speed;
    private Rigidbody rb;
    private int currentPoint = 0;
    private bool attacking = false;

    GameObject route;
    Route routeScript;
    private KitchenLife kitchenLife;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        route = GameObject.FindGameObjectWithTag("Route");
        routeScript = route.GetComponent<Route>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking) 
        {
            WalkToPoint();
        }
        else
        {
            Attack();
        }
    }

    void WalkToPoint()
    {
        transform.LookAt(routeScript.pointsToFollow[currentPoint]);
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        if (armour > 0)
        {
            armour -= damage;
        }
        else
        {
            health -= damage;
        }
    }

    private void Attack()
    {
        kitchenLife.TakeDamage(health);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Point"))
        {
            currentPoint++;
        }

        if (other.CompareTag("Kitchen"))
        {
            kitchenLife = other.gameObject.GetComponent<KitchenLife>();
            attacking = true;
        }
    }
}
