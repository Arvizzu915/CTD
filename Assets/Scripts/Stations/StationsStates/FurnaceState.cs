using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceState : IStationState
{
    private int containedItemID;
    private float furnaceTime = 10f;
    private float timeCount = 0f;
    private bool itemIsReady = false;

    public FurnaceState(int containedItemID)
    {
        this.containedItemID = containedItemID;
    }

    public int OnAccessEmpty()
    {
        //En este caso especifico el horno solo regresa el nuevo objeto
        if (containedItemID != -1 && itemIsReady)
        {
            int idToReturn = containedItemID;
            containedItemID = -1;
            itemIsReady = false;
            return idToReturn;
        }
        return -1;
    }

    public bool OnAccessWithID(int ID, GameObject gameObject)
    {
        //para este ejemplo, digamos que solo acepta el plato basico, y debe tener por lo menos un ingrediente o especia 
        if (containedItemID == -1 && (ID >= 100 && ID < 200) && gameObject != null)
        {
            if(gameObject.GetComponent<PlateModelScript>().GetIngredientID() != -1 || gameObject.GetComponent<PlateModelScript>().GetSpiceID() != -1)
            {
                containedItemID = ChangeItem(gameObject.GetComponent<PlateModelScript>().GetIngredientID(), gameObject.GetComponent<PlateModelScript>().GetSpiceID());
                itemIsReady = false;
                timeCount = furnaceTime;
                return true;
            }
        }
        return false;
    }

    private int ChangeItem(int ingredient, int spice)
    {
        if(ingredient == 200 && spice == -1)
        {
            return 400;
        }
        if(ingredient == -1 && spice == 300)
        {
            return 401;
        }
        if(ingredient == 200 && spice == 300)
        {
            return 402;
        }
        //Aca retornaria -1 si es que el combo de ingredientes no da nada, esto deberia ser un error, o regrearia un plato quemado o algo asi
        return -1;
    }

    public void UpdateState()
    {
        if(timeCount > 0f && !itemIsReady)
        {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0f)
            {
                Debug.Log("Esta listo");
                timeCount = 0f;
                itemIsReady = true;
            }
        }
        
    }
}
