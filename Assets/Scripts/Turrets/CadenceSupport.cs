using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenceSupport : MonoBehaviour
{
    [SerializeField] private float cadenceMultiplier;
    public List<GameObject> turrets = new List<GameObject>();

    private void Update()
    {
        foreach (GameObject item in turrets)
        {
            item.gameObject.GetComponent<TurretsBasicBehavior>().cadence *= cadenceMultiplier;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            Debug.Log("add");
            turrets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            turrets.Remove(other.gameObject);
        }
    }
}
