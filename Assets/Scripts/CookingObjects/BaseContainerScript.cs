using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContainerScript : MonoBehaviour
{
    //Este es el script que menos me gusta, ya que hay muchos arreglos y booleanas que no todos los recipientes usan, ademas de que quiza podria hacerse de una manera mas inteligente, pero no le se,
    //la unica alternativa que se me ocurre es hacer un script diferente para cada recipiente, pero no se si sea mejor o peor.
    //Quiza la mejor opcion sea que instancien o muevan o algo asi, el ingrediente directamente al plato, asi no tiene que guardar los modelos de cada ingrediente con cada estado que puede contener.

    //Todos los modelos de los objetos que puede tener dentro
    [SerializeField]
    private GameObject[] towersInContainer, defaultIngredientsInContainer, cutIngredientsInContainer, cookedIngredientsInContainer, defaultSpicesInContainer, cutSpicesInContainer, cookedSpicesInContainer;
    //Esto es mas para ver que puede recibir/entrar, no tanto el que puede contener (ej. puede contener una torre, pero no recibir una)
    [SerializeField]
    private bool canContainIngredients, canContainSpices, canContainDefault, canContainCut, canContainCooked;
    //Esto es mas para pasarle información a otros recipientes o estaciones
    private List<GameObject> containedGameObjects = new();
    private int towerInContainerID = -1;

    public bool CanPlaceGameObjectsInContainer(GameObject[] gameObjects)
    {
        if (towerInContainerID != -1)
            return false;
        foreach (GameObject gameObject in gameObjects)
        {
            int gameObjectID = gameObject.GetComponent<BaseIngredientScript>().GetIngredientID();
            BaseIngredientScript.IngredientState gameObjectState = gameObject.GetComponent<BaseIngredientScript>().GetCurrentState();
            if ((gameObjectID >= 200 && gameObjectID < 300) && !canContainIngredients)
                return false;
            if ((gameObjectID >= 300 && gameObjectID < 400) && !canContainSpices)
                return false;
            if (gameObjectState == BaseIngredientScript.IngredientState.defaultState && !canContainDefault)
                return false;
            if (gameObjectState == BaseIngredientScript.IngredientState.cutState && !canContainCut)
                return false;
            if (gameObjectState == BaseIngredientScript.IngredientState.cookedState && !canContainCooked)
                return false;
            foreach (GameObject containedGameObject in containedGameObjects)
            {
                if (gameObjectID == containedGameObject.GetComponent<BaseIngredientScript>().GetIngredientID())
                    return false;
            }
        }
        //Si todos los objetos pueden entrar, los agrega todos, si tan solo 1 objeto no puede entrar, entonces no deja.
        foreach (GameObject gameObject in gameObjects)
        {
            containedGameObjects.Add(gameObject);
            ShowGameObjectInContainer(gameObject.GetComponent<BaseIngredientScript>().GetIngredientID(), gameObject.GetComponent<BaseIngredientScript>().GetCurrentState());
        }
        return true;
    }

    private void ShowGameObjectInContainer(int id, BaseIngredientScript.IngredientState ingredientState)
    {
        if (id >= 200 && id < 300)
        {
            switch (ingredientState)
            {
                case BaseIngredientScript.IngredientState.defaultState:
                    defaultIngredientsInContainer[id - 200].SetActive(true);
                    break;
                case BaseIngredientScript.IngredientState.cutState:
                    cutIngredientsInContainer[id - 200].SetActive(true);
                    break;
                case BaseIngredientScript.IngredientState.cookedState:
                    cookedIngredientsInContainer[id - 200].SetActive(true);
                    break;
            }
        }
        else if (id >= 300 && id < 400)
        {
            switch (ingredientState)
            {
                case BaseIngredientScript.IngredientState.defaultState:
                    defaultSpicesInContainer[id - 300].SetActive(true);
                    break;
                case BaseIngredientScript.IngredientState.cutState:
                    cutSpicesInContainer[id - 300].SetActive(true);
                    break;
                case BaseIngredientScript.IngredientState.cookedState:
                    cookedSpicesInContainer[id - 300].SetActive(true);
                    break;
            }
        }
    }

    public void EmptyContainer(bool deleteGameObjects)
    {
        if (deleteGameObjects)
        {
            foreach (GameObject gameObject in containedGameObjects)
            {
                HideGameObjectInContainer(gameObject.GetComponent<BaseIngredientScript>().GetIngredientID(), gameObject.GetComponent<BaseIngredientScript>().GetCurrentState());
                Destroy(gameObject);
            }
        }
        containedGameObjects.Clear();
    }

    private void HideGameObjectInContainer(int id, BaseIngredientScript.IngredientState ingredientState)
    {
        if (id >= 200 && id < 300)
        {
            switch (ingredientState)
            {
                case BaseIngredientScript.IngredientState.defaultState:
                    defaultIngredientsInContainer[id - 200].SetActive(false);
                    break;
                case BaseIngredientScript.IngredientState.cutState:
                    cutIngredientsInContainer[id - 200].SetActive(false);
                    break;
                case BaseIngredientScript.IngredientState.cookedState:
                    cookedIngredientsInContainer[id - 200].SetActive(false);
                    break;
            }
        }
        else if (id >= 300 && id < 400)
        {
            switch (ingredientState)
            {
                case BaseIngredientScript.IngredientState.defaultState:
                    defaultSpicesInContainer[id - 300].SetActive(false);
                    break;
                case BaseIngredientScript.IngredientState.cutState:
                    cutSpicesInContainer[id - 300].SetActive(false);
                    break;
                case BaseIngredientScript.IngredientState.cookedState:
                    cookedSpicesInContainer[id - 300].SetActive(false);
                    break;
            }
        }
    }

    public void GetTowerInContainer(int towerID)
    {

    }
}
