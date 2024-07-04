using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIngredientScript : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultModel = null, cutModel = null, cookedModel = null;
    private GameObject currentModel;

    [SerializeField]
    private int ingredientID;
    public enum IngredientState { defaultState, cutState, cookedState }
    [SerializeField]
    private IngredientState ingredientState = IngredientState.defaultState;


    void Start()
    {
        currentModel = defaultModel;
    }

    public void ChangeState(IngredientState newState)
    {
        ingredientState = newState;
        switch (ingredientState)
        {
            case IngredientState.defaultState:
                //Este quiza ni sea necesario ya que siempre empieza en defacult y no creo que haya forma de regresar al default una vez se pase a cut o cooked, pero pues lo dejo por si acaso
                currentModel.SetActive(false);
                defaultModel.SetActive(true);
                currentModel = defaultModel;
                break;
            case IngredientState.cutState:
                currentModel.SetActive(false);
                cutModel.SetActive(true);
                currentModel = cutModel;
                break;
            case IngredientState.cookedState:
                currentModel.SetActive(false);
                cookedModel.SetActive(true);
                currentModel = cookedModel;
                break;
        }
    }

    public int GetIngredientID()
    {
        return ingredientID;
    }

    public IngredientState GetCurrentState()
    {
        return ingredientState;
    }

    public bool CanBeCut()
    {
        if (cutModel == null)
            return false;
        return true;
    }

    public bool CanBeCooked()
    {
        if (cookedModel == null)
            return false;
        return true;
    }
}
