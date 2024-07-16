using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField] float health, armour, speed, damageToDefense;
    private Rigidbody rb;
    public int currentPoint = 0;
    public float distanceToNextPoint = 0, canAttackDefenseTimer;
    private float canAttackDefenseTimerReference;
    private bool attacking = false, inPoint = false, canAttackDefense = true;
    public bool isInvisible;

    GameObject route;
    Route routeScript;
    private KitchenLife kitchenLife;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        route = GameObject.FindGameObjectWithTag("Route");
        routeScript = route.GetComponent<Route>();
        canAttackDefenseTimerReference = Time.time - canAttackDefenseTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //die
            Destroy(gameObject); return;
        }

        if (!attacking) 
        {
            WalkToPoint();
        }
        else
        {
            Attack();
        }

        if (Time.time - canAttackDefenseTimerReference >= canAttackDefenseTimer) 
        {
            canAttackDefense = true;
            canAttackDefenseTimerReference = Time.time;
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Defense") && canAttackDefense)
        {
            collision.gameObject.GetComponent<Defense>().TakeDamage(damageToDefense);
            canAttackDefense = false;
        }
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
