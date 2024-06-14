using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField] float life, speed;
    private Rigidbody rb;
    private int currentPoint = 0;
    GameObject route;
    Route routeScript;

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
        WalkToPoint();
    }

    void WalkToPoint()
    {
        transform.LookAt(routeScript.pointsToFollow[currentPoint]);
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Point"))
        {
            currentPoint++;
        }
    }
}
