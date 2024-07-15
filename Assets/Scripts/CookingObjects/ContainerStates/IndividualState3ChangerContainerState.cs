using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualState3ChangerContainerState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ingredientModels000, ingredientModels001, ingredientModels010, ingredientModels012, ingredientModels100, ingredientModels102, ingredientModels110, ingredientModels112;
    [SerializeField]
    private GameObject burnedModel;
    private GameObject lastModel;

    [SerializeField]
    private int[] acceptedIDs;

    [SerializeField]
    private int newIngredientState3;

    private int containedItemID = -1;
    private int containedItemIndex = -1;
    private GameObject containedItemGameObject = null;
    private bool isReady = false;

    public int GetContainedItemID()
    {
        return containedItemID;
    }

    public GameObject GetContainedItemGameObject()
    {
        return containedItemGameObject;
    }

    public bool CanPlaceObjectInContainer(int objectID, GameObject objectGameObject)
    {
        if (objectID == -1)
            return false;
        BaseIngredientScript objectIngredientScript = objectGameObject.GetComponent<BaseIngredientScript>();
        int[] objectContainerIDs = objectIngredientScript.ShowContainerIDs(); //se usa 2 veces

        bool canEnter = false;
        for (int i = 0; i < objectContainerIDs.Length; i++)
        {
            if (objectContainerIDs[i] == 0)
                canEnter = true;
        }
        if (containedItemID != -1 || !canEnter)
            return false;

        //Si esta vacio el container y tiene el id de este container, entonces puede entrar
        containedItemID = objectID;
        for (int i = 0; i < acceptedIDs.Length; i++)
        {
            if (acceptedIDs[i] == containedItemID)
                containedItemIndex = i;
        }
        containedItemGameObject = objectGameObject;
        isReady = false;
        HideOriginalModel();
        ShowModel();
        return true;
    }

    public bool IsReady()
    {
        return isReady;
    }

    public bool IngredientIsReady()
    {
        return containedItemGameObject.GetComponent<BaseIngredientScript>().IsReady();
    }

    public void ChangeState3OfContainedItem()
    {
        HideModel();
        containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeState3(newIngredientState3, false);
        isReady = true;
        ShowModel();
    }

    public void ChangeProcessTimeFromContainedItem(bool isBurning)
    {
        if (!isBurning)
        {
            containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeProcessTime(3, newIngredientState3);
        }
        else
        {
            containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeProcessTime(666, newIngredientState3);
        }
    }

    public void ChangeProcessPercentFromContainedItem()
    {
        containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeProcessPercent();
    }

    public void ChangeToBurnedContainer()
    {
        containedItemID = -2;
        containedItemIndex = -1;
        HideModel();
        Destroy(containedItemGameObject);
        containedItemGameObject = null;
        burnedModel.SetActive(true);
        lastModel = burnedModel;
        isReady = false;
    }

    public void EmptyContainer()
    {
        containedItemID = -1;
        containedItemIndex = -1;
        HideModel();
        containedItemGameObject = null;
        isReady = false;
    }

    private void HideOriginalModel()
    {
        containedItemGameObject.GetComponent<BaseIngredientScript>().HideModel();
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        containedItemGameObject.transform.position = newPosition;
        containedItemGameObject.transform.parent = this.transform;
    }

    private void ShowModel()
    {
        BaseIngredientScript containedItemIngredientScript = containedItemGameObject.GetComponent<BaseIngredientScript>();
        int ingredientState1 = containedItemIngredientScript.ShowState1();
        int ingredientState2 = containedItemIngredientScript.ShowState2();
        int ingredientState3 = containedItemIngredientScript.ShowState3();
        
        //Falta que pueda dar sus datos, y muy importante la variable de finish, para conexion con estaciones, y el finishState3
        switch (ingredientState1)
        {
            case 0:
                switch (ingredientState2)
                {
                    case 0:
                        switch (ingredientState3)
                        {
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
                    case 1:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels010[containedItemIndex].SetActive(true);
                                lastModel = ingredientModels010[containedItemIndex];
                                break;
                            case 2:
                                ingredientModels012[containedItemIndex].SetActive(true);
                                lastModel = ingredientModels012[containedItemIndex];
                                break;
                        }
                        break;
                }
                break;
            case 1:
                switch (ingredientState2)
                {
                    case 0:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels100[containedItemIndex].SetActive(true);
                                lastModel = ingredientModels100[containedItemIndex];
                                break;
                            case 2:
                                ingredientModels102[containedItemIndex].SetActive(true);
                                lastModel = ingredientModels102[containedItemIndex];
                                break;
                        }
                        break;
                    case 1:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels110[containedItemIndex].SetActive(true);
                                lastModel = ingredientModels110[containedItemIndex];
                                break;
                            case 2:
                                ingredientModels112[containedItemIndex].SetActive(true);
                                lastModel = ingredientModels112[containedItemIndex];
                                break;
                        }
                        break;
                }
                break;
        }
    }

    private void HideModel()
    {
        if (lastModel != null)
            lastModel.SetActive(false);
    }
}
