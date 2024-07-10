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
    private int[] containedItemMixIDs;
    private GameObject containedItemModel;

    public bool CanPlaceObjectInContainer(int objectID, int objectState1, int objectState2, int objectState3, int[] objectMixIDs, int objectMainContainerID, GameObject objectModel)
    {
        //solo se pueden meter ingredientes base, regulares, especias, platillos y torres al plato, y solo cosas que se puedan poner en plato (no vasos o bowl)
        if (objectID < 200 || objectMainContainerID != 0)
            return false;
        //Por ahora esto solo se separa en plato vacio y plato con algo (para ver que hacer en 1 ingrediente o en 2, no mas)
        if(containedItemID == -1)
        {
            for (int i = 0; i < objectMixIDs.Length; i++)
            {
                switch (objectMixIDs[i])
                {
                    case 1:
                        containedItemID = 501;
                        break;
                }
            }
            if(containedItemID == -1)
            {
                //Si no encontro nada que coincida en todo el arreglo de MixIDs, entonces hace lo basico
                containedItemID = objectID;
                containedItemState1 = objectState1;
                containedItemState2 = objectState2;
                containedItemState3 = objectState3;
                containedItemMixIDs = objectMixIDs;
                SetModelInPlate(objectModel);
            }
            else
            {
                //Si sí encontro, entonces nomas invoca a la torre
                containedItemModel = turretModels[containedItemID - 500];
                containedItemModel.SetActive(true);
            }
            return true;
        }
        else
        {
            //Aca hay que hacer 2 fors
            //if(containedItemMixIDs == objectMixID && containedItemMixIDs != -1)
            //{
            //    EmptyPlate();
            //    switch (containedItemMixIDs)
            //    {
            //        case 0:
            //            containedItemID = 500;
            //            break;
            //    }
            //    containedItemModel = turretModels[containedItemID - 500];
            //    containedItemModel.SetActive(true);
            //}
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
        containedItemMixIDs = new int[0];
        Destroy(containedItemModel);
        containedItemModel = null;

    }
}
