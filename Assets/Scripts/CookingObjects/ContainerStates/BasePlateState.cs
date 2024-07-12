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
    private int containedItemMainContainerID;
    private int containedItemChangeState2ID;
    private GameObject containedItemModel;

    public bool CanPlaceObjectInContainer(int objectID, int objectState1, int objectState2, int objectState3, int[] objectMixIDs, int objectMainContainerID, int objectChangeState2ID, GameObject objectModel)
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
            if (objectMixIDs.Length == 0 || containedItemMixIDs.Length == 0)
            {
                //Si tienen mismo id de cambio de estado, entonces dale, sino pal lobby
                if (objectChangeState2ID == containedItemChangeState2ID && objectChangeState2ID != 0)
                {
                    //Aca este if podria resolverse si juntamos todos los ingredientes en uno, para que solo sea poner:
                    //if(containedItemModel.GetComponent<CutBreadFryIngredientState>().ChangeState2(objectChangeState2ID) == false)
                    //{
                    //    containedItemID = objectID;
                    //    containedItemState1 = objectState1;
                    //    containedItemState2 = objectState2;
                    //    containedItemState3 = objectState3;
                    //    containedItemMixIDs = objectMixIDs;
                    //    SetModelInPlate(objectModel);
                    //    containedItemModel.GetComponent<CutBreadFryIngredientState>().ChangeState2(objectChangeState2ID);

                    //Aca iria lo de isChanger
                    if(objectID == 201)
                    {
                        containedItemModel.GetComponent<BaseIngredientScript>().ChangeState2(objectChangeState2ID);
                    }
                    //}
                    return true;
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
                            EmptyPlate();
                            switch (objectMixIDs[i])
                            {
                                case 0:
                                    containedItemID = 500;
                                    break;
                                case 3:
                                    containedItemID = 503;
                                    break;
                            }
                            containedItemModel = turretModels[containedItemID - 500];
                            containedItemModel.SetActive(true);
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

    private void SetModelInPlate(GameObject objectModel)
    {
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
        containedItemModel = Instantiate(objectModel, newPosition, Quaternion.identity, this.transform);
    }

    private void EmptyPlate()
    {
        containedItemID = -1;
        containedItemState1 = -1;
        containedItemState2 = -1;
        containedItemState3 = -1;
        containedItemMixIDs = new int[0];
        containedItemMainContainerID = 0;
        containedItemChangeState2ID = 0;
        Destroy(containedItemModel);
        containedItemModel = null;

    }
}
