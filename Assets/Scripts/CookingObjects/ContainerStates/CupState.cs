using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupState : IContainerState
{
    private GameObject[] turretModels; //aca tienen que venir todas las torres, aunque no use su modelo, tiene que estar en null, ya que dependen del index de la torre

    private int containerID;
    private GameObject containerGameObject;
    private float yOffset = 0.5f;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    public CupState(int containerID, GameObject containerGameObject, GameObject[] turretModels)
    {
        this.containerID = containerID;
        this.containerGameObject = containerGameObject;
        this.turretModels = turretModels;
    }

    public int CanEnterContainer(int ID, GameObject gameObject)
    {
        //si el objeto a entrar no existe o no tiene id valido, nel
        if (ID == -1 || gameObject == null)
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
            if (ingredientID == -1 || ingredientGameObject == null)
            {
                return 0;
            }
            else if (ingredientID >= 400 && ingredientID < 500)
            {
                //significa que esta entrando una torre del otro plato, y solo puede entrar si son el mismo tipo de plato y este plato esta vacio
                if (ID == containerID && (containedItemID == -1 && containedItemGameObject == null))
                {
                    containedItemID = ingredientID;
                    containedItemGameObject = turretModels[containedItemID - 400];
                    containedItemGameObject.SetActive(true);
                    gameObject.GetComponent<BaseContainerScript>().EmptyContainer(false);
                    return 2;
                }
                return 0;
            }
            else
            {
                //es un else y no un else if, ya que sabemos que el vaso solo puede tener ingredientes o torres (estamos ignorando especias)
                ingredientScript = ingredientGameObject.GetComponent<BaseIngredientScript>();
                returnInt = 2;
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
        int[] ingredientMixIDs = ingredientScript.ShowMixIDs();
        //Ahora vemos si el plato esta vacio o no
        if (containedItemID == -1)
        {
            //Si esta vacio, vemos si el ingrediente tiene algun mixID individual
            for (int i = 0; i < ingredientMixIDs.Length; i++)
            {
                switch (ingredientMixIDs[i])
                {
                    case 404:
                        containedItemID = 404;
                        break;
                }
            }
            if (containedItemID == -1)
            {
                //Si no encontro nada que coincida en todo el arreglo de MixIDs, entonces solo pone al ingrediente en el plato
                containedItemID = ingredientID;
                containedItemGameObject = ingredientGameObject;
                UpdateModel();
            }
            else
            {
                //Si sí encontro, entonces muestra la torre
                containedItemGameObject = turretModels[containedItemID - 400];
                containedItemGameObject.SetActive(true);
            }
            if (returnInt == 2)
                gameObject.GetComponent<BaseContainerScript>().EmptyContainer(false);
            return returnInt;
        }
        else
        {
            //si no esta vacio, entonces vamos a ver si el ingrediente puede mezclarse con lo que esta en el plato
            //Para empezar, si lo que esta en el plato es una torre, entonces nel
            if (containedItemID >= 400 && containedItemID < 500)
                return 0;
            int[] containedItemMixIDs = containedItemGameObject.GetComponent<BaseIngredientScript>().ShowMixIDs();
            //Si no es una torre (asumimos que es un ingrediente) vemos si comparten algun mixID para convertirse en una torre
            for (int i = 0; i < ingredientMixIDs.Length; i++)
            {
                for (int i2 = 0; i2 < containedItemMixIDs.Length; i2++)
                {
                    if (ingredientMixIDs[i] == containedItemMixIDs[i2])
                    {
                        //Si comparten un mismo mixID, entonces borramos lo que esta y lo que entra, y metemos la torre
                        EmptyContainer(true);
                        switch (ingredientMixIDs[i])
                        {
                            case 0:
                                //nada por ahora, porque no hay mezclas en el vaso por ahora, pero en un futuro quiza hayan
                                break;
                        }
                        containedItemGameObject = turretModels[containedItemID - 400];
                        containedItemGameObject.SetActive(true);
                        if (returnInt == 1)
                        {
                            Object.Destroy(ingredientGameObject);
                        }
                        else if (returnInt == 2)
                        {
                            gameObject.GetComponent<BaseContainerScript>().EmptyContainer(true);
                        }
                        return returnInt;
                    }
                }
            }
            //Si no coincidieron en ningun mixID, entonces toca ver si comparten algun ChangeState2ID
            int ingredientChangeState2ID = ingredientScript.ShowChangeState2ID();
            //ve si tienen el mismo changeState2ID, y que no sea -1
            if (ingredientChangeState2ID == containedItemGameObject.GetComponent<BaseIngredientScript>().ShowChangeState2ID() && ingredientChangeState2ID != -1)
            {
                //Aca checa cual es el que es el changer (como el pan) para ver a cual se le cambia el estado, si ninguno es el changer significa que ninguno cambia al otro, por lo que retorna false
                if (ingredientScript.ShowIfIsState2Changer())
                {
                    //Si es el ingrediente entrante el changer, entonces solo le cambia el estado 2 al ingrediente del plato y elimina al ingrediente entrante
                    containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeState2(ingredientChangeState2ID, true);
                    if (returnInt == 1)
                    {
                        Object.Destroy(ingredientGameObject);
                    }
                    else if (returnInt == 2)
                    {
                        gameObject.GetComponent<BaseContainerScript>().EmptyContainer(true);
                    }
                    return returnInt;
                }
                else if (containedItemGameObject.GetComponent<BaseIngredientScript>().ShowIfIsState2Changer())
                {
                    //Si el changer es el ingrediente del plato, entonces vacia el plato totalmente, le cambia el estado al ingrediente entrante y lo mete al plato
                    EmptyContainer(true);
                    ingredientScript.ChangeState2(ingredientChangeState2ID, true);
                    containedItemID = ingredientID;
                    containedItemGameObject = ingredientGameObject;
                    UpdateModel();
                    if (returnInt == 2)
                        gameObject.GetComponent<BaseContainerScript>().EmptyContainer(false);
                    return returnInt;
                }
            }
            return 0;
        }
    }

    public void EmptyContainer(bool deleteContainedItemGameObject)
    {
        if (deleteContainedItemGameObject && containedItemID < 400)
            Object.Destroy(containedItemGameObject);
        if (containedItemID >= 400 && containedItemID < 500)
            containedItemGameObject.SetActive(false);
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

    public void UpdateModel()
    {
        if (containedItemGameObject != null)
        {
            Vector3 newPosition = new Vector3(containerGameObject.transform.position.x, containerGameObject.transform.position.y + yOffset, containerGameObject.transform.position.z);
            containedItemGameObject.transform.position = newPosition;
            containedItemGameObject.transform.rotation = containerGameObject.transform.rotation;
            containedItemGameObject.transform.parent = containerGameObject.transform;
            containedItemGameObject.GetComponent<BaseIngredientScript>().ShowModel();
        }
    }
}
