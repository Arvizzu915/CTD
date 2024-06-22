using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashcanState : IStationState
{
    public int OnAccessEmpty()
    {
        return -1;
    }

    public bool OnAccessWithID(int ID)
    {
        return true;
    }

    public void UpdateState()
    {
        //nada xd
    }
}
