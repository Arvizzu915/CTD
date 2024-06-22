using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceState : IStationState
{
    private int containedItemID;
    private float furnaceTime = 3f;
    private float timeCount = 0f;
    private bool itemIsReady = false;

    //public FurnaceState()
    //{

    //}

    public int OnAccessEmpty()
    {
        if (containedItemID != -1 && itemIsReady)
        {
            int itemToReturn = containedItemID;
            containedItemID = -1;
            itemIsReady = false;
            return itemToReturn;
        }
        return -1;
    }

    public bool OnAccessWithID(int ID)
    {
        if(containedItemID == -1)
        {
            containedItemID = ID;
            itemIsReady = false;
            timeCount = furnaceTime;
        }
        return false;
    }

    private int ChangeItem(int ID)
    {
        int newID = ID;
        //aca cambiara al plato
        return newID;
    }

    public void UpdateState()
    {
        if(timeCount > 0f && !itemIsReady)
        {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0f)
            {
                timeCount = 0f;
                itemIsReady = true;
                containedItemID = ChangeItem(containedItemID);
            }
        }
        
    }
}
