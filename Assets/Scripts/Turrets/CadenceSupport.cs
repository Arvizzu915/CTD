using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenceSupport : MonoBehaviour
{
    [SerializeField] private float cadenceMultiplier;
    public List<Transform> turrets = new List<Transform>();
    private bool activate = false;
    private float timer = 0;

    private void Start()
    {
        activate = true;
        timer = Time.time;
    }

    private void Update()
    {
        if (activate) 
        {
            if (Time.time - timer >= 2 && activate)
            {
                Support();
            }
        }
    }

    private void Support()
    {


        for (int i = 0; i < turrets.Count; i++)
        {
            Debug.Log(turrets[i].name);
            turrets[i].gameObject.GetComponent<TurretsBasicBehavior>().cadence /= cadenceMultiplier;
        }

        activate = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            turrets.Add(other.gameObject.transform.parent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            turrets.Remove(other.gameObject.transform.parent);
        }
    }
}
