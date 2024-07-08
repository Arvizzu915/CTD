using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyCutIngredientState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ingredientModels;
    // 0 - default
    // 1 - cortado
    private GameObject lastModel;

    [SerializeField]
    private int ingredientState1 = 0;
    //Solo 1 de estas a la vez
    // 0 - default / completo
    // 1 - cortado

    public void ChangeState3(int newState)
    {
        ingredientState1 = newState;
        lastModel.SetActive(false);
        ingredientModels[ingredientState1].SetActive(true);
        lastModel = ingredientModels[ingredientState1];
    }

    public int ShowState3()
    {
        return ingredientState1;
    }
}
