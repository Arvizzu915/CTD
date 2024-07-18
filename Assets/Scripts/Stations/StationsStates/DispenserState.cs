using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserState : IStationState
{
    private int dispensedItemID;
    private float itemCooldown = 5f;
    private float timeCount = 0f;
    private int itemCount = 0;

    public DispenserState(int dispensedItemID)
    {
        this.dispensedItemID = dispensedItemID;
    }

    public int GetContainedItemID()
    {
        if (itemCount > 0)
        {
            itemCount--;
            return dispensedItemID;
        }
        Debug.Log("Agotado");
        return -1;
    }

    public GameObject GetContainedItemGameObject()
    {
        return null;
    }

    public void EmptyStation()
    {
        //Nada
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        return 0;
    }

    public void OnAccess2()
    {
        //Nada
    }

    public void UpdateState()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= itemCooldown)
        {
            timeCount = 0f;
            itemCount++;
        }
    }
}
