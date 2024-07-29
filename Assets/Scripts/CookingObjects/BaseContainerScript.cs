using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContainerScript : MonoBehaviour
{
    //Ambos
    IContainerState containerState;
    [SerializeField]
    public int containerID = 100;//por ahora la hize publica, pero podria ser privada y solo tener una funcion extra que devuelva su valor

    //Contenedores Generales (plato, vaso)
    [SerializeField]
    private GameObject[] turretModels;

    //Contenedores Especificos (olla, canastilla)
    [SerializeField]
    private BasicSliderScript cookingSlider;
    [SerializeField]
    private GameObject burnedModel;
    [SerializeField]
    private GameObject[] ingredientModels000, ingredientModels001, ingredientModels002;
    [SerializeField]
    private GameObject[] ingredientModels010, ingredientModels012;
    [SerializeField]
    private GameObject[] ingredientModels100, ingredientModels102;
    [SerializeField]
    private GameObject[] ingredientModels110, ingredientModels112;
    [SerializeField]
    private int[] ingredientsIDs; //este no sirve tanto para ver que ingredientes pueden entrar, sino para buscar al ingrediente dentro del arreglo, y usar el su index para activar modelos de los otros arreglos


    void Start()
    {
        switch (containerID)
        {
            case 100:
                containerState = new FlatPlateState(containerID, this.gameObject, turretModels);
                break;
            case 101:
                containerState = new CupState(containerID, this.gameObject, turretModels);
                break;
            case 105:
                containerState = new PotState(containerID, this.gameObject, cookingSlider, burnedModel, ingredientModels000, ingredientModels001, ingredientsIDs);
                break;
            case 106:
                containerState = new FryingBasketState(containerID,
                                                       this.gameObject,
                                                       cookingSlider,
                                                       burnedModel,
                                                       ingredientModels000,
                                                       ingredientModels002,
                                                       ingredientModels010,
                                                       ingredientModels012,
                                                       ingredientModels100,
                                                       ingredientModels102,
                                                       ingredientModels110,
                                                       ingredientModels112,
                                                       ingredientsIDs);
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

    public void UpdateCookingSlider()
    {
        //por ahora esto esta aqui de manera random, no me gusta, pero no hace nada malo asi que por ahora aqui se queda
        cookingSlider.ChangeValue(Time.deltaTime);
    }
}
