using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private GameObject grabHitbox;

    public event Action OnPressed1, OnPressed2;

    //Esto nomas esta de mientras{
    [SerializeField]
    private PlacementSystem placementSystem;
    [SerializeField]
    private InventorySystem inventorySystem;
    //}

    private void Update()
    {
        //Esto tambien nomas esta de mientras{
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    inventorySystem.GetObject(400, -1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    inventorySystem.GetObject(401, -1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    inventorySystem.GetObject(402, -1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    inventorySystem.GetObject(0, -1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    inventorySystem.GetObject(100, -1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    inventorySystem.GetObject(200, -1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    inventorySystem.GetObject(300, -1);
        //}
        //}

        if (Input.GetKeyDown(KeyCode.Space))
            OnPressed1?.Invoke();
        if(Input.GetKeyDown(KeyCode.K))
            OnPressed2?.Invoke();
    }

    public Vector3 GetPointingPosition()
    {
        
        return new Vector3(grabHitbox.transform.position.x, grabHitbox.transform.position.y - 0.5f, grabHitbox.transform.position.z);
    }

    public Vector3 GetGrabHitboxPosition()
    {
        //aca podriamos modificar las coordenadas para que coloque el objeto justo donde lo queremos
        return new Vector3(grabHitbox.transform.position.x, grabHitbox.transform.position.y, grabHitbox.transform.position.z);
    }
}
