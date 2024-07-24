using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveState : IStationState
{
    private int[] acceptedIDs;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    private Transform stationTransform;
    private float yOffset = 1.085f;

    private BaseIngredientScript ingredientScript = null;
    private int processState = -1;
    // 2 normal > 1 cocinado > 0 quemado > -1 no hay nada

    public StoveState(Transform stationTransform, int[] acceptedIDs)
    {
        this.stationTransform = stationTransform;
        this.acceptedIDs = acceptedIDs;
    }

    public int GetContainedItemID()
    {
        return containedItemID;
    }

    public GameObject GetContainedItemGameObject()
    {
        return containedItemGameObject;
    }

    public void EmptyStation()
    {
        containedItemID = -1;
        containedItemGameObject = null;
        processState = -1;
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        if (ID == -1 || gameObject == null)
            return 0;
        int returnInt = 0;
        if (containedItemID == -1)
        {
            //si no tiene nada, el unico objeto que puede rcibir son los accepted ids
            bool canEnter = false;
            for (int i = 0; i < acceptedIDs.Length; i++)
            {
                if (acceptedIDs[i] == ID)
                    canEnter = true;
            }
            if (!canEnter)
                return 0;
            containedItemID = ID;
            containedItemGameObject = gameObject;
            SetObjectInStation();
            returnInt = 1;
        }
        else
        {
            //si ya tiene dentro una olla (o cualquier otro contenedor que acepte la estufa), entonces ve si el objeto puede meterse a la olla
            switch (containedItemGameObject.GetComponent<BaseContainerScript>().CanEnterContainer(ID, gameObject))
            {
                case 0:
                    return 0;
                case 1:
                    returnInt = 1;
                    //deberia quitarselo del inventario
                    break;
                case 2:
                    returnInt = 2;
                    //nada
                    break;
            }
        }
        
        //como sabemos que el stove state solo recibe contenedores, podemos permitirnos no hacer ninguna comprobación, y directamente acceder a su script 
        if (containedItemGameObject.GetComponent<BaseContainerScript>().GetContainedItemGameObject() != null)
        {
            //aca no hace falta checar el id del objeto dentro de la olla, ya que sabemos que la olla solo puede tener ingredientes
            ingredientScript = containedItemGameObject.GetComponent<BaseContainerScript>().GetContainedItemGameObject().GetComponent<BaseIngredientScript>();
            switch (ingredientScript.ShowState3())
            {
                case -1:
                    processState = 0;
                    break;
                case 0:
                    processState = 2;
                    break;
                case 1:
                    processState = 1;
                    break;
            }
        }
        else
        {
            processState = -1;
        }
        return returnInt;
    }

    public void OnAccess2()
    {
        //Nada
    }

    public void UpdateState()
    {
        if (containedItemGameObject != null)
        {
            if (containedItemGameObject.GetComponent<BaseContainerScript>().GetContainedItemID() == -1)
                processState = -1;
            if (processState > 0)
            {
                ingredientScript.cookingTimes[0] -= Time.deltaTime;
                if(processState == 2 && ingredientScript.cookingTimes[0] <= 0)
                {
                    ingredientScript.ChangeState3(1, false);
                    containedItemGameObject.GetComponent<BaseContainerScript>().UpdateModel();
                    processState = 1;
                }
                else if(processState == 1 && ingredientScript.cookingTimes[0] <= -10f)
                {
                    ingredientScript.ChangeState3(-1, false);
                    containedItemGameObject.GetComponent<BaseContainerScript>().UpdateModel();
                    processState = 0;
                }
            }
        }
    }

    private void SetObjectInStation()
    {
        containedItemGameObject.transform.SetParent(null);
        Vector3 newPosition = new Vector3(stationTransform.position.x, stationTransform.position.y + yOffset, stationTransform.position.z);
        containedItemGameObject.transform.position = newPosition;
        containedItemGameObject.transform.rotation = stationTransform.rotation;
    }
}
