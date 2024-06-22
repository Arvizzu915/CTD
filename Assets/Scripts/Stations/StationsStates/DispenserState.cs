using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserState : IStationState
{
    int dispensedItemID;
    private float itemCooldown = 3f;
    private float timeCount = 0f;
    private int itemCount = 0;

    public DispenserState(int dispensedItemID)
    {
        this.dispensedItemID = dispensedItemID;
    }

    public int OnAccessEmpty()
    {
        if(itemCount > 0)
        {
            itemCount--;
            return dispensedItemID;
        }
        return -1;
    }

    public bool OnAccessWithID(int ID)
    {
        //Por ahora es false, pero quiza si esta sosteniendo un plato, podría haber una exepcion
        return false;
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
