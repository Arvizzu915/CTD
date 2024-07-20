using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotState : IContainerState
{
    private GameObject[] ingredientModels000, ingredientModels001;
    private GameObject burnedModel, lastModel;

    private int containerID;
    private GameObject containerGameObject;
    private float yOffset = 0f;

    private int[] ingredientsIDs;
    private int containedItemIndex = -1;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    public PotState(int containerID, GameObject containerGameObject, GameObject burnedModel, GameObject[] ingredientModels000, GameObject[] ingredientModels001, int[] ingredientsIDs)
    {
        this.containerID = containerID;
        this.containerGameObject = containerGameObject;
        this.burnedModel = burnedModel;
        this.ingredientModels000 = ingredientModels000;
        this.ingredientModels001 = ingredientModels001;
        this.ingredientsIDs = ingredientsIDs;
    }

    public int GetContainedItemID()
    {
        return containedItemID;
    }

    public GameObject GetContainedItemGameObject()
    {
        return containedItemGameObject;
    }

    public void EmptyContainer(bool deleteContainedItemGameObject)
    {
        containedItemIndex = -1;
        if (deleteContainedItemGameObject)
            Object.Destroy(containedItemGameObject);
        containedItemID = -1;
        containedItemGameObject = null;
        UpdateModel();
    }

    public int CanEnterContainer(int ID, GameObject gameObject)
    {
        //Si el recipiente no esta vacio, nel
        if (containedItemID != -1 || containedItemGameObject != null)
            return 0;
        //Ve si lo que va a entrar es un plato o un ingrediente
        int ingredientID;
        GameObject ingredientGameObject;
        BaseIngredientScript ingredientScript;
        int returnInt = 0;
        if (ID >= 100 && ID < 200)
        {
            ingredientID = gameObject.GetComponent<BaseContainerScript>().GetContainedItemID();
            ingredientGameObject = gameObject.GetComponent<BaseContainerScript>().GetContainedItemGameObject();
            if(ingredientID >= 200 && ingredientID < 300)
            {
                //Solo si el plato tiene un ingrediente, lo toma en cuenta
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
        //Si llega hasta aca es porque hay un ingrediente que quiere entrar, vamos a ver si puede entrar
        int[] ingredientContainersAndStationsIDs = ingredientScript.ShowContainersAndStationsIDs();
        bool canEnterThisContainer = false;
        for (int i = 0; i < ingredientContainersAndStationsIDs.Length; i++)
        {
            if (ingredientContainersAndStationsIDs[i] == containerID)
                canEnterThisContainer = true;
        }
        //Primero vemos si puede entrar a este recipiente
        if (!canEnterThisContainer)
            return 0;
        //Gana aqui
        containedItemID = ingredientID;
        containedItemGameObject = ingredientGameObject;
        SetOriginalGameObject();
        for (int i = 0; i < ingredientsIDs.Length; i++)
        {
            if (ingredientsIDs[i] == containedItemID)
                containedItemIndex = i;
        }
        UpdateModel();
        if (returnInt == 2)
            gameObject.GetComponent<BaseContainerScript>().EmptyContainer(false);
        return returnInt;
    }

    private void SetOriginalGameObject()
    {
        containedItemGameObject.GetComponent<BaseIngredientScript>().HideModel();
        Vector3 newPosition = new Vector3(containerGameObject.transform.position.x, containerGameObject.transform.position.y + yOffset, containerGameObject.transform.position.z);
        containedItemGameObject.transform.position = newPosition;
        containedItemGameObject.transform.parent = containerGameObject.transform;
    }

    public void UpdateModel()
    {
        if ((containedItemID == -1 || containerGameObject == null) && lastModel != null)
        {
            lastModel.SetActive(false);
            lastModel = null;
        }
        else
        {
            if(lastModel != null)
                lastModel.SetActive(false);
            BaseIngredientScript containedItemIngredientScript = containedItemGameObject.GetComponent<BaseIngredientScript>();
            int ingredientState1 = containedItemIngredientScript.ShowState1();
            int ingredientState2 = containedItemIngredientScript.ShowState2();
            int ingredientState3 = containedItemIngredientScript.ShowState3();

            switch (ingredientState1)
            {
                case 0:
                    switch (ingredientState2)
                    {
                        case 0:
                            switch (ingredientState3)
                            {
                                case -1:
                                    burnedModel.SetActive(true);
                                    lastModel = burnedModel;
                                    break;
                                case 0:
                                    ingredientModels000[containedItemIndex].SetActive(true);
                                    lastModel = ingredientModels000[containedItemIndex];
                                    break;
                                case 1:
                                    ingredientModels001[containedItemIndex].SetActive(true);
                                    lastModel = ingredientModels001[containedItemIndex];
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
