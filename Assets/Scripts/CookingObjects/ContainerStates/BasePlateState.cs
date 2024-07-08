using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlateState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dishModels;

    private int containedItemID;
    private int containedItemState;
    private GameObject containedItemModel;

    public bool CanPlaceObjectInContainer(int objectID, int objectState, GameObject objectModel)
    {
        //solo se pueden meter ingredientes base, regulares, especias, platillos y torres al plato
        if (objectID < 200)
            return false;
        if(containedItemID == -1)
        {
            containedItemID = objectID;
            containedItemState = objectState;
            SetModelInPlate(objectModel);
            return true;
        }
        else if((objectID >= 200 && objectID < 300) && (containedItemID >= 300 && containedItemID < 400))
        {

        }
        //si no es un ingrediente base o regular, y ya hay algo en el plato, entonces no puede ya meter nada
        if (objectID >= 400 && containedItemID != -1)
            return false;
        //if (fullItemID != -1 || (baseIngredientID != -1 && regularIngredientID != -1))
        //    return false;
        //int objectIndex = acceptedObjectsIDs.FindIndex(ID => ID == objectID);
        //if (objectIndex == -1)
        //    return false;
        //if (objectID >= 500 && (baseIngredientID == -1 && regularIngredientID == -1))
        //{
        //    //Si se le quiere poner un platillo o una torre, no tiene que tener nada
        //    fullItemID = objectID;
        //}
        //else if ((objectID >= 200 && objectID < 300) && baseIngredientID == -1)
        //{
        //    baseIngredientID = objectID;
        //}
        //else if ((objectID >= 300 && objectID < 400) && regularIngredientID == -1)
        //{
        //    regularIngredientID = objectID;
        //}

        //if(baseIngredientID != -1 && regularIngredientID != -1)
        //{
        //    //Si tiene el base y el regular, entonces comprueba que sean compatibles
        //}
        //currentContainedObjectID = objectID;
        //currentContainedObjectIndex = objectIndex;
        //isFinished = false;
        //ShowModelForID();
        return true;
    }

    private void SetModelInPlate(GameObject objectModel)
    {
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        containedItemModel = Instantiate(objectModel, newPosition, Quaternion.identity, this.transform);
    }

    private bool CanMixIngredients(int objectID)
    {
        //Aqui en teoria van todas las recetas
        //rayos, me acabo de dar cuenta de que esto va a estar del diavlo si tomamos en cuenta a los estados de los ingredientes
        switch (objectID)
        {
            case 200:
                switch (containedItemID)
                {
                    case 300:
                        return true;
                        break;
                }
                break;
            default:
                return false;
        }
        return false;
    }
}
