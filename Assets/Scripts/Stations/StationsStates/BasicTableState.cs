using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTableState : IStationState
{
    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    private Vector3 stationPosition;
    private float yOffset = 1f;

    public BasicTableState(Vector3 stationPosition)
    {
        this.stationPosition = stationPosition;
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        if (ID == -1 || gameObject == null)
            return 0;
        if (containedItemID == -1)
        {
            containedItemID = ID;
            containedItemGameObject = gameObject;
            SetObjectInStation();
            return 1;
        }
        else if (containedItemID >= 100 && containedItemID < 200)
        {
            BaseContainerScript containedContainerScript = containedItemGameObject.GetComponent<BaseContainerScript>();
            if(containedContainerScript.GetContainedItemID() == -1 && containedContainerScript.GetContainedItemGameObject() == null)
            {
                //solo si el contenedor que tiene la mesa esta vacio, entonces puede recibir lo que viene
                switch(containedContainerScript.CanEnterContainer(ID, gameObject))
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                }
            }
        }
        return 0;
    }

    public void EmptyStation()
    {
        containedItemID = -1;
        containedItemGameObject = null;
    }

    public GameObject GetContainedItemGameObject()
    {
        return containedItemGameObject;
    }

    public int GetContainedItemID()
    {
        return containedItemID;
    }

    public void OnAccess2()
    {
        //Nada
    }

    public void UpdateState()
    {
        //Nada
    }

    private void SetObjectInStation()
    {
        containedItemGameObject.transform.SetParent(null);
        Vector3 newPosition = new Vector3(stationPosition.x, stationPosition.y + yOffset, stationPosition.z);
        containedItemGameObject.transform.position = newPosition;
        containedItemGameObject.GetComponent<BaseIngredientScript>().ShowModel();
    }
}
