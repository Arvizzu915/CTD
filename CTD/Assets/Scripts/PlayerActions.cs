using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private GameObject grabHitbox;

    public event Action OnPressed, OnExit;

    //Esto nomas esta de mientras{
    [SerializeField]
    private PlacementSystem placementSystem;
    //}

    private void Update()
    {
        //Esto tambien nomas esta de mientras{
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            placementSystem.StartPlacement(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            placementSystem.StartPlacement(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            placementSystem.StartPlacement(2);
        }
        //}

        if (Input.GetKeyDown(KeyCode.Space))
            OnPressed?.Invoke();
        if(Input.GetKeyUp(KeyCode.Space))
            OnExit?.Invoke();
    }

    public Vector3 GetPointingPosition()
    {
        
        return new Vector3(grabHitbox.transform.position.x, grabHitbox.transform.position.y - 0.5f, grabHitbox.transform.position.z);
    }
}
