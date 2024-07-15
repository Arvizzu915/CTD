using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveState : IStationState
{
    private int[] acceptedIDs;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    private float progressPercentTime = 0.5f;
    private float timeCount = 0f;

    public StoveState(int[] acceptedIDs)
    {
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
    }

    public bool OnAccessWithID(int objectID, GameObject objectGameObject)
    {
        bool canEnter = false;
        for (int i = 0; i < acceptedIDs.Length; i++)
        {
            if (acceptedIDs[i] == objectID)
                canEnter = true;
        }
        if (containedItemID != -1 || !canEnter)
            return false;

        containedItemID = objectID;
        containedItemGameObject = objectGameObject;
        StartTimer();
        return true;
    }

    private void StartTimer()
    {
        timeCount = progressPercentTime;
        if (CheckIfItemIsReady())
        {
            //inicia protocolo de quemar
            containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().ChangeProcessTimeFromContainedItem(true);
        }
        else
        {
            //Lo normal
            containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().ChangeProcessTimeFromContainedItem(false);
        }
    }

    private bool CheckIfItemIsReady()
    {
        return containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().IsReady();
    }

    private bool CheckIfIngredientInItemIsReady()
    {
        return containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().IngredientIsReady();
    }

    public void OnAccess2()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        if (timeCount > 0f)
        {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0f)
            {
                //Avanza el proceso del ingrediente, y ve si esta listo
                containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().ChangeProcessPercentFromContainedItem();
                if (CheckIfIngredientInItemIsReady())
                {
                    //Si el ingrediente esta listo, entonces ve primero si el contenedor esta listo o no
                    if (CheckIfItemIsReady())
                    {
                        //Si esta listo, significa que ya se quemo
                        containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().ChangeToBurnedContainer();
                    }
                    else
                    {
                        //si no esta listo, entonces ahora le cambia a cocinado
                        containedItemGameObject.GetComponent<IndividualState3ChangerContainerState>().ChangeState3OfContainedItem();
                        //vuelve a iniciar el contador, pero ahora pa quemarse
                        StartTimer();
                    }
                }
                //Si no esta listo, reinicia el contador
                timeCount = progressPercentTime;
            }
        }
    }
}
