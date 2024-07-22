using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenLife : MonoBehaviour
{
    public float KitchenHealth;

    private void FixedUpdate()
    {
        if (KitchenHealth <= 0) 
        {
            Time.timeScale = 0.0f;
        }
    }

    public void TakeDamage(float damage)
    {
        KitchenHealth -= damage;
    }
}
