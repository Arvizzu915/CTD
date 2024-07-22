using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTableState : IStationState
{
    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

    private Transform stationTransform;
    private float yOffset = 1f;

    public BasicTableState(Transform stationTransform)
    {
        this.stationTransform = stationTransform;
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        if (ID == -1 || gameObject == null)
            return 0;
        if (containedItemID == -1)
        {
            containedItemID = ID;
            containedItemGameObject = gameObject;
            SetObjectInStation();
            return 1;
        }
        else if (containedItemID >= 100 && containedItemID < 200)
        {
            BaseContainerScript containedContainerScript = containedItemGameObject.GetComponent<BaseContainerScript>();
            if(containedItemID >= 105 || containedContainerScript.GetContainedItemID() == -1 || ID >= 200)
            {
                //al menos que lo que este dentro sea un plato principal con algo dentro, puede ver si lo que viene se puede meter al contenedor
                switch(containedContainerScript.CanEnterContainer(ID, gameObject))
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                }
            }
        }
        else if((containedItemID >= 200 && containedItemID <300) && (ID >= 200 && ID < 300))
        {
            //esto es para empanizar fuera del plato
            BaseIngredientScript ingredientScript = gameObject.GetComponent<BaseIngredientScript>();
            BaseIngredientScript containedIngredientScript = containedItemGameObject.GetComponent<BaseIngredientScript>();
            int ingredientChangeState2ID = ingredientScript.ShowChangeState2ID();
            //ve si tienen el mismo changeState2ID, y que no sea -1
            if (ingredientChangeState2ID == containedIngredientScript.ShowChangeState2ID() && ingredientChangeState2ID != -1)
            {
                //Aca checa cual es el que es el changer (como el pan) para ver a cual se le cambia el estado, si ninguno es el changer significa que ninguno cambia al otro, por lo que retorna 0
                if (ingredientScript.ShowIfIsState2Changer() && containedIngredientScript.ShowIfIsState2Changer())
                    return 0;
                if (ingredientScript.ShowIfIsState2Changer())
                {
                    //Si es el ingrediente entrante el changer, entonces solo le cambia el estado 2 al ingrediente del plato y elimina al ingrediente entrante
                    containedIngredientScript.ChangeState2(ingredientChangeState2ID, true);
                    Object.Destroy(gameObject);
                    return 1;
                }
                else if (containedIngredientScript.ShowIfIsState2Changer())
                {
                    //Si el changer es el ingrediente de la mesa, entonces vacia la mesa, elimina el objeto dentro, le cambia el estado al ingrediente entrante y lo mete a la mesa
                    Object.Destroy(containedItemGameObject);
                    EmptyStation();
                    ingredientScript.ChangeState2(ingredientChangeState2ID, true);
                    containedItemID = ID;
                    containedItemGameObject = gameObject;
                    SetObjectInStation();
                    return 1;
                }
            }
        }
        return 0;
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
        //Nada
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
