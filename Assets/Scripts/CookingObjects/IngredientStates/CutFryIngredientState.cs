using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFryIngredientState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ingredientModels00;
    // 0 - default, default
    // 1 - default, freido
    private GameObject[] ingredientModels10;
    // 0 - cortado, default
    // 1 - cortado, freido
    private GameObject lastModel;

    [SerializeField]
    private int ingredientState1 = 0;
    private int ingredientState3 = 0;

    //Solo 1 de estas a la vez
    // 0 - default / completo
    // 1 - cortado

    //Solo 1 de estas a la vez
    // 0 - default
    // 2 - freido

    [SerializeField]
    private int[] _102MixIDs;
    private int[] currentMixIDs = new int[0];

    public void ChangeState1(int newState)
    {
        ingredientState1 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixIDs();
    }

    public void ChangeState3(int newState)
    {
        ingredientState3 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixIDs();
    }

    private void ChangeMixIDs()
    {
        if (ingredientState1 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _102MixIDs;
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

    public int ShowState3()
    {
        return ingredientState3;
    }

    public int[] ShowMixIDs()
    {
        return currentMixIDs;
    }

    private void ShowModel()
    {
        switch (ingredientState1)
        {
            case 0:
                switch (ingredientState3)
                {
                    case 0:
                        ingredientModels00[0].SetActive(true);
                        lastModel = ingredientModels00[0];
                        break;
                    case 1:
                        ingredientModels00[1].SetActive(true);
                        lastModel = ingredientModels00[1];
                        break;
                }
                break;
            case 1:
                switch (ingredientState3)
                {
                    case 0:
                        ingredientModels10[0].SetActive(true);
                        lastModel = ingredientModels10[0];
                        break;
                    case 1:
                        ingredientModels10[1].SetActive(true);
                        lastModel = ingredientModels10[1];
                        break;
                }
                break;
        }
    }
}
