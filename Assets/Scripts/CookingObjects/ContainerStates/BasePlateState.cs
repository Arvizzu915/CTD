using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlateState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] turretModels;

    private int containedItemID;
    private int containedItemState1;
    private int containedItemState2;
    private int containedItemState3;
    private int containedItemMixID;
    private GameObject containedItemModel;

    public bool CanPlaceObjectInContainer(int objectID, int objectState1, int objectState2, int objectState3, int objectMixID, GameObject objectModel)
    {
        //solo se pueden meter ingredientes base, regulares, especias, platillos y torres al plato
        if (objectID < 200)
            return false;
        //Por ahora esto solo se separa en plato vacio y plato con algo (para ver que hacer en 1 ingrediente o en 2, no mas)
        if(containedItemID == -1)
        {
            if (objectMixID != -1)
            {
                EmptyPlate();
                switch (containedItemMixID)
                {
                    case 0:
                        containedItemID = 500;
                        break;
                }
                containedItemModel = turretModels[containedItemID - 500];
                containedItemModel.SetActive(true);
            }
            //switch (objectID)
            //{
            //    case 300:
            //        if (objectState1 == 0 && objectState2 == 1 && objectState3 == 2)
            //        {
            //            EmptyPlate();
            //            containedItemID = 501;
            //            containedItemModel = turretModels[containedItemID - 500];
            //            containedItemModel.SetActive(true);
            //        }
            //        break;
            //}
            containedItemID = objectID;
            containedItemState1 = objectState1;
            containedItemState2 = objectState2;
            containedItemState3 = objectState3;
            SetModelInPlate(objectModel);
            return true;
        }
        else
        {
            if(containedItemMixID == objectMixID && containedItemMixID != -1)
            {
                EmptyPlate();
                switch (containedItemMixID)
                {
                    case 0:
                        containedItemID = 500;
                        break;
                }
                containedItemModel = turretModels[containedItemID - 500];
                containedItemModel.SetActive(true);
            }
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
                }
                break;
            default:
                return false;
        }
        return false;
    }

    private void EmptyPlate()
    {
        containedItemID = -1;
        containedItemState1 = -1;
        containedItemState2 = -1;
        containedItemState3 = -1;
        containedItemMixID = -1;
        Destroy(containedItemModel);
        containedItemModel = null;

    }
}
