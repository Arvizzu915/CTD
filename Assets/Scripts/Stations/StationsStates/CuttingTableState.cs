using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTableState : IStationState
{
    private int stationID;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    private Transform stationTransform;
    private float yOffset = 1.05f;

    private BaseIngredientScript ingredientScript = null;

    public CuttingTableState(int stationID, Transform stationTransform)
    {
        this.stationID = stationID;
        this.stationTransform = stationTransform;
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        if (ID == -1 || gameObject == null)
            return 0;
        //Ve si lo que va a entrar es un plato o un ingrediente
        int ingredientID;
        GameObject ingredientGameObject;
        int returnInt = 0;
        if (ID >= 100 && ID < 200)
        {
            ingredientID = gameObject.GetComponent<BaseContainerScript>().GetContainedItemID();
            ingredientGameObject = gameObject.GetComponent<BaseContainerScript>().GetContainedItemGameObject();
            if(ingredientID >= 200 && ingredientID < 300)
            {
                //solo si el contenedor tiene un ingrediente, entonces lo toma en cuenta
                ingredientScript = ingredientGameObject.GetComponent<BaseIngredientScript>();
                returnInt = 2;
            }
            else
            {
                return 0;
            }
        }
        else if (ID >= 200 && ID < 300)
        {
            ingredientID = ID;
            ingredientGameObject = gameObject;
            ingredientScript = gameObject.GetComponent<BaseIngredientScript>();
            returnInt = 1;
        }
        else
        {
            return 0;
        }
        //si llega hasta aca es porque si encontro un ingrediente
        int[] ingredientContainersAndStationsIDs = ingredientScript.ShowContainersAndStationsIDs();
        bool canEnter = false;
        for (int i = 0; i < ingredientContainersAndStationsIDs.Length; i++)
        {
            if (ingredientContainersAndStationsIDs[i] == stationID)
                canEnter = true;
        }
        //Primero vemos si puede entrar a esta estacion
        if (containedItemID != -1 || !canEnter)
            return 0;

        //si puede, entonces iguala
        containedItemID = ingredientID;
        containedItemGameObject = ingredientGameObject;
        SetObjectInStation();
        return returnInt;
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
        if (containedItemGameObject != null && ingredientScript != null)
        {
            if (ingredientScript.ShowState1() != 1)
            {
                ingredientScript.cookingTimes[1] -= 1;
                if (ingredientScript.cookingTimes[1] <= 0) 
                {
                    ingredientScript.ChangeState1(1, true);
                }
            }
        }
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
