using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContainerScript : MonoBehaviour
{
    //Ambos
    IContainerState containerState;
    [SerializeField]
    private int containerID = 100;

    //Contenedores Generales (plato, vaso)
    [SerializeField]
    private GameObject[] turretModels;

    //Contenedores Especificos (olla, canastilla)
    [SerializeField]
    private GameObject burnedModel;
    [SerializeField]
    private GameObject[] ingredientModels000, ingredientModels010, ingredientModels100, ingredientModels110;
    [SerializeField]
    private GameObject[] ingredientModels001, ingredientModels012, ingredientModels102, ingredientModels112;
    [SerializeField]
    private int[] ingredientsIDs; //este no sirve tanto para ver que ingredientes pueden entrar, sino para buscar al ingrediente dentro del arreglo, y usar el su index para activar modelos de los otros arreglos


    void Start()
    {
        switch (containerID)
        {
            case 100:
                containerState = new FlatPlateState(containerID, this.gameObject, turretModels);
                break;
            case 105:
                containerState = new PotState(containerID, this.gameObject, burnedModel, ingredientModels000, ingredientModels001, ingredientsIDs);
                break;
        }
    }

    public int GetContainedItemID()
    {
        return containerState.GetContainedItemID();
    }

    public GameObject GetContainedItemGameObject()
    {
        return containerState.GetContainedItemGameObject();
    }

    public void EmptyContainer(bool deleteContainedItemGameObject)
    {
        containerState.EmptyContainer(deleteContainedItemGameObject);
    }

    public int CanEnterContainer(int ID, GameObject gameObject)
    {
        return containerState.CanEnterContainer(ID, gameObject);
    }

    public void UpdateModel()
    {
        containerState.UpdateModel();
    }
}
