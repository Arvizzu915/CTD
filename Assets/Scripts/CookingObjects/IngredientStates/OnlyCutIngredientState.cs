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

    [SerializeField]
    private int[] _100MixIDs;
    private int[] currentMixIDs = new int[0];

    public void ChangeState1(int newState)
    {
        ingredientState1 = newState;
        lastModel.SetActive(false);
        ingredientModels[ingredientState1].SetActive(true);
        lastModel = ingredientModels[ingredientState1];
        ChangeMixIDs();
    }

    private void ChangeMixIDs()
    {
        if (ingredientState1 == 1)
        {
            currentMixIDs = _100MixIDs;
        }
        else
        {
            currentMixIDs = new int[0];
        }
    }

    public int ShowState1()
    {
        return ingredientState1;
    }

    public int[] ShowMixIDs()
    {
        return currentMixIDs;
    }
}
