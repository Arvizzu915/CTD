using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashcanState : IStationState
{
    public int GetContainedItemID()
    {
        return -1;
    }

    public GameObject GetContainedItemGameObject()
    {
        return null;
    }
    
    public void EmptyStation()
    {
        //nada
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        if (ID == -1 || gameObject == null)
            return 0;
        if (ID >= 100 && ID < 200)
        {
            gameObject.GetComponent<BaseContainerScript>().EmptyContainer(true);
            return 2;
        }
        else if (ID >= 200 && ID < 300)
        {
            Object.Destroy(gameObject);
            return 1;
        }
        return 0;
    }

    public void OnAccess2()
    {
        //nada
    }

    public void UpdateState()
    {
        //nada
    }
}
