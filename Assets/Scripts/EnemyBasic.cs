using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField] float health, armour, speed;
    private Rigidbody rb;
    public int currentPoint = 0;
    public float distanceToNextPoint = 0;
    private bool attacking = false, inPoint = false;

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
        transform.LookAt(new UnityEngine.Vector3(routeScript.pointsToFollow[currentPoint].transform.position.x, transform.position.y, routeScript.pointsToFollow[currentPoint].transform.position.z));
        rb.velocity = transform.forward * speed * Time.deltaTime;

        distanceToNextPoint = UnityEngine.Vector3.Distance(transform.position, routeScript.pointsToFollow[currentPoint].position);
    }

    public void TakeDamage(float damage)
    {
        if (armour > 0)
        {
            armour -= damage/2;
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
        if(other.CompareTag("Point") && !inPoint)
        {
            inPoint = true;
            currentPoint++;
        }

        if (other.CompareTag("Kitchen"))
        {
            kitchenLife = other.gameObject.GetComponent<KitchenLife>();
            attacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Point") && inPoint)
        {
            inPoint = false;
        }
    }
}
