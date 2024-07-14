using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenLife : MonoBehaviour
{
    public float KitchenHealth;

    public void TakeDamage(float damage)
    {
        KitchenHealth -= damage;
    }
}
