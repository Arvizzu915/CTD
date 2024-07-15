using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlateState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] turretModels;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;

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
        //La razon por la que solo se checa el id y no el gameobject, es porque asumimos que si no tiene id tampoco tiene game object, y si tiene id, entonces tiene gameobject
        if (objectID == -1)
            return false;
        //Quiza todo esto podria ir despues del primer if, ya que no hace falta hacer todo eso si nisiquiera se va a usar
        BaseIngredientScript objectIngredientScript = objectGameObject.GetComponent<BaseIngredientScript>();
        int[] objectMixIDs = objectIngredientScript.ShowMixIDs(); //se usa 6 veces
        int[] objectContainerIDs = objectIngredientScript.ShowContainerIDs(); //se usa 2 veces
        int objectChangeState2ID = objectIngredientScript.ShowChangeState2ID(); //se usa 3 veces
        bool objectIsState2Changer = objectIngredientScript.ShowIfIsState2Changer(); //solo 1 uso
        BaseIngredientScript containedItemIngredientScript = containedItemGameObject.GetComponent<BaseIngredientScript>();
        int[] containedItemMixIDs = containedItemIngredientScript.ShowMixIDs(); //se usa 3 veces
        //ni usa el MainContainerID del item dentro
        int containedItemChangeState2ID = containedItemIngredientScript.ShowChangeState2ID(); //solo 1 uso
        bool containedItemIsState2Changer = containedItemIngredientScript.ShowIfIsState2Changer(); //solo 1 uso

        bool canEnter = false;
        for (int i = 0; i < objectContainerIDs.Length; i++)
        {
            if (objectContainerIDs[i] == 0)
                canEnter = true;
        }
        //solo se pueden meter ingredientes base, regulares, especias, platillos y torres al plato, y solo cosas que se puedan poner en plato (no vasos o bowl)
        if (!canEnter)
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
                    case 2:
                        containedItemID = 502;
                        break;
                    case 5:
                        containedItemID = 505;
                        break;
                }
            }
            if(containedItemID == -1)
            {
                //Si no encontro nada que coincida en todo el arreglo de MixIDs, entonces hace lo basico
                containedItemID = objectID;
                containedItemGameObject = objectGameObject;
                SetModelInPlate();
            }
            else
            {
                //Si sí encontro, entonces nomas invoca a la torre
                containedItemGameObject = turretModels[containedItemID - 500];
                containedItemGameObject.SetActive(true);
            }
            return true;
        }
        else
        {
            if (objectMixIDs.Length == 0 || containedItemMixIDs.Length == 0)
            {
                //Si tienen mismo id de cambio de estado, entonces dale, sino pal lobby
                if (objectChangeState2ID == containedItemChangeState2ID && objectChangeState2ID != 0)
                {
                    //Aca checa cual es el que es el changer (como el pan) para ver a cual se le cambia el estado, si ninguno es el changer, entonces da false
                    if (objectIsState2Changer)
                    {
                        return CanChangeContainedItemState2(objectChangeState2ID);
                    }
                    else if (containedItemIsState2Changer)
                    {
                        EmptyContainer(true);
                        containedItemID = objectID;
                        containedItemGameObject = objectGameObject;
                        SetModelInPlate();
                        return CanChangeContainedItemState2(objectChangeState2ID);
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                //2 fors donde compara cada id de las listas entre si, hasta encontrar uno donde sean iguales y esten en el case, si es asi entonces pone la torre correspondiente y returna true
                for (int i = 0; i < objectMixIDs.Length; i++)
                {
                    for (int i2 = 0; i2 < containedItemMixIDs.Length; i2++)
                    {
                        if (objectMixIDs[i] == containedItemMixIDs[i2])
                        {
                            // OJO, aqui en teoria siempre deberia encontrar algo en el case, ya que solo se busca si ambos coinciden en el plato, no se en que caso no
                            // Pero hay que tener en cuenta por si luego pasa algo raro, habria que hacerle para que solo si encuentra algo en el case, ponga la torre, sino no
                            EmptyContainer(true);
                            switch (objectMixIDs[i])
                            {
                                case 0:
                                    containedItemID = 500;
                                    break;
                                case 3:
                                    containedItemID = 503;
                                    break;
                            }
                            containedItemGameObject = turretModels[containedItemID - 500];
                            containedItemGameObject.SetActive(true);
                            return true;
                        }
                    }
                }
                //Si no encontro nada o no coincidieron los 2, entonces returna false
                //La razon por la que no checo si es pan cortado (para empanizar), es porque en teoria ese nunca tendra un mixID, ya que su funcion es cambiar estados, no ser juntado, pero si cambia
                //en un futuro, entonces aqui habria que agregar los ifs respectivos. (iguales a los del if de arriba donde checa eso mismo.
                return false;
            }
        }
    }

    private void SetModelInPlate()
    {
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        //containedItemGameObject = Instantiate(objectGameObject, newPosition, Quaternion.identity, this.transform);
        containedItemGameObject.transform.position = newPosition;
        containedItemGameObject.transform.parent = this.transform;
        containedItemGameObject.GetComponent<BaseIngredientScript>().ShowModel();

    }

    public bool CanChangeContainedItemState2(int newState)
    {
        //esta funcion no se porque la hize booleana, pero otro dia vere si en verdad es necesario
        containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeState2(newState, true);
        return true;
    }

    public void EmptyContainer(bool deleteModel)
    {
        if (deleteModel && containedItemID < 400)
        {
            Destroy(containedItemGameObject);
        }
        if(containedItemID >= 400)
        {
            containedItemGameObject.SetActive(false);
        }
        containedItemID = -1;
        containedItemGameObject = null;

    }
}
