using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTableState : IStationState
{
    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    private Transform stationTransform;
    private float yOffset = 1f;

    public BasicTableState(Transform stationTransform)
    {
        this.stationTransform = stationTransform;
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
            if(containedItemID >= 105 || containedContainerScript.GetContainedItemID() == -1)
            {
                //al menos que lo que este dentro sea un plato principal con algo dentro, puede ver si lo que viene se puede meter al contenedor
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
        Vector3 newPosition = new Vector3(stationTransform.position.x, stationTransform.position.y + yOffset, stationTransform.position.z);
        containedItemGameObject.transform.position = newPosition;
        containedItemGameObject.transform.rotation = stationTransform.rotation;
        if (containedItemID >= 200 && containedItemID < 300)
        {
            containedItemGameObject.GetComponent<BaseIngredientScript>().ShowModel();
        }
    }
}
